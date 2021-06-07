
namespace Shared
{
    public enum ErrorCodes
    {   // General
        NO_ERROR = 0,
        DELETING_ACCOUNT_ERROR = 15,
        // Login
        NOT_LOGGED_IN = 1,
        USER_NOT_FOUND = 2,
        DB_CONNECTION_ERROR = 3,
        USER_ALREADY_LOGGED_IN = 4,
        INCORRECT_PASSWORD = 5,
        // Registry
        USER_ALREADY_EXISTS = 6,
        // Friend invitation
        ALREADY_FRIENDS = 7,
        ADDING_FRIENDS_ERROR = 8,
        WRONG_INVATATION_ID = 9,
        SELF_INVITE_ERROR = 10,
        INVITATION_ALREADY_EXIST = 11,
        INVITATION_ALREADY_ACCEPTED = 12,
        // Conversation
        CONVERSATION_ALREADY_STARTED = 13,
        WRONG_CONVERSATION_ID = 14,
        USER_OFFLINE = 15,
    }
}
