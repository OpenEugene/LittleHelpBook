using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEugene.Module.LittleHelpBook.Models;

/// <summary>
/// Subcategoryies are 2md level attributes that have a partent attribute
/// </summary>
public partial class SubCategory : Attribute {
    public int SubCategoryId => AttributeId;
}