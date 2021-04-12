using DbLibrary;
using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;

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
        public delegate string AsyncFunctions(int clientId);
        /// <summary>
        /// List of all avaliable syncFunctions, index of given function is number of that option
        /// </summary>
        public Dictionary<Options, Functions> syncFunctions;
        public Dictionary<Options, AsyncFunctions> asyncFunctions;

        public List<User> activeUsers;
        public Dictionary<int, Invitation> invitations;
        public DbMethods dbMethods;

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
            foreach(var function in asyncFunctions.Keys)
            {
                item = asyncFunctions[function](clientId);
                if (item != "") res.Add(item);
            }
            return res;
        }


        public string Logout(string msg, int clientId)
        {
            throw new NotImplementedException();
        }

        public string CreateUser(string msg, int clientId)
        {
            Login login = MessageProccesing.DeserializeObject(msg) as Login;

            DbMethods dbConnection = new DbMethods();
            lock (activeUsers[clientId]) { dbConnection = activeUsers[clientId].dbConnection; }
            if (dbConnection.CheckIfNameExist(login.username))
                return MessageProccesing.CreateMessage(ErrorCodes.USER_ALREADY_EXISTS, Options.CREATE_USER);

            login.passwordHash = Security.HashPassword(login.passwordHash);
            if (dbConnection.AddNewUser(login.username, login.passwordHash)) return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR, Options.CREATE_USER);
            else return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR, Options.CREATE_USER);
        }

        public string Login(string msg, int clientId)
        {
            Login login = MessageProccesing.DeserializeObject(msg) as Login;
            string passwordHash = "";
            DbMethods dbConnection = new DbMethods();
            lock (activeUsers[clientId]) { dbConnection = activeUsers[clientId].dbConnection; }
            try { passwordHash = dbConnection.GetFromUser("password_hash", login.username); }
            catch { return MessageProccesing.CreateMessage(ErrorCodes.USER_NOT_FOUND, Options.LOGIN); }

            if (Security.VerifyPassword(passwordHash, login.passwordHash))
            {
                lock (activeUsers)
                {
                    foreach (User u in activeUsers)
                    {
                        if (u != null)
                        {
                            if (u.userName == login.username && u.logged)
                            {
                                return MessageProccesing.CreateMessage(ErrorCodes.USER_ALREADY_LOGGED_IN, Options.LOGIN);
                            }
                        }
                    }
                    activeUsers[clientId].logged = true;
                    activeUsers[clientId].userName = login.username;
                    activeUsers[clientId].userId = dbConnection.GetUserId(login.username);
                }
                return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR, Options.LOGIN);
            }
            else return MessageProccesing.CreateMessage(ErrorCodes.INCORRECT_PASSWORD, Options.LOGIN);
        }

        public string CheckUserName(string msg, int clientId)
        {
            Username username = MessageProccesing.DeserializeObject(msg) as Username;
            DbMethods dbConnection = new DbMethods();
            lock (activeUsers[clientId]) { dbConnection = activeUsers[clientId].dbConnection; }
            if (!dbConnection.CheckIfNameExist(username.username))
                return MessageProccesing.CreateMessage(ErrorCodes.NO_ERROR, Options.CHECK_USER_NAME);
            return MessageProccesing.CreateMessage(ErrorCodes.USER_ALREADY_EXISTS, Options.CHECK_USER_NAME);
        }


        public string SendCall(int clientId)
        {
            throw new NotImplementedException();
        }

        public string SendInvitations(int clientId)
        {
            throw new NotImplementedException();
        }

        public string SendActiveFriends(int clientId)
        {
            throw new NotImplementedException();
        }

        public string SendAcceptedFriends(int clientId)
        {
            throw new NotImplementedException();
        }

        public string DeclineFriend(string msg, int clientId)
        {
            throw new NotImplementedException();
        }

        public string AcceptFriend(string msg, int clientId)
        {
            throw new NotImplementedException();
        }

        public string AddFriend(string msg, int clientId)
        {
            throw new NotImplementedException();
        }

        public string DeleteAccount(string msg, int clientId)
        {
            throw new NotImplementedException();
        }

        public string GetFriends(string msg, int clientId)
        {
            throw new NotImplementedException();
        }

        public string Disconnect(string msg, int clientId)
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
            syncFunctions.Add(Options.DISCONNECT, new Functions(Disconnect));
            syncFunctions.Add(Options.GET_FRIENDS, new Functions(GetFriends));
            syncFunctions.Add(Options.DELETE_ACCOUNT, new Functions(DeleteAccount));
            syncFunctions.Add(Options.ADD_FRIEND, new Functions(AddFriend));
            syncFunctions.Add(Options.ACCEPT_FRIEND, new Functions(AcceptFriend));
            syncFunctions.Add(Options.DECLINE_FRIEND, new Functions(DeclineFriend));


            asyncFunctions.Add(Options.ACCEPT_FRIEND, new AsyncFunctions(SendAcceptedFriends));
            asyncFunctions.Add(Options.ACTIVE_FRIENDS, new AsyncFunctions(SendActiveFriends));
            asyncFunctions.Add(Options.FRIEND_INVITATIONS, new AsyncFunctions(SendInvitations));
            asyncFunctions.Add(Options.INCOMMING_CALL, new AsyncFunctions(SendCall));

            dbMethods = new DbMethods();
            activeUsers = new List<User>();
            //invitations = dbMethods.GetInvitations();
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
            return activeUsers.Count - 1;
        }
    }
}
