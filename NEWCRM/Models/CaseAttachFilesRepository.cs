using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWCRM.Models
{
    public class CaseAttachFilesRepository : RepositoryBase
    {
        public int AddByEntity(CaseAttachFile data)
        {
            using (var db = GetDBContext())
            {
                db.CaseAttachFiles.Add(data);
                return db.SaveChanges();
            }
        }

        public CaseAttachFile GetByID(int _id)
        {
            using (var db = GetDBContext())
            {
                return db.CaseAttachFiles.SingleOrDefault(c => c.cafID == _id);
            }
        }

        public int RemoveByID(int _id)
        {
            using (var db = GetDBContext())
            {
                var obj = db.CaseAttachFiles.SingleOrDefault(c => c.cafID == _id);
                if (obj != null)
                {
                    db.CaseAttachFiles.Remove(obj);
                    return db.SaveChanges();
                }
                return 0;
            }
        }

        public List<CaseAttachFile> getFiles(int _casid)
        {
            using (var db = GetDBContext())
            {
                return db.CaseAttachFiles.Where(c => c.casID == _casid).OrderBy(c => c.cafCreatedOn).ToList();
            }
        }
    }
}