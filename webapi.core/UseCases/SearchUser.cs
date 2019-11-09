namespace webapi.core.UseCases
{
    public class SearchUser
    {
        public string fullname { get; set; } = "";
        public string identifier { get; set; } = "";
        public string phone { get; set; } = "";
        public string email { get; set; } = "";
        public int? gender { get; set; }

        public string sortAsc { get; set; } = "";
        public string sortDesc { get; set; } = "";
    }
}