namespace TodoApi.Models
{
    public class CustomerEmail
    {
        public Guid id { get; set; } = new Guid();
        public string? value { get; set; } = "";

        // Foreign Key
        public Guid customerId { get; set; }
        public virtual Customer customer { get; set; } = null!;
    }
}
