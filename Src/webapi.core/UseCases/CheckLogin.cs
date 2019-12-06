namespace webapi.core.UseCases {
    public class CheckLogin {
        public string Id { get; set; }
        public int getId () {
            return int.Parse (this.Id);
        }
    }
}