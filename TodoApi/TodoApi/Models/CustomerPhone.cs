using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class CustomerPhone
    {
        [Key]
        public Guid value { get; set; } = Guid.NewGuid();
        public string text { get; set; } = "";

        // Foreign Key
        public Guid customerId { get; set; }
        public virtual Customer customer { get; set; } = null!;
    }
}
