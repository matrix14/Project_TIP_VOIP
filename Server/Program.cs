using System;
using Shared;
namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = Shared.MessageProccesing.CreateMessage(Shared.Options.DISCONNECT);
            Console.WriteLine(x);
            var y = Shared.MessageProccesing.CreateMessage(Shared.ErrorCodes.ADDING_FRIENDS_ERROR);
            Console.WriteLine(y);
            var z = Shared.MessageProccesing.CreateMessage<Shared.Login>(Shared.Options.LOGIN, new Shared.Login("ale", "jest"));
            Console.WriteLine(z);
            Login k = Shared.MessageProccesing.DeserializeObject(z) as Login;
            Console.WriteLine(k.passwordHash);
        }
    }
}
