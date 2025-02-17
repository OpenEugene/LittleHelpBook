using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Oqtane.Models;

namespace OpenEugene.Module.LittleHelpBook.Models;

[Table("PhoneNumber")]
public partial class PhoneNumber : ModelBase
{
    [Key]
    public int PhoneNumberId { get; set; }
    public int ProviderId { get; set; }

    public int? CountryCode { get; set; }

    public int? AreaCode { get; set; }

    [StringLength(500)]
    public string Number { get; set; }

    public int? Extension { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Column("l10N")]
    [StringLength(4000)]
    public string L10N { get; set; }

    public bool IsActive { get; set; }

    public string FullNumber { get {
            var builder = new StringBuilder();
            if (CountryCode>0) {
                builder.AppendFormat("{0} ",CountryCode);
            }
            if (AreaCode > 0) {
                builder.AppendFormat("({0})", AreaCode);
            }
            builder.Append(Number);
            if (Extension > 0) {
                builder.AppendFormat("x{0}", Extension);
            }
            return builder.ToString();
        } 
    }
}