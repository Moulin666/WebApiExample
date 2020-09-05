using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppUser : IdentityUser
    {
        [PersonalData] public string FirstName { get; set; }
        [PersonalData] public string LastName { get; set; }
        [PersonalData] public string SecondName { get; set; }

        public string AvatarUrl { get; set; }
    }
}
