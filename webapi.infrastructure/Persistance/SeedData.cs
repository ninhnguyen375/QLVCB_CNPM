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
            Role = 1,
            Status = 1
        }
      );
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
          Id = "VN",
          Name = "Vietnam Airlines"
        },
        new Airline {
          Id = "QH",
          Name = "Bamboo Airways"
        },
        new Airline {
          Id = "BL",
          Name = "Jetstar Paciffic"
        },
        new Airline {
          Id = "VJ",
          Name = "Vietjet Air"
        }
      );
      context.SaveChanges();
    }
  }
}