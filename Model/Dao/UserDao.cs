using Model.EF;
using System;
using System.Collections.Generic;
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

        public void RecoverPassword(User entity)
        {
            var admin = db.Users.SingleOrDefault(x => x.UserName == entity.UserName);
            admin.Password = entity.Password;
            db.SaveChanges();
        }

        public bool Delete(int id)
        {
            try
            {
                var admin = db.Users.Find(id);
                db.Users.Remove(admin);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public User ViewDetail(int id)
        {
            return db.Users.Find(id);
        }

        public IEnumerable<User> ListAllPaging()
        {
            return db.Users.Include("UserGroups");
        }


        public int Insert(User entity)
        {
            try
            {
                db.Users.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public bool Update(User entity)
        {
            try
            {
                var admin = db.Users.Find(entity.ID);
                admin.Name = entity.Name;
                admin.Address = entity.Address;
                admin.Email = entity.Email;
                admin.Phone = entity.Phone;                
                admin.GroupID = entity.GroupID;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<string> GetListCredential(string userName)
        {
            var user = db.Users.Single(x => x.UserName == userName);
            var data = (from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupID equals b.ID
                        join c in db.Roles on a.RoleID equals c.ID
                        where b.ID == user.GroupID
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });
            return data.Select(x => x.RoleID).ToList();
        }

        public User GetByID(string userName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == userName);
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
        
    }
}