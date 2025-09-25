using TodoApi.Models;

namespace TodoApi.ViewModels
{
    public class ContactQueryRequest
    {
        public string? text { get; set; }
    }
    public class ContactQueryResponse
    {
        public Guid contact_id { get; set; }
        public string name { get; set; } = "";
        public string? description { get; set; } = "";
        public string? position { get; set; } = "";
        public List<IdValueResult>? emails { get; set; } = new();
        public List<IdValueResult>? phones { get; set; } = new();
        //public Guid? customer_id { get; set; }
        public GetCustomerListResponse? customerInfo { get; set; } = new();
    }
    public class IndexDataBaseViewModel
    {
        public DateTime date_create { get; set; }
        public Guid user_create { get; set; }
        public bool can_delete { get; set; } = false;
        public bool can_edit { get; set; } = false;
    }
    public class customer_phone
    {
        public Guid value { get; set; } = Guid.NewGuid();
        public string text { get; set; } = "";
        //public Guid? customerId { get; set; }
}
    public class customer_email
    {
        public string value { get; set; } = "";
        //public Guid? customerId { get; set; }
    }
    public class GetCustomerListResponse : GetCustomerListResult
    {
        public List<string>? customerEmails { get; set; }
        public List<customer_phone>? customerPhones { get; set; }
    }
    public class GetCustomerListResult
    {
        public Guid customer_id { get; set; }
        public string customer_stage_id { get; set; } = null!;
        public string name { get; set; } = null!;

    }
}
