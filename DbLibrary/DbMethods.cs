using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;



// MySql date "yyyy-MM-dd HH:mm:ss"
namespace DbLibrary
{
    public class DbMethods : DbConnection
    {

        public int GetActiveConversationId(string username)
        {
            string query = string.Format("SELECT conversation_id FROM conversations WHERE user_id = {0} and status= 2", GetUserId(username));
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            try
            {
                dataReader.Read();
                return dataReader.GetInt32(0);
            }
            catch
            {
                return 0;
            }
            finally
            {
                dataReader.Close();
            }
        }

        public int GetUserActivity(string username)
        {
            string query = string.Format("SELECT is_active FROM users WHERE user_id = {0}", GetUserId(username));
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            try
            {
                dataReader.Read();
                return dataReader.GetInt32(0);
            }
            catch
            {
                return 0;
            }
            finally
            {
                dataReader.Close();
            }
        }

        public void SetUserActivity(string username,bool status)
        {
            string query = string.Format("UPDATE users SET is_active = {1} WHERE user_id = {0}", GetUserId(username),status);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Read();
            dataReader.Close();
            
        }

        public int GetSecondUserId(int conversationId, int userId)
        {
            string query = string.Format("SELECT user1_id,user2_id FROM conversations WHERE  conversation_id = {0}", conversationId);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            // Check if conversation already exist
            try
            {
                dataReader.Read();
                int secondUserId = dataReader.GetInt32("user1_id");
                if (secondUserId != userId) return secondUserId;
                return dataReader.GetInt32("user2_id");
            }
            catch
            {
                return 0;
                ;
            }
            finally
            {
                dataReader.Close();
            }

        }
        public void AddFriends(int invitorId, string inviteeUsername)
        {
            string inviteeId = GetUserId(inviteeUsername).ToString();
            string query = string.Format("INSERT INTO friends(user1_id,user2_id,date) VALUES({0},{1},'{2}')", invitorId, inviteeId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Read();
            dataReader.Close();

        }


        public int GetConversationId(string username)
        {
            string Id = GetUserId(username).ToString();
            string query = string.Format("SELECT conversation_id FROM conversations c WHERE c.user_id = '{0}'", Id);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            dataReader.Read();
            try
            {
                return dataReader.GetInt32(0);
            }
            catch
            {
                return -1;
            }
            finally
            {
                dataReader.Close();
            }
        }
        public bool CheckIfConversationExist(string username)
        {
            string query = string.Format("SELECT cv.status FROM conversations_view cv WHERE cv.username = '{0}'", username);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            dataReader.Read();
            try
            {
                dataReader.GetInt32(0);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                dataReader.Close();
            }
        }

        public void UpdateUserConversationStatus(string username, CallStatus status)
        {
            string Id = GetUserId(username).ToString();
            string query = String.Format("UPDATE conversations SET status={0} WHERE user_id = {1}", (int)status, Id);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Close();
        }

        public int CreateNewConversation(string usernameA, string usernameB)
        {
            string IdA = GetUserId(usernameA).ToString();
            string IdB = GetUserId(usernameB).ToString();
            string query = String.Format("INSERT INTO conversations(user_id,status) VALUES({0},{1})", IdA,2);
            //Create Command
            MySqlCommand cmd = new MySqlCommand(query, connection);
            //Create a data reader and Execute the command
            MySqlDataReader dataReader = cmd.ExecuteReader();

            dataReader.Read();
            dataReader.Close();

            query = "SELECT MAX(c.conversation_id) FROM conversations c";
            cmd = new MySqlCommand(query, connection);
            dataReader = cmd.ExecuteReader();
            dataReader.Read();
            int conversationId = dataReader.GetInt32(0);
            dataReader.Close();

            query = String.Format("INSERT INTO conversations(conversation_id,user_id) VALUES({0},{1})", conversationId,IdB);
            cmd = new MySqlCommand(query, connection);
            dataReader = cmd.ExecuteReader();
            dataReader.Read();
            dataReader.Close();

            return conversationId;
        }

        public int AddUserToConversation(string invitor,string invitee)
        {
            string inviteeId = GetUserId(invitee).ToString();           
            string query = String.Format("INSERT INTO conversations(conversation_id,user_id) VALUES({0},{1})", GetConversationId(invitor), inviteeId);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Read();
            dataReader.Close();
            return GetConversationId(invitor);
        }

        public void DeleteFromConversation(string username,int conversationId)
        {
            string Id = GetUserId(username).ToString();
            string query = String.Format("DELETE FROM conversations WHERE user_id = {0} and conversation_id = {1}", Id, conversationId);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Close();
        }

        public List<string> GetConversationsParticipants(int conversationId,string username)
        {
            List<string> result = new List<string>();

            // Status 2 means accepted call
            string query = string.Format("SELECT cv.username FROM conversations_view cv WHERE cv.conversation_id = {0} AND cv.username != '{1}'", conversationId, username);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                result.Add(dataReader.GetString(0));
            }
            dataReader.Close();
            return result;
        }

