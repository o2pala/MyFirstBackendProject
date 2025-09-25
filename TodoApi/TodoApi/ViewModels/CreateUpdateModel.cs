using TodoApi.Models;

namespace TodoApi.ViewModels
{
    public class ContactCreateRequest
    {
        public string name { get; set; } = "";
        public string? description { get; set; } = "";
        public string? position { get; set; } = "";
        public List<IdValueResult>? emails { get; set; }
        public List<IdValueResult>? phones { get; set; }
        //public Customer? customerInfo { get; set; }
        public Guid customer_id { get; set; }
    }
    public class ContactUpdateRequest : ContactCreateRequest
    {
        public Guid contact_id { get; set; }
    }
}
