namespace webapi.core.UseCases
{
    public class SearchLuggage
    {
        public int? LuggageWeight { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public string sortAsc { get; set; } = "";
        public string sortDesc { get; set; } = "";

    }
}