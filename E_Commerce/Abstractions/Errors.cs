namespace E_Commerce.Abstractions
{
    public static class Errors
    {
        public class UserErrors
        {
            public static readonly Error InvalidCredentionals =
                new Error("User.InvalidCredentionals", "Invalid Username or password", StatusCodes.Status401Unauthorized);
            public static readonly Error DuplicateEmail =
                new Error("User.DuplicatedEmail", "Another User with the same Email", StatusCodes.Status409Conflict);
            public static readonly Error EmailNotConfirmed =
                new Error("User.EmailNotConfirmed", "EmailNotConfirmed", StatusCodes.Status401Unauthorized);
            public static readonly Error DuplicateConfirmation =
                new Error("User.DuplicateConfirmation", "This email had been confirmed before", StatusCodes.Status409Conflict);
            public static readonly Error NotFound =
                new Error("User.NotFound", "This Id is not valid", StatusCodes.Status404NotFound);
        }
        public class TokenErrors
        {
            public static readonly Error EmptyToken =
                new Error("NotFound", "Null Refrence", StatusCodes.Status404NotFound);
        }
        public class ItemErrors
        {
            public static readonly Error Emptyitem =
                new Error("Item.NotFound", "No Item with this id", StatusCodes.Status404NotFound);
        }
    }
}
