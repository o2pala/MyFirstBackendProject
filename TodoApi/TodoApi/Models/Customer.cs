using System.ComponentModel.DataAnnotations.Schema;
using TodoApi.ViewModels;

namespace TodoApi.Models
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; } = "";
        public string CustomerStageId { get; set; } = "";
        //public virtual Contact? Contacts { get; set; } = null;
        public virtual ICollection<CustomerEmail> CustomerEmails { get; set; } = new List<CustomerEmail>();
        public virtual ICollection<CustomerPhone> CustomerPhones { get; set; } = new List<CustomerPhone>();
    }
}
