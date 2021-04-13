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
        public delegate string AsyncFunctions(int clientId,string senderName);
        /// <summary>
        /// List of all avaliable syncFunctions, index of given function is number of that option
        /// </summary>
        public Dictionary<Options, Functions> syncFunctions;
        public Dictionary<Options, AsyncFunctions> asyncFunctions;
        public DbMethods dbMethods;
        public List<User> activeUsers;

        /// <summary>
        /// <para> Key: <c>username</c> </para> 
        /// <para> Value: <c>List(option,senderId)</c> </para>
        /// </summary>
        private Dictionary<string, List<Tuple<Options,string>>> whichFunction;

        /// <summary>
        /// <para> Key: <c>InvitationId</c> </para> 
        /// <para> Value: <c>Invitation</c> </para>
        /// </summary>
        public Dictionary<int, Invitation> invitations;

        /// <summary>
        /// <para> Key: <c>clientId</c>  </para>
        /// <para>Value: <c>List(invitationId)</c>  </para>
        /// </summary>
        public Dictionary<int, List<int>> userInvitationsIds;

        /// <summary>
        /// <para> Key: <c>username</c> </para> 
        /// <para> Value: <c>handler</c> </para>
        /// </summary>
        private Dictionary<string,EventWaitHandle> eventHandlers;

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
            if (!activeUsers[clientId].logged) userLoginHandler[clientId].WaitOne();
            eventHandlers[activeUsers[clientId].username].WaitOne();
            lock (whichFunction[activeUsers[clientId].username])
            {
                foreach (var function in whichFunction[activeUsers[clientId].username])
                {
                    item = asyncFunctions[function.Item1](clientId, function.Item2);
                    whichFunction[activeUsers[clientId].username].Remove(function);
                    if (item != "") res.Add(item);
                }
            }
            return res;
        }


        public string Logout(string msg, int clientId)
        {
            throw new NotImplementedException();
        }

        public string CreateUser(string msg, int clientId)
        {
            // Get message as object
            Login login = MessageProccesing.DeserializeObject(msg) as Login;

            // Check if user doesnt already exists in DB
            DbMethods dbConnection = new DbMethods();
            lock (activeUsers[clientId]) { dbConnection = activeUsers[clientId].dbConnection; }
            if (dbConnection.CheckIfNameExist(login.username))
                return MessageProccesing.CreateMessage(ErrorCodes.USER_ALREADY_EXISTS);

            // Hash password
            login.passwordHash = Security.HashPassword(login.passwordHash);
            if (dbConnection.AddNewUser(login.username, login.passwordHash)) return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR);
            else return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR);
        }

        public string Login(string msg, int clientId)
        {
            // Get message as object
            Login login = MessageProccesing.DeserializeObject(msg) as Login;

            // Get password hash from DB
            string passwordHash;
            DbMethods dbConnection = new DbMethods();
            lock (activeUsers[clientId]) { dbConnection = activeUsers[clientId].dbConnection; }
            try { passwordHash = dbConnection.GetFromUser("password_hash", login.username); }
            catch { return MessageProccesing.CreateMessage(ErrorCodes.USER_NOT_FOUND); }

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
                    activeUsers[clientId].userId = dbConnection.GetUserId(login.username);
                }
                // Start async thread
                eventHandlers[activeUsers[clientId].username] = new EventWaitHandle(false, EventResetMode.ManualReset);
                userLoginHandler[clientId].Set();
                if (!whichFunction.ContainsKey(activeUsers[clientId].username)) whichFunction[activeUsers[clientId].username] = new List<Tuple<Options, string>>();
                List<string> friends = activeUsers[clientId].dbConnection.GetFriendsNames(activeUsers[clientId].username);
                foreach (var key in friends)
                {
                    // Check if friend is active
                    if (activeUsers.Contains(new User { username = key}))
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
                lock (whichFunction[activeUsers[clientId].username]) whichFunction[activeUsers[clientId].username].Add(new Tuple<Options, string>(Options.FRIEND_INVITATIONS, activeUsers[clientId].username));
                eventHandlers[activeUsers[clientId].username].Set();
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
            if (!dbConnection.CheckIfNameExist(username))
                return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR);
            return MessageProccesing.CreateMessage(ErrorCodes.USER_ALREADY_EXISTS);
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
                if (activeUsers[clientId].dbConnection.CheckFriends(inviteeUsername, invitorUsername)) return MessageProccesing.CreateMessage(
                      ErrorCodes.ALREADY_FRIENDS);
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
            ei.invitationId = activeUsers[clientId].dbConnection.CreateNewInvitation(ei.username, ei.inviteeUsername);

            lock (invitations)
            {
                invitations[ei.invitationId] = ei;
            }

            try
            {
                userInvitationsIds[activeUsers[clientId].userId].Add(ei.invitationId);
            }
            catch
            {
                userInvitationsIds[activeUsers[clientId].userId] = new List<int> { ei.invitationId };
            }
            try
            {
                userInvitationsIds[activeUsers[clientId].dbConnection.GetUserId(inviteeUsername)].Add(ei.invitationId);
            }
            catch
            {
                userInvitationsIds[activeUsers[clientId].dbConnection.GetUserId(inviteeUsername)] = new List<int> { ei.invitationId };
            }

            // Tells aync Thred that there is a new invitation
            whichFunction[inviteeUsername].Add(new Tuple<Options, string>(Options.FRIEND_INVITATIONS, activeUsers[clientId].username));
            eventHandlers[inviteeUsername].Set();

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
                if (activeUsers[clientId].dbConnection.CheckFriends(inv.inviteeUsername, inv.username)) return MessageProccesing.CreateMessage(
                      ErrorCodes.ALREADY_FRIENDS);
            }

            // Update status
            lock (invitations[invitationId]) invitations[invitationId].status = 2;
            activeUsers[clientId].dbConnection.UpdateInvitations(invitationId, 2);

            activeUsers[clientId].dbConnection.AddFriends(activeUsers[clientId].userId, invitations[invitationId].username);

            // Delete index in ivitations
            lock(userInvitationsIds[activeUsers[clientId].userId])
            {
                userInvitationsIds[activeUsers[clientId].userId].Remove(invitationId);
            }
            // Tell async invitee Thread about accepted invitation
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
                if (activeUsers[clientId].dbConnection.CheckFriends(inv.inviteeUsername, inv.username)) return MessageProccesing.CreateMessage(
                      ErrorCodes.ALREADY_FRIENDS);
            }

            int secondeUserId = activeUsers[clientId].dbConnection.GetUserId(invitations[invitationId].username);
            if (secondeUserId == activeUsers[clientId].userId) return MessageProccesing.CreateMessage(ErrorCodes.SELF_INVITE_ERROR);

            // Set status a declined
            invitations[invitationId].status = 3;
            // Tell invitor about declined invitation
            activeUsers[clientId].dbConnection.UpdateInvitations(invitationId, 3);
            whichFunction[inv.username].Add(new Tuple<Options, string>(Options.FRIEND_INVITATIONS, activeUsers[clientId].username));
            eventHandlers[invitations[invitationId].username].Set();
            return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR);
        }

        public string GetFriends(string msg, int clientId)
        {
            if (!activeUsers[clientId].logged) return MessageProccesing.CreateMessage(ErrorCodes.NOT_LOGGED_IN);
            List<Friend> friends = new List<Friend>();
            foreach (string friendName in activeUsers[clientId].dbConnection.GetFriendsNames(activeUsers[clientId].username))
            {
                Friend friend = new Friend(friendName, 0);
                if (activeUsers.Contains(new User() { username = friendName })) friend.active = 1;
                friends.Add(friend);
            }
            return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR, friends);

        }

        public string DeleteAccount(string msg, int clientId)
        {
            activeUsers[clientId].dbConnection.DeleteUser(activeUsers[clientId].userId);
            ClearUserData(clientId);
            return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR);
        }


        /// <summary>
        /// Send new invitations, accepted and deleted ones
        /// </summary>
        public string SendInvitations(int clientId, string senderName)
        {
            List<Invitation> result = new List<Invitation>();
            var InvitationsIds = userInvitationsIds[activeUsers[clientId].userId];
            // Check invitations status
            for (int i=0;i<InvitationsIds.Count;i++)
            {
                // If invitation wasnt sended
                if(invitations[InvitationsIds[i]].status == 0 && invitations[InvitationsIds[i]].inviteeUsername == activeUsers[clientId].username)
                {
                    invitations[InvitationsIds[i]].status = 1;
                    result.Add(invitations[InvitationsIds[i]]);
                    activeUsers[clientId].dbConnection.UpdateInvitations(InvitationsIds[i],1);
                }
                // If invitation was accepted
                else if(invitations[InvitationsIds[i]].status >= 2 && invitations[InvitationsIds[i]].username == activeUsers[clientId].username)
                {
                    result.Add(invitations[InvitationsIds[i]]);
                    activeUsers[clientId].dbConnection.DeleteInvitation(InvitationsIds[i]);
                    lock (invitations[InvitationsIds[i]])
                    {
                        invitations.Remove(InvitationsIds[i]);
                    }
                    lock(userInvitationsIds[activeUsers[clientId].userId])
                    {
                        userInvitationsIds[activeUsers[clientId].userId].Remove(InvitationsIds[i]);
                    }
                }
            }
            return MessageProccesing.CreateMessage(Options.FRIEND_INVITATIONS, result);
        }


        public string SendActiveFriends(int clientId,string senderName)
        {
            return MessageProccesing.CreateMessage(Options.ACTIVE_FRIENDS, new Username(senderName));
        }


        public string SendCall(int clientId, string senderName)
        {
            throw new NotImplementedException();
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


            asyncFunctions.Add(Options.ACTIVE_FRIENDS, new AsyncFunctions(SendActiveFriends));
            asyncFunctions.Add(Options.FRIEND_INVITATIONS, new AsyncFunctions(SendInvitations));
            asyncFunctions.Add(Options.INCOMMING_CALL, new AsyncFunctions(SendCall));

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
            eventHandlers[activeUsers[clientId].username].Set();
            ClearUserData(clientId);
        }

        private void ClearUserData(int clientId)
        {
            lock (whichFunction) whichFunction.Remove(activeUsers[clientId].username);                   
            lock (eventHandlers)   eventHandlers.Remove(activeUsers[clientId].username);
            lock (userLoginHandler)  userLoginHandler.Remove(clientId);

            lock (activeUsers)
            {
                activeUsers[clientId].dbConnection.CloseConnection();
                activeUsers[clientId] = null;
            }
        }

    }
}
