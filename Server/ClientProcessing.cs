using DbLibrary;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Server
{
    /// <summary>
    /// Class which process all client messages sended here from ServerConnection,
    /// and gives back server response ready to send to client
    /// </summary>
    public class ClientProcessing
    {
        /// <summary>
        /// Delegate which represents function used to procces client data
        /// </summary>
        public delegate string Functions(string msg, int clientId);
        public delegate string AsyncFunctions(int clientId, string senderName);
        /// <summary>
        /// List of all avaliable syncFunctions, index of given function is number of that option
        /// </summary>
        public Dictionary<Options, Functions> syncFunctions;
        public Dictionary<Options, AsyncFunctions> asyncFunctions;
        public DbMethods dbMethods;
        public List<User> activeUsers;

        /// <summary>
        /// Contains list of functions to procces by client
        /// <para> Key: <c>username</c> </para> 
        /// <para> Value: <c>List(option,senderUsername)</c> </para>
        /// </summary>
        private Dictionary<string, List<Tuple<Options, string>>> whichFunction;

        /// <summary>
        /// <para> Key: <c>InvitationId</c> </para> 
        /// <para> Value: <c>Invitation</c> </para>
        /// </summary>
        public Dictionary<int, Invitation> invitations;

        /// <summary>
        /// <para> Key: <c>userId</c>  </para>
        /// <para>Value: <c>List(invitationId)</c>  </para>
        /// </summary>
        public Dictionary<int, List<int>> userInvitationsIds;

        /// <summary>
        /// <para> Key: <c>username</c> </para> 
        /// <para> Value: <c>handler</c> </para>
        /// </summary>
        private Dictionary<string, EventWaitHandle> eventHandlers;

        /// <summary>
        /// <para> Key: <c>clientId</c> </para>
        /// <para> Value: <c>handler</c> </para>
        /// </summary>
        private Dictionary<int, EventWaitHandle> userLoginHandler;

        /// <summary>
        /// Function that takes message from client procces it and return server response
        /// </summary>
        /// <param name="message"> Client message</param>
        /// <returns>Server response ready to send</returns>
        public string ProccesClient(string message, int clientId)
        {
            string[] fields = message.Split("$$", StringSplitOptions.RemoveEmptyEntries);
            int option = int.Parse(fields[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1]);

            lock (syncFunctions[(Options)option])
            {
                return syncFunctions[(Options)option](message, clientId);
            }
        }

        public List<string> CheckServerMessages(int clientId)
        {
            List<string> res = new List<string>();
            string item;

            // Async messages only to logged users
            if (!activeUsers[clientId].logged) userLoginHandler[clientId].WaitOne();
            // Wait unit event
            eventHandlers[activeUsers[clientId].username].WaitOne();
            lock (eventHandlers[activeUsers[clientId].username])
            {
                lock (whichFunction[activeUsers[clientId].username])
                {
                    foreach (var function in whichFunction[activeUsers[clientId].username])
                    {
                        item = asyncFunctions[function.Item1](clientId, function.Item2);
                        if (item != "") res.Add(item);
                    }
                    whichFunction[activeUsers[clientId].username].Clear();
                    eventHandlers[activeUsers[clientId].username].Reset();
                }
            }
            return res;
        }

        // Mark sended invitations to unseded
        public string Logout(string msg, int clientId)
        {
            lock (userLoginHandler) userLoginHandler[clientId].Reset();
            lock (activeUsers[clientId]) activeUsers[clientId].logged = false;
            lock (eventHandlers) eventHandlers[activeUsers[clientId].username].Reset();
            lock (userInvitationsIds[activeUsers[clientId].userId])
            {
                for (int i = 0; i < userInvitationsIds[activeUsers[clientId].userId].Count; i++)
                {
                    invitations[userInvitationsIds[activeUsers[clientId].userId][i]].status = 0;
                    lock (activeUsers[clientId].dbConnection)
                        lock (activeUsers[clientId].dbConnection) activeUsers[clientId].dbConnection.UpdateInvitations(i, 0);
                }
            }
            return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR);
        }

        public string CreateUser(string msg, int clientId)
        {
            // Get message as object
            Login login = MessageProccesing.DeserializeObject(msg) as Login;

            // Check if user doesnt already exists in DB
            DbMethods dbConnection = new DbMethods();
            lock (activeUsers[clientId]) { dbConnection = activeUsers[clientId].dbConnection; }
            lock (activeUsers[clientId].dbConnection)
            {
                if (dbConnection.CheckIfNameExist(login.username))
                    return MessageProccesing.CreateMessage(ErrorCodes.USER_ALREADY_EXISTS);
            }

            // Hash password
            login.passwordHash = Security.HashPassword(login.passwordHash);
            lock (activeUsers[clientId].dbConnection)
            {
                if (dbConnection.AddNewUser(login.username, login.passwordHash)) return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR);
                else return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR);
            }

        }

        public string Login(string msg, int clientId)
        {
            // Get message as object
            Login login = MessageProccesing.DeserializeObject(msg) as Login;

            // Get password hash from DB
            string passwordHash;
            DbMethods dbConnection = new DbMethods();
            lock (activeUsers[clientId]) { dbConnection = activeUsers[clientId].dbConnection; }
            lock (activeUsers[clientId].dbConnection)
            {
                try { passwordHash = dbConnection.GetFromUser("password_hash", login.username); }
                catch { return MessageProccesing.CreateMessage(ErrorCodes.USER_NOT_FOUND); }
            }

            // Verify password
            if (Security.VerifyPassword(passwordHash, login.passwordHash))
            {
                lock (activeUsers)
                {
                    // Check if user isnt already logged in
                    foreach (User u in activeUsers)
                    {
                        if (u != null)
                        {
                            if (u.username == login.username && u.logged)
                            {
                                return MessageProccesing.CreateMessage(ErrorCodes.USER_ALREADY_LOGGED_IN);
                            }
                        }
                    }
                    // If user isnt already logged in, add data to activeUsers
                    activeUsers[clientId].username = login.username;
                    activeUsers[clientId].logged = true;
                    lock (activeUsers[clientId].dbConnection)
                    {
                        activeUsers[clientId].userId = dbConnection.GetUserId(login.username);
                    }
                }
                // Start async thread
                eventHandlers[activeUsers[clientId].username] = new EventWaitHandle(false, EventResetMode.ManualReset);
                userLoginHandler[clientId].Set();
                if (!whichFunction.ContainsKey(activeUsers[clientId].username)) whichFunction[activeUsers[clientId].username] = new List<Tuple<Options, string>>();

                List<string> friends = new List<string>();
                lock (activeUsers[clientId].dbConnection)
                {
                    friends = activeUsers[clientId].dbConnection.GetFriendsNames(activeUsers[clientId].username);
                }
                foreach (var key in friends)
                {
                    // Check if friend is active
                    if (activeUsers.Contains(new User { username = key }))
                    {
                        // Send to active friend information about activity of user
                        lock (whichFunction[key])
                        {
                            whichFunction[key].Add(new Tuple<Options, string>(Options.ACTIVE_FRIENDS, activeUsers[clientId].username));
                            eventHandlers[key].Set();
                        }
                    }
                }
                // Send invitations
                lock (userInvitationsIds)
                {
                    // If there isnt any invitation create new List
                    if (!userInvitationsIds.ContainsKey(activeUsers[clientId].userId))
                    {
                        userInvitationsIds[activeUsers[clientId].userId] = new List<int>();
                    }
                    // Else send pending invitations
                    else
                    {
                        lock (whichFunction[activeUsers[clientId].username]) whichFunction[activeUsers[clientId].username].Add(new Tuple<Options, string>(Options.FRIEND_INVITATIONS, activeUsers[clientId].username));
                        eventHandlers[activeUsers[clientId].username].Set();
                    }
                }
                return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR);
            }
            else return MessageProccesing.CreateMessage(ErrorCodes.INCORRECT_PASSWORD);
        }

        public string CheckUserName(string msg, int clientId)
        {
            // Get message as object
            Username username = MessageProccesing.DeserializeObject(msg) as Username;

            // Check if user doesnt already exists in DB
            DbMethods dbConnection = new DbMethods();
            lock (activeUsers[clientId]) { dbConnection = activeUsers[clientId].dbConnection; }
            lock (activeUsers[clientId].dbConnection)
            {
                if (!dbConnection.CheckIfNameExist(username))
                    return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR);
                return MessageProccesing.CreateMessage(ErrorCodes.USER_ALREADY_EXISTS);
            }
        }

        public string AddFriend(string msg, int clientId)
        {
            // Get message as object
            Username inviteeUsername = MessageProccesing.DeserializeObject(msg) as Username;
            string invitorUsername = activeUsers[clientId].username;

            // Check if user is logged 
            lock (activeUsers[clientId])
            {
                if (!activeUsers[clientId].logged) return MessageProccesing.CreateMessage(ErrorCodes.NOT_LOGGED_IN);
            }

            if (inviteeUsername == invitorUsername) return MessageProccesing.CreateMessage(ErrorCodes.SELF_INVITE_ERROR);
            Invitation ei = new Invitation();

            // Check if given users arent already friends
            lock (activeUsers[clientId])
            {
                lock (activeUsers[clientId].dbConnection)
                {
                    if (activeUsers[clientId].dbConnection.CheckFriends(inviteeUsername, invitorUsername)) return MessageProccesing.CreateMessage(
                      ErrorCodes.ALREADY_FRIENDS);
                }
                ei.username = invitorUsername;
            }

            // Check if invitation exists
            lock (invitations)
            {
                foreach (var i in invitations.Values)
                {
                    if ((i.username == inviteeUsername && i.inviteeUsername == invitorUsername) || ((i.inviteeUsername == inviteeUsername && i.username == invitorUsername)))
                        return MessageProccesing.CreateMessage(ErrorCodes.INVITATION_ALREADY_EXIST);
                }
            }

            ei.inviteeUsername = inviteeUsername;
            lock (activeUsers[clientId].dbConnection)
            {
                ei.invitationId = activeUsers[clientId].dbConnection.CreateNewInvitation(ei.username, ei.inviteeUsername);
            }

            lock (invitations)
            {
                invitations[ei.invitationId] = ei;
            }

            // Add invite to invitor list of invitations ids
            try
            {
                userInvitationsIds[activeUsers[clientId].userId].Add(ei.invitationId);
            }
            catch
            {
                userInvitationsIds[activeUsers[clientId].userId] = new List<int> { ei.invitationId };
            }
            // Add invite to invitee list of invitations ids
            try
            {
                lock (activeUsers[clientId].dbConnection)
                    userInvitationsIds[activeUsers[clientId].dbConnection.GetUserId(inviteeUsername)].Add(ei.invitationId);
            }
            catch
            {
                lock (activeUsers[clientId].dbConnection)
                    userInvitationsIds[activeUsers[clientId].dbConnection.GetUserId(inviteeUsername)] = new List<int> { ei.invitationId };
            }

            try
            {
                // Tells aync Thred that there is a new invitation
                whichFunction[inviteeUsername].Add(new Tuple<Options, string>(Options.FRIEND_INVITATIONS, activeUsers[clientId].username));
                eventHandlers[inviteeUsername].Set();
            }
            catch
            {
                ;
            }

            return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR);
        }

        public string AcceptFriend(string msg, int clientId)
        {
            // Check if user is logged 
            lock (activeUsers[clientId])
            {
                if (!activeUsers[clientId].logged) return MessageProccesing.CreateMessage(ErrorCodes.NOT_LOGGED_IN);
            }

            InvitationId invitationId = MessageProccesing.DeserializeObject(msg) as InvitationId;

            // Check if given invitatation exists
            if (!invitations.ContainsKey(invitationId)) return MessageProccesing.CreateMessage(ErrorCodes.WRONG_INVATATION_ID);
            Invitation inv = invitations[invitationId];

            // Check if given invitation werent already accepted
            if (inv.status == 2) return MessageProccesing.CreateMessage(ErrorCodes.INVITATION_ALREADY_ACCEPTED);


            // Check if given users arent already friends
            lock (activeUsers[clientId])
            {
                lock (activeUsers[clientId].dbConnection)
                {
                    if (activeUsers[clientId].dbConnection.CheckFriends(inv.inviteeUsername, inv.username)) return MessageProccesing.CreateMessage(
                      ErrorCodes.ALREADY_FRIENDS);
                }
            }

            // Update status
            lock (invitations[invitationId]) invitations[invitationId].status = 2;
            lock (activeUsers[clientId].dbConnection)
                activeUsers[clientId].dbConnection.UpdateInvitations(invitationId, 2);

            // Create recornd in DB
            lock (activeUsers[clientId].dbConnection)
                activeUsers[clientId].dbConnection.AddFriends(activeUsers[clientId].userId, invitations[invitationId].username);

            // Delete index in invitee ivitations ids
            lock (userInvitationsIds[activeUsers[clientId].userId])
            {
                userInvitationsIds[activeUsers[clientId].userId].Remove(invitationId);
            }
            // Tell async invitor Thread about accepted invitation
            whichFunction[inv.username].Add(new Tuple<Options, string>(Options.FRIEND_INVITATIONS, activeUsers[clientId].username));
            eventHandlers[inv.username].Set();
            return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR);
        }

        public string DeclineFriend(string msg, int clientId)
        {
            // Check if user is logged 
            lock (activeUsers[clientId])
            {
                if (!activeUsers[clientId].logged) return MessageProccesing.CreateMessage(ErrorCodes.NOT_LOGGED_IN);
            }

            InvitationId invitationId = MessageProccesing.DeserializeObject(msg) as InvitationId;

            // Check if given invitatation exists
            if (!invitations.ContainsKey(invitationId)) return MessageProccesing.CreateMessage(ErrorCodes.WRONG_INVATATION_ID);
            Invitation inv = invitations[invitationId];

            // Check if given invitation werent already accepted
            if (inv.status == 2) return MessageProccesing.CreateMessage(ErrorCodes.INVITATION_ALREADY_ACCEPTED);


            // Check if given users arent already friends
            lock (activeUsers[clientId])
            {
                lock (activeUsers[clientId].dbConnection)
                {
                    if (activeUsers[clientId].dbConnection.CheckFriends(inv.inviteeUsername, inv.username)) return MessageProccesing.CreateMessage(
                      ErrorCodes.ALREADY_FRIENDS);
                }
            }

            int secondeUserId = activeUsers[clientId].dbConnection.GetUserId(invitations[invitationId].username);
            if (secondeUserId == activeUsers[clientId].userId) return MessageProccesing.CreateMessage(ErrorCodes.SELF_INVITE_ERROR);

            // Set status a declined
            invitations[invitationId].status = 3;

            // Update DB invitation
            lock (activeUsers[clientId].dbConnection)
                activeUsers[clientId].dbConnection.UpdateInvitations(invitationId, 3);

            // Tell invitor about declined invitation
            whichFunction[inv.username].Add(new Tuple<Options, string>(Options.FRIEND_INVITATIONS, activeUsers[clientId].username));
            eventHandlers[invitations[invitationId].username].Set();
            return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR);
        }

        public string GetFriends(string msg, int clientId)
        {
            // If user isint logged return error
            if (!activeUsers[clientId].logged) return MessageProccesing.CreateMessage(ErrorCodes.NOT_LOGGED_IN);
            List<Friend> friends = new List<Friend>();

            // Get friends from DB
            lock (activeUsers[clientId].dbConnection)
            {
                foreach (string friendName in activeUsers[clientId].dbConnection.GetFriendsNames(activeUsers[clientId].username))
                {
                    Friend friend = new Friend(friendName, 0);
                    if (activeUsers.Contains(new User() { username = friendName })) friend.active = 1;
                    friends.Add(friend);
                }
            }
            return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR, friends);

        }

        public string DeleteAccount(string msg, int clientId)
        {
            // If user isint logged return error
            if (!activeUsers[clientId].logged) return MessageProccesing.CreateMessage(ErrorCodes.NOT_LOGGED_IN);
            // Delete user from DB
            lock (activeUsers[clientId].dbConnection)
                activeUsers[clientId].dbConnection.DeleteUser(activeUsers[clientId].userId);

            // Delete user informations from memory
            ClearUserData(clientId);
            return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR);
        }

        public string InviteToConversation(string msg, int clientId)
        {
            // If user isint logged return error
            if (!activeUsers[clientId].logged) return MessageProccesing.CreateMessage(ErrorCodes.NOT_LOGGED_IN);
            bool conversationExists;
            lock (activeUsers[clientId].dbConnection)
                conversationExists = activeUsers[clientId].dbConnection.CheckIfConversationExist(activeUsers[clientId].username);
            Username user = MessageProccesing.DeserializeObject(msg) as Username;
            bool isActive = false;
            foreach (User active in activeUsers)
            {
                if (active.username == user.username && active.logged)
                {
                    isActive = true;
                    break;
                }
            }
            if (!isActive) return MessageProccesing.CreateMessage(ErrorCodes.USER_OFFLINE);
            int conversationId;

            // Invite to existing converstaion
            if (conversationExists)
            {
                lock (activeUsers[clientId].dbConnection)
                    conversationId = activeUsers[clientId].dbConnection.AddUserToConversation(activeUsers[clientId].username, user.username);
            }
            // Create new conversation
            else
            {
                lock (activeUsers[clientId].dbConnection)
                    conversationId = activeUsers[clientId].dbConnection.CreateNewConversation(activeUsers[clientId].username, user.username);
            }

            // try catch czy jest zalogowany
            whichFunction[user.username].Add(new Tuple<Options, string>(Options.INCOMMING_CALL, conversationId.ToString()));
            eventHandlers[user.username].Set();
            return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR);
        }

        // Tell serverconnection
        public string JoinConversation(string msg, int clientId)
        {
            int conversationId = MessageProccesing.DeserializeObject(msg) as InvitationId;
            // If user isint logged return error
            if (!activeUsers[clientId].logged) return MessageProccesing.CreateMessage(ErrorCodes.NOT_LOGGED_IN);

            // Notyfiy other participans about joining conversation
            List<string> conversationsParticipansList = new List<string>();
            lock (activeUsers[clientId].dbConnection)
                conversationsParticipansList = activeUsers[clientId].dbConnection.GetConversationsParticipants(conversationId, activeUsers[clientId].username);
            foreach (string participant in conversationsParticipansList)
            {
                whichFunction[participant].Add(new Tuple<Options, string>(Options.ACCEPTED_CALL, activeUsers[clientId].username));
                eventHandlers[participant].Set();
            }
            lock (activeUsers[clientId].dbConnection)
                activeUsers[clientId].dbConnection.UpdateUserConversationStatus(activeUsers[clientId].username, CallStatus.ACCEPTED);
            // Infro to server connection. Nedded to create udp sockets
            throw new CustomException(MessageProccesing.CreateMessage(Options.JOIN_CONVERSATION, conversationId));
        }

        public string LeaveConversation(string msg, int clientId)
        {
            InvitationId conversationId = MessageProccesing.DeserializeObject(msg) as InvitationId;
            // If user isint logged return error
            if (!activeUsers[clientId].logged) return MessageProccesing.CreateMessage(ErrorCodes.NOT_LOGGED_IN);

            lock (activeUsers[clientId].dbConnection)
                activeUsers[clientId].dbConnection.DeleteFromConversation(activeUsers[clientId].username);
            // Notyfiy other participans about leacing conversation
            List<string> conversationsParticipansList = new List<string>();
            lock (activeUsers[clientId].dbConnection)
                conversationsParticipansList = activeUsers[clientId].dbConnection.GetConversationsParticipants(conversationId, activeUsers[clientId].username);
            foreach (string participant in conversationsParticipansList)
            {
                whichFunction[participant].Add(new Tuple<Options, string>(Options.DECLINED_CALL, activeUsers[clientId].username));
                eventHandlers[participant].Set();
            }
            throw new CustomException(MessageProccesing.CreateMessage(Options.LEAVE_CONVERSATION, conversationId));
        }



        /// <summary>
        /// Send new invitations, accepted and deleted ones
        /// </summary>
        public string SendInvitations(int clientId, string senderName)
        {
            List<Invitation> result = new List<Invitation>();
            var InvitationsIds = userInvitationsIds[activeUsers[clientId].userId];
            // Check invitations status
            for (int i = 0; i < InvitationsIds.Count; i++)
            {
                // If invitation wasnt sended
                if (invitations[InvitationsIds[i]].status == 0 && invitations[InvitationsIds[i]].inviteeUsername == activeUsers[clientId].username)
                {
                    invitations[InvitationsIds[i]].status = 1;
                    result.Add(invitations[InvitationsIds[i]]);
                    lock (activeUsers[clientId].dbConnection)
                        activeUsers[clientId].dbConnection.UpdateInvitations(InvitationsIds[i], 1);
                }
                // If invitation was accepted or declined
                else if (invitations[InvitationsIds[i]].status >= 2 && invitations[InvitationsIds[i]].username == activeUsers[clientId].username)
                {
                    result.Add(invitations[InvitationsIds[i]]);
                    lock (activeUsers[clientId].dbConnection)
                        activeUsers[clientId].dbConnection.DeleteInvitation(InvitationsIds[i]);

                    // Delete invitation
                    lock (invitations[InvitationsIds[i]])
                    {
                        invitations.Remove(InvitationsIds[i]);
                    }

                    // Delete invitation id
                    lock (userInvitationsIds[activeUsers[clientId].userId])
                    {
                        userInvitationsIds[activeUsers[clientId].userId].Remove(InvitationsIds[i]);
                    }
                }
            }
            return MessageProccesing.CreateMessage(Options.FRIEND_INVITATIONS, result);
        }


        public string SendActiveFriends(int clientId, string senderName)
        {
            return MessageProccesing.CreateMessage(Options.ACTIVE_FRIENDS, new Username(senderName));
        }


        public string SendIncommingCall(int clientId, string callId)
        {
            Call call = new Call();
            call.callId = int.Parse(callId);
            lock (activeUsers[clientId].dbConnection)
                call.usernames = activeUsers[clientId].dbConnection.GetConversationsParticipants(call.callId, activeUsers[clientId].username);
            return MessageProccesing.CreateMessage(Options.INCOMMING_CALL, call);
        }

        public string SendDeclinedCall(int clientId, string senderName)
        {
            return MessageProccesing.CreateMessage(Options.DECLINED_CALL, new Username(senderName));
        }

        public string SendAcceptedCall(int clientId, string senderName)
        {
            return MessageProccesing.CreateMessage(Options.ACCEPTED_CALL, new Username(senderName));
        }




        public ClientProcessing()
        {
            syncFunctions = new Dictionary<Options, Functions>();
            asyncFunctions = new Dictionary<Options, AsyncFunctions>();
            syncFunctions.Add(Options.LOGOUT, new Functions(Logout));
            syncFunctions.Add(Options.LOGIN, new Functions(Login));
            syncFunctions.Add(Options.CREATE_USER, new Functions(CreateUser));
            syncFunctions.Add(Options.CHECK_USER_NAME, new Functions(CheckUserName));
            syncFunctions.Add(Options.GET_FRIENDS, new Functions(GetFriends));
            syncFunctions.Add(Options.DELETE_ACCOUNT, new Functions(DeleteAccount));
            syncFunctions.Add(Options.ADD_FRIEND, new Functions(AddFriend));
            syncFunctions.Add(Options.ACCEPT_FRIEND, new Functions(AcceptFriend));
            syncFunctions.Add(Options.DECLINE_FRIEND, new Functions(DeclineFriend));
            syncFunctions.Add(Options.INVITE_TO_CONVERSATION, new Functions(InviteToConversation));
            syncFunctions.Add(Options.JOIN_CONVERSATION, new Functions(JoinConversation));
            syncFunctions.Add(Options.LEAVE_CONVERSATION, new Functions(LeaveConversation));

            asyncFunctions.Add(Options.ACTIVE_FRIENDS, new AsyncFunctions(SendActiveFriends));
            asyncFunctions.Add(Options.FRIEND_INVITATIONS, new AsyncFunctions(SendInvitations));
            asyncFunctions.Add(Options.INCOMMING_CALL, new AsyncFunctions(SendIncommingCall));
            asyncFunctions.Add(Options.ACCEPTED_CALL, new AsyncFunctions(SendAcceptedCall));
            asyncFunctions.Add(Options.DECLINED_CALL, new AsyncFunctions(SendDeclinedCall));

            dbMethods = new DbMethods();
            activeUsers = new List<User>();
            eventHandlers = new Dictionary<string, EventWaitHandle>();
            userLoginHandler = new Dictionary<int, EventWaitHandle>();
            invitations = dbMethods.GetInvitations();
            userInvitationsIds = dbMethods.GetUsersInvitationsIds();
            whichFunction = new Dictionary<string, List<Tuple<Options, string>>>();
        }



        public int AddActiveUser()
        {
            for (int i = 0; i < activeUsers.Count; i++)
            {
                if (activeUsers[i] == null)
                {
                    activeUsers[i] = new User();
                    userLoginHandler.Add(i, new EventWaitHandle(false, EventResetMode.ManualReset));
                    return i;
                }
            }
            activeUsers.Add(new User());
            userLoginHandler.Add(activeUsers.Count - 1, new EventWaitHandle(false, EventResetMode.ManualReset));
            return activeUsers.Count - 1;
        }

        public void Disconnect(int clientId)
        {
            userLoginHandler[clientId].Set();
            try
            {
                eventHandlers[activeUsers[clientId].username].Set();
            }
            catch
            {
                ;
            }
            ClearUserData(clientId);
        }

        private void ClearUserData(int clientId)
        {
            lock (whichFunction) whichFunction.Remove(activeUsers[clientId].username);
            lock (eventHandlers) eventHandlers.Remove(activeUsers[clientId].username);
            lock (userLoginHandler) userLoginHandler.Remove(clientId);

            lock (activeUsers)
            {
                lock (activeUsers[clientId].dbConnection)
                    activeUsers[clientId].dbConnection.CloseConnection();
                activeUsers[clientId] = null;
            }
        }

    }
}
