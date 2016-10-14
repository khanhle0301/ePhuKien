using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CredentialDao
    {
        private PhuKienDbContext db = null;

        public CredentialDao()
        {
            db = new PhuKienDbContext();
        }


        public void Insert(string roleID, int userGrID)
        {
            var credential = new Credential();
            credential.UserGroupID = userGrID;
            credential.RoleID = roleID;
            db.Credentials.Add(credential);
            db.SaveChanges();
        }


        public void Create(UserGroup userGroup, string Role)
        {
            if (!string.IsNullOrEmpty(Role))
            {
                foreach (var role in Role.Split(','))
                {
                    this.Insert(role, userGroup.ID);
                }
            }

        }

        public void Update(UserGroup userGroup, string Role)
        {
            this.RemoveAllCredential(userGroup.ID);         
            if (!string.IsNullOrEmpty(Role))
            {
                foreach (var role in Role.Split(','))
                {
                    this.Insert(role, userGroup.ID);
                }
            }
        }

        public void RemoveAllCredential(int id)
        {
            db.Credentials.RemoveRange(db.Credentials.Where(x => x.UserGroupID == id));
            db.SaveChanges();
        }
    }
}
