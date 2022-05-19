using Microsoft.Identity.Firebase.Models;

namespace Microsoft.Identity.Firebase.Services
{
    public class BaseService
    {
        protected ApplicationDbContext db;

        public BaseService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public BaseService() { }
        public void Dispose()
        {
            this.db.Dispose();
        }
    }
}