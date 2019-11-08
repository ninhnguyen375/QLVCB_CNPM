using System.Linq;
using webapi.core.Domain.Entities;

namespace webapi.infrastructure.Persistance {
  public class SeedData {
    public static void Initialize (AppDbContext context) {
      context.Database.EnsureCreated ();

      if (context.Users.Any ()) {
        return;
      }

      context.Users.AddRange (
        new User {
          Email = "admin@admin.com",
            FullName = "admin",
            Identifier = "00000000",
            Password = "$2b$10$IZshIpJy3mRvjTGJJYD45OOccUcUNI8RrCUvURHcemPbdNfXR/q3i",
            Role = "ADMIN",
            Status = 1
        }
      );

      for (int i = 0; i < 50; i++)
      {
          context.Users.Add (
            new User {
              Email = "ning@ninh.com" + i,
                FullName = "ninh" + i,
                Identifier = "000000" + i,
                Password = "$2b$10$IZshIpJy3mRvjTGJJYD45OOccUcUNI8RrCUvURHcemPbdNfXR/q3i",
                Role = "STAFF",
                Status = 1
            }
        );
      }
      context.SaveChanges ();

      if (context.Airports.Any()) {
        return;
      }

      context.Airports.AddRange(
        new Airport {
          Id = "SGN",
          Name = "Tân Sơn Nhất",
          Location = "Thành phố Hồ Chí Minh"
        },
        new Airport {
          Id = "HAN",
          Name = "Nội Bài",
          Location = "Hà Nội"
        },
        new Airport {
          Id = "HUI",
          Name = "Phú Bài",
          Location = "Huế"
        },
        new Airport {
          Id = "CXR",
          Name = "Cam Ranh",
          Location = "Nha Trang"
        },
        new Airport {
          Id = "PQC",
          Name = "Phú Quốc",
          Location = "Phú Quốc"
        }, 
        new Airport {
          Id = "DAD",
          Name = "Đà Nẵng",
          Location = "Đà Nẵng"
        },
        new Airport {
          Id = "PXU",
          Name = "Pleiku",
          Location = "Pleiku"
        },
        new Airport {
          Id = "DLI",
          Name = "Liên Khương",
          Location = "Đà Lạt"
        },
        new Airport {
          Id = "VDH",
          Name = "Đồng Hới",
          Location = "Đồng Hới"
        },
        new Airport {
          Id = "VCL",
          Name = "Chu Lai",
          Location = "Tam Kỳ"
        },
        new Airport {
          Id = "THD",
          Name = "Thọ Xuân",
          Location = "Thanh Hóa"
        },
        new Airport {
          Id = "VCA",
          Name = "Cần Thơ",
          Location = "Cần Thơ"
        },
        new Airport {
          Id = "TBB",
          Name = "Tuy Hòa",
          Location = "Tuy Hòa"
        },
        new Airport {
          Id = "VCS",
          Name = "Côn Đảo",
          Location = "Côn Đảo"
        },
        new Airport {
          Id = "CAH",
          Name = "Cà Mau",
          Location = "Cà Mau"
        },
        new Airport {
          Id = "HPH",
          Name = "Cát Bi",
          Location = "Hải Phòng"
        },
        new Airport {
          Id = "VKG",
          Name = "Rạch Giá",
          Location = "Rạch Giá"
        },
        new Airport {
          Id = "VII",
          Name = "Vinh",
          Location = "Vinh"
        },
        new Airport {
          Id = "DIN",
          Name = "Điện Biên Phủ",
          Location = "Điện Biên"
        },
        new Airport {
          Id = "VDO",
          Name = "Vân Đồn",
          Location = "Quảng Ninh"
        },
        new Airport {
          Id = "UIH",
          Name = "Phú Cát",
          Location = "Quy Nhơn"
        },
        new Airport {
          Id = "BMV",
          Name = "Buôn Ma Thuột",
          Location = "Buôn Ma Thuột",
        }
      );
      context.SaveChanges();

      if (context.Airlines.Any()) {
        return;
      }

      context.Airlines.AddRange(
        new Airline {
          Id = "QH",
          Name = "Bamboo Airways"
        }, 
        new Airline {
          Id = "VJ",
          Name = "Vietjet Air"
        },
        new Airline {
          Id = "BL",
          Name = "Jetstar Paciffic"
        },
        new Airline {
          Id = "VN",
          Name = "Vietnam Airlines"
        }
      );
      context.SaveChanges();

      if (context.Luggages.Any()) {
        return;
      }

      context.Luggages.AddRange(
        new Luggage {
          LuggageWeight = 40,
          Price = 570000
        },
        new Luggage {
          LuggageWeight = 35,
          Price = 505000
        },
        new Luggage {
          LuggageWeight = 30,
          Price = 440000
        },
        new Luggage {
          LuggageWeight = 25,
          Price = 310000
        },
        new Luggage {
          LuggageWeight = 20,
          Price = 220000
        },
        new Luggage {
          LuggageWeight = 15,
          Price = 200000
        },
        new Luggage {
          LuggageWeight = 0,
          Price = 0
        }
      );
      context.SaveChanges();

      if (context.Flights.Any ()) {
        return;
      }

      context.Flights.AddRange(
        new Flight {
          Id = "VJ100",
          StartTime = 1200,
          FlightTime = 125,
          AirportFrom = "VCS",
          AirportTo = "HAN",
          SeatsCount = 100,
          SeatsLeft = 100,
          Status = 1,
          AirlineId = "VN",
        },
        new Flight {
          Id = "VJ101",
          StartTime = 500,
          FlightTime = 60,
          AirportFrom = "VDO",
          AirportTo = "UIH",
          SeatsCount = 100,
          SeatsLeft = 100,
          Status = 1,
          AirlineId = "VJ",
        }
      );
      context.SaveChanges();

      if (context.TicketCategories.Any ()) {
        return;
      }

      context.TicketCategories.AddRange(
        new TicketCategory {
          Name = "Em Bé"
        },
        new TicketCategory {
          Name = "Trẻ Em"
        },
        new TicketCategory {
          Name = "Nguời Lớn"
        }
      );
      context.SaveChanges();
      
      if (context.FlightTicketCategories.Any ()) {
        return;
      }

      context.FlightTicketCategories.AddRange(
        new FlightTicketCategory {
          FlightId = "VJ100",
          TicketCategoryId = 1,
          Price = 700000
        },
        new FlightTicketCategory {
          FlightId = "VJ100",
          TicketCategoryId = 2,
          Price = 100000
        },
        new FlightTicketCategory {
          FlightId = "VJ101",
          TicketCategoryId = 1,
          Price = 600000
        },
        new FlightTicketCategory {
          FlightId = "VJ101",
          TicketCategoryId = 2,
          Price = 100000
        }
      );
      context.SaveChanges();
    }
  }
}