using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;

using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using OpenEugene.Module.LittleHelpBook.Models;
using System;

namespace OpenEugene.Module.LittleHelpBook.Repository
{
    public partial class LittleHelpBookRepository
    {

        public IEnumerable<Category> GetCategories()
        {
            using var db = _factory.CreateDbContext();

            var list = from a in db.Attribute.AsNoTracking()
                       where a.ParentAttributeId == null
                       select new Category()
                       {
                           AttributeId = a.AttributeId,
                           Name = a.Name,
                           Description = a.Description,
                           ParentAttributeId = a.ParentAttributeId,
                           L10N = a.L10N,
                           CreatedBy = a.CreatedBy,
                           CreatedOn = a.CreatedOn,
                           ModifiedBy = a.ModifiedBy,
                           ModifiedOn = a.ModifiedOn,
                       };

            return list.ToList();
        }

    }
}