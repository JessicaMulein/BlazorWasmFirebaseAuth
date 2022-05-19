using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Microsoft.Identity.Firebase.Models
{
    public class ApplicationDbContext : IdentityDbContext<FirebaseUser>
    {
        public ApplicationDbContext()
            : base()
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
