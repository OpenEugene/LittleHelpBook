using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace OpenEugene.Module.LittleHelpBook.Models;

/// <summary>
/// categoryies are top level attributes that have no partent attribute
/// </summary>
public partial class Category : Attribute {
    public int CategoryId => AttributeId;
 }