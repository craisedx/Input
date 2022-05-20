using Input.Constants;
using Microsoft.AspNetCore.Identity;

namespace Input.Identity
{
    public class AppErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            var error = base.DuplicateUserName(userName);
            error.Description = UserErrorsConstants.UserNameAlreadyUse(userName);
            return error;
        }

        public override IdentityError DuplicateEmail(string email)
        {
            var error = base.DuplicateEmail(email);
            error.Description = UserErrorsConstants.EmailAlreadyUse(email);
            return error;
        }
    }
}