namespace BE.Models {
    using System.ComponentModel.DataAnnotations;
    using System;

    public class User {
        [Key]
        public long id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        /** 0: male, 1: female, 2: other */
        public int gender { get; set; } = 1;
        public Nullable<DateTime> date_of_birth { get; set; }
        public int role { get; set; } = 1;
        /** 0: new, 1: approve, 2: banned, 3: deleted */
        public int status { get; set; } = 0;
        public Nullable<DateTime> updatedAt { get; set; }
        public Nullable<DateTime> createdAt { get; set; }
        public Nullable<DateTime> deletedAt { get; set; }
    }

    public class Pagination {
        public int page { get; set; } = 1;
        public int limit { get; set; } = 10;
    }

    public class Search {
        public string name { get; set; } = "";
        public string email { get; set; } = "";
        public string address { get; set; } = "";
        public string phone { get; set; } = "";
        public int gender { get; set; } = -1;
        public Nullable<DateTime> date_of_birth { get; set; } = null;
        public int role { get; set; }
        public int status { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime deletedAt { get; set; }
    }
}