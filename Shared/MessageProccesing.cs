using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class MessageProccesing
    {
        public static Object DeserializeObject(string message)
        {
#if NET472
            string[] separatorString = {"$$"};
            string[] fields = message.Split(separatorString, StringSplitOptions.RemoveEmptyEntries);
#else
            string[] fields = message.Split("$$", StringSplitOptions.RemoveEmptyEntries);
#endif

#if NET472
            char[] separatorChar = {':'};
            int intOption = int.Parse(fields[0].Split(separatorChar, StringSplitOptions.RemoveEmptyEntries)[1]);
#else
            int intOption = int.Parse(fields[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1]);
#endif
            Options option = (Options)intOption;
            //Remove option
            var list = new List<string>(fields);
            list.RemoveAt(0);

            if (list.Count > 0)
            {
#if NET472
                string data = list[0].Split(separatorChar, 2, StringSplitOptions.RemoveEmptyEntries)[1];
#else
                string data = list[0].Split(':', 2, StringSplitOptions.RemoveEmptyEntries)[1];
#endif

                if (option == Options.LOGIN || option == Options.CREATE_USER) return JsonConvert.DeserializeObject<Login>(data);
                else if (option == Options.CHECK_USER_NAME || option == Options.ADD_FRIEND || option == Options.ACTIVE_FRIENDS || option == Options.INVITE_TO_CONVERSATION || option == Options.ACCEPTED_CALL
                    || option == Options.ACCEPTED_CALL || option == Options.DECLINED_CALL || option == Options.INACTIVE_FRIENDS) return JsonConvert.DeserializeObject<Username>(data);
                else if (option == Options.GET_FRIENDS) return JsonConvert.DeserializeObject<List<Friend>>(data);
                else if (option == Options.FRIEND_INVITATIONS) return JsonConvert.DeserializeObject<List<Invitation>>(data);
                else if (option == Options.INCOMMING_CALL) return JsonConvert.DeserializeObject<Call>(data);
                else if (option == Options.ACCEPT_FRIEND || option == Options.DECLINE_FRIEND || option == Options.JOIN_CONVERSATION || option == Options.LEAVE_CONVERSATION || option == Options.CREATE_UDP) return JsonConvert.DeserializeObject<Id>(data);
            }
            return "";
        }

        public static Object DeserializeObject(Options option, string data)
        {
            if (option == Options.LOGIN || option == Options.CREATE_USER) return JsonConvert.DeserializeObject<Login>(data);
            else if (option == Options.CHECK_USER_NAME || option == Options.ADD_FRIEND || option == Options.ACTIVE_FRIENDS || option == Options.INVITE_TO_CONVERSATION || option == Options.ACCEPTED_CALL
                || option == Options.ACCEPTED_CALL || option == Options.DECLINED_CALL || option == Options.INACTIVE_FRIENDS) return JsonConvert.DeserializeObject<Username>(data);
            else if (option == Options.GET_FRIENDS) return JsonConvert.DeserializeObject<List<Friend>>(data);
            else if (option == Options.FRIEND_INVITATIONS) return JsonConvert.DeserializeObject<List<Invitation>>(data);
            else if (option == Options.INCOMMING_CALL) return JsonConvert.DeserializeObject<Call>(data);
            else if (option == Options.ACCEPT_FRIEND || option == Options.DECLINE_FRIEND || option == Options.JOIN_CONVERSATION || option == Options.LEAVE_CONVERSATION) return JsonConvert.DeserializeObject<Id>(data);
            return null;
        }

        public static Object DeserializeObjectOnErrorCode(Options option, string data)
        {
            if (option == Options.GET_FRIENDS) return JsonConvert.DeserializeObject<List<Friend>>(data);
            else if (option == Options.INVITE_TO_CONVERSATION) return JsonConvert.DeserializeObject<Id>(data);
            return null;
        }

        public static string CreateMessage<T>(Options option,T obj)
        {
            return String.Format("{0}Data:{1}$$", CreateMessage(option), JsonConvert.SerializeObject(obj));
        }

        public static string CreateMessage<T>(ErrorCodes error, T obj)
        {
            return String.Format("{0}Data:{1}$$", CreateMessage(error), JsonConvert.SerializeObject(obj));
        }

        public static string CreateMessage(Options option)
        {
            return String.Format("Option:{0}$$", (int)option);
        }
        public static string CreateMessage(ErrorCodes error)
        {
            return String.Format("Error:{0}$$", (int)error);
        }

    }
}

