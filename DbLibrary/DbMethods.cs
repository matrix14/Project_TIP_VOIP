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
            //Create Command
            MySqlCommand cmd = new MySqlCommand(query, connection);
            //Create a data reader and Execute the command
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

        /*
        public Dictionary<int, ExtendedInvitation> GetInvitations()
        {
            string query = String.Format("SELECT * FROM invitations_view");
            //Create Command
            MySqlCommand cmd = new MySqlCommand(query, connection);
            //Create a data reader and Execute the command
            MySqlDataReader dataReader = cmd.ExecuteReader();
            Dictionary<int, ExtendedInvitation> result = new Dictionary<int, ExtendedInvitation>();
            while (dataReader.Read())
            {
                ExtendedInvitation ei = new ExtendedInvitation();
                ei.invitationId = dataReader.GetInt32("invitation_id");
                ei.sender = dataReader.GetString("sender");
                ei.reciver = dataReader.GetString("reciver");
                ei.p = dataReader.GetString("p");
                ei.g = dataReader.GetString("g");
                ei.sended = dataReader.GetBoolean("sended");
                ei.accepted = dataReader.GetBoolean("accepted");
                ei.encryptedSenderPrivateKey = dataReader.GetString("sender_encypted_private_dh_key");
                ei.ivToDecryptSenderPrivateKey = dataReader.GetString("sender_iv_to_decrypt_private_dh_key");
                ei.publicKeySender = dataReader.GetString("sender_public_dh_key");

                if (ei.accepted)
                {
                    ei.publicKeyReciver = dataReader.GetString("reciver_public_dh_key");
                }

                result[ei.invitationId] = ei;
            }
            dataReader.Close();
            return result;
        }
        */




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
