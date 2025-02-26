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
        public ProviderAttribute GetProviderAttribute(int id)
        {
            return GetProviderAttribute(id, true);
        }


        public Models.ProviderAttribute GetProviderAttribute(int id, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.ProviderAttribute.Find(id);
            }
            else
            {
                return db.ProviderAttribute.AsNoTracking().FirstOrDefault(item => item.ProviderAttributeId ==id);
            }
        }

        public async Task DeleteProviderAttributeAsync(int id)
        {
            using var db = _factory.CreateDbContext();
            Models.ProviderAttribute item = db.ProviderAttribute.Find(id);
            db.ProviderAttribute.Remove(item);
            await db.SaveChangesAsync();
        }


        public ProviderAttribute AddProviderAttribute(ProviderAttribute item)
        {
            using var db = _factory.CreateDbContext();
            db.ProviderAttribute.Add(item);
            db.SaveChanges();
            return item;
        }

        public void DeleteProviderAttribute(int id)
        {
            using var db = _factory.CreateDbContext();
            var item = db.ProviderAttribute.Find(id);

            if (item == null) return;  // error looging?
            db.ProviderAttribute.Remove(item);
            db.SaveChanges();

        }

    }
}