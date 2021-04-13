
namespace Shared
{
    public enum ErrorCodes
    {   // General
        NO_ERROR = 0,

        // Login
        NOT_LOGGED_IN = 1,
        USER_NOT_FOUND = 2,
        DB_CONNECTION_ERROR = 3,
        USER_ALREADY_LOGGED_IN = 4,
        INCORRECT_PASSWORD = 5,
        // Registry
        USER_ALREADY_EXISTS = 6,
        CONVERSATION_ALREADY_STARTED = 7,
        WRONG_CONVERSATION_ID = 8,
        ALREADY_FRIENDS = 9,
        ADDING_FRIENDS_ERROR = 10,
        WRONG_INVATATION_ID = 11,
        SELF_INVITE_ERROR = 12,
        DELETING_ACCOUNT_ERROR = 13,
        INVITATION_ALREADY_EXIST = 14,
        INVITATION_ALREADY_ACCEPTED = 15,
    }
}
