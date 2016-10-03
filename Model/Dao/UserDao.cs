using Model.EF;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace Model.Dao
{
    public class UserDao
    {
        private PhuKienDbContext db = null;

        public UserDao()
        {
            db = new PhuKienDbContext();
        }

        public bool UserNameExists(string userName)
        {
            return db.Users.Any(x => x.UserName == userName);
        }

        public bool EmailExists(string email)
        {
            return db.Users.Any(x => x.Email == email);
        }


        public int Login(string userName, string passWord)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName && x.Password == passWord);
            if (result == null)
                return 0;
            return 1;
        }

        public User GetByUserName(string userName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == userName);
        }

        public User GetByEmail(string email)
        {
            return db.Users.SingleOrDefault(x => x.Email == email);
        }

        public void RecoverPassword(User entity)
        {
            var admin = db.Users.SingleOrDefault(x => x.UserName == entity.UserName);
            admin.Password = entity.Password;
            db.SaveChanges();
        }


        public int Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
    }
}