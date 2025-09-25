using System.Text.Json.Serialization;
namespace TodoApi.Models
{
    public class Phone
    {
        public Guid? id { get; set; } = Guid.NewGuid();
        public string value { get; set; } = "";

        // Foreign Key
        public Guid contactId { get; set; }

        [JsonIgnore]
        public virtual Contact contact { get; set; } = null!;
    }
}
