using System.Text.Json.Serialization;

namespace TodoApi.ViewModels
{
    public class IdValueResult
    {
        public Guid? id { get; set; }
        public string value { get; set; }

        public IdValueResult(Guid? id, string value)
        {
            this.id = id;
            this.value = value;
        }
    }
    public class IdValue
    {
        public Guid? id { get; set; }
        public string value { get; set; } = null!;
        [JsonIgnore]
        public int ord { get; set; }
    }
    public class CommandIdValue : IdValue { }

    public class IdValueOnly
    {
        public string value { get; set; } = null!;
    }
}
