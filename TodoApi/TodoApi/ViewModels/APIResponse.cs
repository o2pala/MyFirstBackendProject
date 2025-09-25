namespace TodoApi.ViewModels
{
    public class APIResponse<T>
    {
        public T? data { get; set; }
        public Guid id { get; set; }

        public int status { get; set; }

        public string? message { get; set; }

        public void Deconstruct(out Guid id, out int status, out T data)
        {
            id = this.id;
            status = this.status;
            data = this.data;
        }
    }
    public class APIResponse
    {
        public Guid id { get; set; }

        public int status { get; set; }

        public string? message { get; set; }

        public void Deconstruct(out int status)
        {
            status = this.status;
        }

        public void Deconstruct(out Guid id, out int status)
        {
            id = this.id;
            status = this.status;
        }
    }
}
