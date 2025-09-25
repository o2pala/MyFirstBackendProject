using System.ComponentModel.DataAnnotations.Schema;
using TodoApi.Models;

public class Contact : BaseEntity
{
    public string name { get; set; } = "";
    public string? description { get; set; }
    public string? position { get; set; }
    
    public Guid customerId { get; set; }
    [ForeignKey(nameof(customerId))]
    public virtual Customer Customer { get; set; } = null!;

    // --- Navigation Properties ---
    // 1 Contact มีได้หลาย Email
    public virtual ICollection<Email> emails { get; set; } = new List<Email>();

    // 1 Contact มีได้หลาย Phone
    public virtual ICollection<Phone> phones { get; set; } = new List<Phone>();

}
