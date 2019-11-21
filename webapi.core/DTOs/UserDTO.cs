namespace webapi.core.DTOs {
    public class UserDTO {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Identifier { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        // 1: active, 2: banned, 3: deleted
        public int Status { get; set; }

        // 1: ADMIN, 2: STAFF
        public string Role { get; set; }

        public string Password { get; set; }
    }
}