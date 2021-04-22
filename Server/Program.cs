using System;
using System.Collections.Generic;
using Shared;
namespace Server
{

    class Program
    {
        static void Main(string[] args)
        {
            //StartTests();
        }


        public static string RegisterUser(int clientId,string name,string password,ClientProcessing cp)
        {
            return cp.CreateUser(MessageProccesing.CreateMessage<Login>(Options.CREATE_USER, new Login(name, password)), clientId);
        }

        public static string Login(int clientId, string name, string password, ClientProcessing cp)
        {
            return cp.Login(MessageProccesing.CreateMessage<Login>(Options.LOGIN, new Login(name, password)), clientId);
        }

        public static string CheckUsername(int clientId, string name, ClientProcessing cp)
        {
            return cp.CheckUserName(MessageProccesing.CreateMessage<Username>(Options.CHECK_USER_NAME, new Username(name)), clientId);
        }

        public static string AddFriend(int clientId,string username, ClientProcessing cp)
        {
            return cp.AddFriend(MessageProccesing.CreateMessage<Username>(Options.ADD_FRIEND, new Username(username)), clientId);
        }

        public static string AcceptFriend(int clientId,int invitationId, ClientProcessing cp)
        {
            return cp.AcceptFriend(MessageProccesing.CreateMessage<InvitationId>(Options.ACCEPT_FRIEND, new InvitationId(invitationId)), clientId);
        }

        public static string DeclineFriend(int clientId, int invitationId, ClientProcessing cp)
        {
            return cp.DeclineFriend(MessageProccesing.CreateMessage<InvitationId>(Options.DECLINE_FRIEND, new InvitationId(invitationId)), clientId);
        }

        public static string SendInvitations(int clientId, ClientProcessing cp)
        {
            return cp.SendInvitations(clientId,"test1");
        }

        public static void InvitationProcces(ClientProcessing cp, int fId, int sId)
        {
            Console.WriteLine("Add friend:   " + AddFriend(fId, "test1", cp));
            Console.WriteLine("Send invs:   " + SendInvitations(sId, cp));
            Console.WriteLine("Send invs:   " + SendInvitations(sId, cp));
            //Console.WriteLine("Decline friend:   " + DeclineFriend(sId, 1, cp));
            Console.WriteLine("Acc friend:   " + AcceptFriend(sId, 1, cp));
            Console.WriteLine("Send invs:   " + SendInvitations(sId, cp));
            Console.WriteLine("Send invs:   " + SendInvitations(fId, cp));
        }

        public static void StartTests()
        {
            ClientProcessing cp = new ClientProcessing();
            int fId = cp.AddActiveUser();
            int sId = cp.AddActiveUser();


            Console.WriteLine(RegisterUser(fId, "test", "haslo1", cp));
            Console.WriteLine(RegisterUser(sId, "test1", "haslo2", cp));

            Console.WriteLine(Login(fId, "test", "haslo1", cp));
            Console.WriteLine(Login(sId, "test1", "haslo2", cp));

            Console.WriteLine(CheckUsername(fId, "test1", cp));
            Console.WriteLine(CheckUsername(fId, "test2", cp));

            InvitationProcces(cp, fId, sId);
            //Console.WriteLine(DeclineFriend(fId, 1, cp));



        }
    }
}
