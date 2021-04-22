
namespace Shared
{
    public enum Options
    {
        // Sterring                         ADDITIONAl CLIENT FIELDS                                        SERVER RESPONSE
        LOGOUT = 0,                         //                                                              Error:<>$$
        LOGIN = 1,                          //  Data:<JSONLogin>$$                                          Error:<>$$
        CREATE_USER = 2,                    //  Data:<JSONLogin>$$                                          Error:<>$$
        CHECK_USER_NAME = 3,                //  Data:<JSONUsername>$$                                       Error:<>$$
        GET_FRIENDS = 4,                    //                                                              Error:<>$$Data:<JSONFriend>$$       
        DELETE_ACCOUNT = 5,                 //                                                              Error:<>$$
        ADD_FRIEND = 6,                     //  Data:<JSONUsername>$$                                       Error:<>$$
        ACCEPT_FRIEND = 7,                  //  Data:<JSONInvitationId>$$                                   Error:<>$$
        DECLINE_FRIEND = 8,                 //  Data:<JSONInvitationId>$$                                   Error:<>$$
        INVITE_TO_CONVERSATION = 9,         //  Data:<JSONUsername>$$                                       Error:<>$$
        JOIN_CONVERSATION = 10,             //  Data:<JSONInvitationId>$$                                   Error:<>$$
        LEAVE_CONVERSATION = 11,            //  Data:<JSONInvitationId>$$                                   Error:<>$$



        ACTIVE_FRIENDS = 21,                //  SERVER ONLY                                                 Option:21$$Data:<JSONUsername>$$
        FRIEND_INVITATIONS = 22,            //  SERVER ONLY                                                 Option:22$$Data:<JSONInvitations>$$
        INCOMMING_CALL = 23,                //  SERVER ONLY                                                 Option:23$$Data:<JSONCall>$$
        ACCEPTED_CALL = 24,                 //  SERVER ONLY                                                 Option:24$$Data:<Username>$$
        DECLINED_CALL = 25,                 //  SERVER ONLY                                                 Option:24$$Data:<Username>$$
    }
}