        public List<string> GetFriendsNames(string username)
        {
            List<string> result = new List<string>();
            string query = string.Format("SELECT f.user2 FROM friends_view f WHERE f.user1 = '{0}'", username);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            while(dataReader.Read())
            {
                result.Add(dataReader.GetString(0));
            }
            dataReader.Close();


            query = string.Format("SELECT f.user1 FROM friends_view f WHERE f.user2 = '{0}'", username);
            cmd = new MySqlCommand(query, connection);
            dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                result.Add(dataReader.GetString(0));
            }
            dataReader.Close();
            return result;
        }

        public int CreateNewInvitation(string invitorUsername, string inviteeUsername)
        {
            int invitorId = GetUserId(invitorUsername);
            int inviteeId = GetUserId(inviteeUsername);
            string query = String.Format("INSERT INTO invitations(invitor_id,invitee_id,status,date) VALUES('{0}','{1}','{2}','{3}')", invitorId, inviteeId,0,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //Create Command
            MySqlCommand cmd = new MySqlCommand(query, connection);
            //Create a data reader and Execute the command
            MySqlDataReader dataReader = cmd.ExecuteReader();

            dataReader.Read();
            dataReader.Close();

            query = "SELECT LAST_INSERT_ID()";
            cmd = new MySqlCommand(query, connection);
            dataReader = cmd.ExecuteReader();
            dataReader.Read();
            try
            {
                return dataReader.GetInt32(0);
            }
            catch
            {
                return -1;
            }
            finally
            {
                dataReader.Close();
            }
        }

        public void UpdateInvitations(int invitationId,int status)
        {
            string query = String.Format("UPDATE invitations SET status={0} WHERE invitation_id = {1}", status, invitationId);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Close();
        }

        public Dictionary<int, Invitation> GetInvitations()
        {
            string query = String.Format("SELECT * FROM invitations_view");
            //Create Command
            MySqlCommand cmd = new MySqlCommand(query, connection);
            //Create a data reader and Execute the command
            MySqlDataReader dataReader = cmd.ExecuteReader();
            Dictionary<int, Invitation> result = new Dictionary<int, Invitation>();
            while (dataReader.Read())
            {
                Invitation ei = new Invitation();
                ei.invitationId = dataReader.GetInt32("invitation_id");
                ei.username = dataReader.GetString("invitor");
                ei.inviteeUsername = dataReader.GetString("invitee");
                ei.date = dataReader.GetDateTime("date");
                ei.status = 0;
                result[ei.invitationId] = ei;
            }
            dataReader.Close();
            return result;
        }

        public bool CheckFriends(string usernameA, string usernameB)
        {
            string userAId = GetUserId(usernameA).ToString();
            string userBId = GetUserId(usernameB).ToString();

            string query = string.Format("SELECT friend_id FROM friends WHERE (user1_id = {0} AND user2_id = {1}) OR (user1_id = {1}  AND user2_id = {0})", userAId, userBId);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Read();
            try
            {
                string Id = dataReader.GetString(0);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                dataReader.Close();
            }

        }

        public Dictionary<int, List<int>> GetUsersInvitationsIds()
        {
            Dictionary<int, List<int>> res = new Dictionary<int, List<int>>();
            string query = String.Format("SELECT invitation_id,invitor_id,invitee_id,status FROM invitations");
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                if (!res.ContainsKey(dataReader.GetInt32("invitor_id"))) res[dataReader.GetInt32("invitor_id")] = new List<int>();
                res[dataReader.GetInt32("invitor_id")].Add(dataReader.GetInt32("invitation_id"));
                if (dataReader.GetInt16("status") == 0)
                {
                    if (!res.ContainsKey(dataReader.GetInt32("invitee_id"))) res[dataReader.GetInt32("invitee_id")] = new List<int>();
                    res[dataReader.GetInt32("invitee_id")].Add(dataReader.GetInt32("invitation_id"));
                }

            }
            dataReader.Close();
            return res;

        }
        public bool AddNewUser(string username, string passwordHash)
        {
            string query = String.Format("INSERT INTO users(username,password_hash) VALUES('{0}','{1}')", username, passwordHash);
            //Create Command
            MySqlCommand cmd = new MySqlCommand(query, connection);
            //Create a data reader and Execute the command
            MySqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Close();
            return true;
        }

