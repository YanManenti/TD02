using System.Text.Json.Serialization;

namespace TD02.Models
{
    public class ResponseObject<T>
    {
        public string message { get; set; }
        public List<T> data { get; set; }
    }

}