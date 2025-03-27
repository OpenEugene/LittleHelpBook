using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;

using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using OpenEugene.Module.LittleHelpBook.Models;
using System.Threading.Tasks;

namespace OpenEugene.Module.LittleHelpBook.Repository
{
    public partial class LittleHelpBookRepository
    {

        public IEnumerable<Attribute> GetAttributes() {
            using var db = _factory.CreateDbContext();
            var list = from a in db.Attribute.AsNoTracking()
                       select a;
            return list.ToList();
        }

        public Models.Attribute GetAttribute(int id)
        {
            return GetAttribute(id, true);
        }

        public Models.Attribute GetAttribute(int id, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.Attribute.Find(id);
            }
            else
            {
                return db.Attribute.AsNoTracking().FirstOrDefault(item => item.AttributeId == id);
            }
        }

        public async Task DeleteAttributeAsync(int id)
        {
            using var db = _factory.CreateDbContext();
            Models.Attribute item = db.Attribute.Find(id);
            db.Attribute.Remove(item);
            await db.SaveChangesAsync();
        }

    }
}