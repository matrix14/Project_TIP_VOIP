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
            string[] fields = message.Split("$$", StringSplitOptions.RemoveEmptyEntries);

            int intOption = int.Parse(fields[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1]);
            Options option = (Options)intOption;
            //Remove option
            var list = new List<string>(fields);
            list.RemoveAt(0);

            if (list.Count > 0)
            {
                string data =list[0].Split(':',2, StringSplitOptions.RemoveEmptyEntries)[1];

                if (option == Options.LOGIN || option == Options.CREATE_USER) return JsonConvert.DeserializeObject<Login>(data);
                else if (option == Options.CHECK_USER_NAME || option == Options.ADD_FRIEND || option == Options.ACTIVE_FRIENDS || option == Options.INVITE_TO_CONVERSATION || option == Options.ACCEPTED_CALL) return JsonConvert.DeserializeObject<Username>(data);
                else if (option == Options.GET_FRIENDS) return JsonConvert.DeserializeObject<List<Friend>>(data);
                else if (option == Options.FRIEND_INVITATIONS) return JsonConvert.DeserializeObject<List<Invitation>>(data);
                else if (option == Options.INCOMMING_CALL) return JsonConvert.DeserializeObject<Call>(data);
                else if (option == Options.ACCEPT_FRIEND || option == Options.DECLINE_FRIEND || option == Options.JOIN_CONVERSATION || option == Options.LEAVE_CONVERSATION) return JsonConvert.DeserializeObject<InvitationId>(data);
            }
            return "";
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

