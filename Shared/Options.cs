
namespace Shared
{
    public enum Options
    {
        // Sterring                         ADDITIONAl CLIENT FIELDS                                        SERVER RESPONSE
        LOGOUT = 0,                         //                                                              Error:<>$$
        LOGIN = 1,                          //  Data:<JSONLogin>                                            Error:<>$$
        CREATE_USER = 2,                    //  Data:<JSONLogin>                                            Error:<>$$
        CHECK_USER_NAME = 3,                //  Data:<JSONUsername>                                         Error:<>$$
        DISCONNECT = 4,                     //                                                              Error:<>$$
        GET_FRIENDS = 5,                    //                                                              Error:<>$$Data:<JSONFriend>$$       
        DELETE_ACCOUNT = 6,                 //                                                              Error:<>$$
        ADD_FRIEND = 7,                     //  Data:<JSONUsername>                                         Error:<>$$
        ACCEPT_FRIEND = 8,                  //  Data:<JSONInvitation>                                       Error:<>$$
        DECLINE_FRIEND = 9,                 //  Data:<JSONInvitation>                                       Error:<>$$



        ACCEPTED_FRIENDS = 20,              //  SERVER ONLY                                                 Option:10$$Data:<JSONFriends>$$
        ACTIVE_FRIENDS = 21,                //  SERVER ONLY                                                 Option:11$$Data:<JSONFriends>$$
        FRIEND_INVITATIONS = 22,            //  SERVER ONLY                                                 Option:20$$Data:<JSONInvitation>$$
        INCOMMING_CALL = 23,                //  SERVER ONLY                                                 Option:21$$Data:<JSONCall>$$
    }
}