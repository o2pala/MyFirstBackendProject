using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid id { get; set; }
        public DateTime dateCreate { get; set; } = DateTime.UtcNow;
        public Guid userCreate { get; set; }
    }
}