        public string GetFromUser(string fieldName, string username)
        {
            string query = String.Format("SELECT {0} FROM users WHERE username = '{1}'", fieldName, username);
            //Create Command
            MySqlCommand cmd = new MySqlCommand(query, connection);
            //Create a data reader and Execute the command
            MySqlDataReader dataReader = cmd.ExecuteReader();

            try
            {
                dataReader.Read();
                string password = dataReader.GetString(0);
                return password;
            }
            catch
            {
                throw new Exception("Nie ma takiego uzytkownika!");
            }
            finally
            {
                dataReader.Close();
            }
        }



        public bool DeleteInvitation(int invitationId)
        {
            string query = String.Format("DELETE FROM invitations WHERE invitation_id = {0}", invitationId);

            //Create Command
            MySqlCommand cmd = new MySqlCommand(query, connection);
            //Create a data reader and Execute the command
            MySqlDataReader dataReader = cmd.ExecuteReader();
            try
            {
                dataReader.Read();
                dataReader.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckIfNameExist(string username)
        {
            string query = string.Format("SELECT username FROM users WHERE username = '{0}'", username);

            //Create Command
            MySqlCommand cmd = new MySqlCommand(query, connection);
            //Create a data reader and Execute the command
            MySqlDataReader dataReader = cmd.ExecuteReader();

            try
            {
                dataReader.Read();
                string usernameFromDatabase = dataReader.GetString(0);
            }
            catch
            {
                dataReader.Close();
                return false;
            }
            //close Data Reader
            dataReader.Close();
            return true;
        }


        public string CreateNewConversation(string IdA, string bUsername, string iv)
        {
            string IdB = GetUserId(bUsername).ToString();
            string query = string.Format("SELECT * FROM conversations WHERE (user1_id = '{0}' AND user2_id = '{1}') OR (user1_id = '{1}'  AND user2_id = '{0}')", IdA, IdB);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            // Check if conversation already exist
            try
            {
                dataReader.Read();
                dataReader.GetString(0);
                return "";
            }
            catch
            {
                ;
            }
            dataReader.Close();


            query = string.Format("INSERT INTO conversations(user1_id,user2_id,iv_to_decrypt_converstion_key) VALUES({0},{1},'{2}')", IdA, IdB, iv);
            cmd = new MySqlCommand(query, connection);
            dataReader = cmd.ExecuteReader();
            dataReader.Close();

            query = "SELECT LAST_INSERT_ID()";
            cmd = new MySqlCommand(query, connection);
            dataReader = cmd.ExecuteReader();
            dataReader.Read();
            try
            {
                return dataReader.GetString(0);
            }
            catch
            {
                return "";
            }
            finally
            {
                dataReader.Close();
            }
        }

        public int GetConversationId(string usernameA, string usernameB)
        {
            string userAId = GetUserId(usernameA).ToString();
            string userBId = GetUserId(usernameB).ToString();

            string query = string.Format("SELECT conversation_id FROM conversations WHERE (user1_id = '{0}' AND user2_id = '{1}') OR (user1_id = '{1}'  AND user2_id = '{0}')", userAId, userBId);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Read();
            string Id = dataReader.GetString(0);
            dataReader.Close();
            return int.Parse(Id);
        }





        public int GetUserId(string username)
        {
            string query = "SELECT user_id FROM users " + String.Format("WHERE username = '{0}'", username);

            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            try
            {
                dataReader.Read();
                int userId = dataReader.GetInt32(0);
                dataReader.Close();
                return userId;
            }
            catch
            {
                throw new Exception("Nie ma takiego uzytkownika!");
            }
            finally
            {
                dataReader.Close();
            }
        }



        public int GetInvitationId(string senderUserName, string reciverUserName)
        {
            string query = String.Format("SELECT invitation_id i FROM invitations JOIN users u ON u.user_id = i.sender JOIN users u2 ON u2.user_id = i.reciver WHERE u.username = {0} AND u2.username = {1}"
                , senderUserName, reciverUserName);

            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();

            try
            {
                dataReader.Read();
                int invitationId = dataReader.GetInt32(0);
                dataReader.Close();
                return invitationId;
            }
            catch
            {
                throw new Exception("Nie ma takiego uzytkownika!");
            }
        }



        public bool DeleteUser(int userId)
        {
            string query = String.Format("DELETE FROM users WHERE user_id = {0}", userId);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            try
            {
                dataReader.Read();
                dataReader.Close();
            }
            catch
            {
                return false;
            }
            finally
            {
                dataReader.Close();
            }
            return true;
        }

        public void CloseConnection()
        {
            
            connection.Close();
        }
    }


}
