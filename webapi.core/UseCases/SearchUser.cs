using webapi.core.DTOs;

namespace webapi.core.UseCases
{
    public class SearchUser : UserDTO
    {
        public string SortAsc { get; set; } = "";
        public string SortDesc { get; set; } = "";
    }
}