using System;
using System.Collections.Generic;
using System.Linq;
using webapi.core.Domain.Entities;
using webapi.core.UseCases;

namespace webapi.infrastructure.Persistance {
  public class SeedData {
    public static void Initialize (AppDbContext context) {
      context.Database.EnsureCreated ();

      // 1. Users
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
                Email = "ninh" +  i + "@ninh.com",
                FullName = "ninh" + i,
                Identifier = "000000" + i,
                Password = "$2b$10$IZshIpJy3mRvjTGJJYD45OOccUcUNI8RrCUvURHcemPbdNfXR/q3i",
                Role = "STAFF",
                Status = 1
            }
        );
      }
      context.SaveChanges ();

      // 2. Airports
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

      // 3. Airlines
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

      // 4. Luggages
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

      // 5. TicketCategories
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
          Name = "Người Lớn"
        }
      );
      context.SaveChanges();

      // 6. Dates
      if (context.Dates.Any ()) {
        return;
      }

      int month = 11; // Khởi tạo là tháng 11

      for (int i = 1; i <= 31; i++) {
        // Kiểm tra ngày của tháng
        if (month == 11 && i == 31) {
          month = 12;
          i = 1;
        }

        string day = i < 9 ? "0" + i : i.ToString(); // Xử lý ngày

        context.Dates.Add(new Date {
          DepartureDate = DateTime.Parse("2019/" + month + "/" + day)
        });
        context.SaveChanges();
      }

      // 7. Flights
      if (context.Flights.Any ()) {
        return;
      }

      var airlines = context.Airlines.ToList(); // Lấy tất cả các hãng hàng không

      // Random khoảng thời gian khởi hành
      Random r = new Random();
      int[] startTimeArr = new int[] {
        300, 330, 360, 390, 420, 470, 500, 540, 600, 660, 690, 720, // 5h -> 12h
        780, 840, 870, 900, 915, 960, 1005, 1030, 1065,             // 12h -> 18h
        1080, 1100, 1140, 1155, 1200, 1230, 1260, 1280, 1320, 1380  // 18h -> 24h
      };

      int numberId = 1; // Mã số chuyến bay
      int flightCount = 0; // Số chuyến bay theo tuyến 
      string flightId = ""; // Lưu mã chuyến bay
      int startTime = 0;
      int percentByAirline = 0;
      int percentByStartTime = 0;
      decimal price;

      FlightTimeList FlightTimeList = new FlightTimeList();

      foreach (var airline in airlines) {
        numberId = 1; // Reset mã số chuyến bay theo hãng

        foreach (var ftl in FlightTimeList.TimeList) {
          foreach (var fto in ftl.FlightTimeObjs) {
            // Lấy số chuyến bay
            if (airline.Id == "VN" || airline.Id == "VJ") {
              if (fto.FlightTime > 0 && ( // Nếu có chuyến bay và bay đến
                  fto.Id == "HAN" ||      // 6 tỉnh này thì nhiều chuyến hơn
                  fto.Id == "DAD" ||
                  fto.Id == "CXR" ||
                  fto.Id == "HUI" ||
                  fto.Id == "PQC" ||
                  fto.Id == "DLI")) {
                flightCount = 5; // 5 chuyến
              } else if (fto.Id != ftl.AirportId && fto.FlightTime > 0) { // Ngược lại thì ít chuyến
                flightCount = 2; // 2 chuyến
              }
            } else { // Các hãng hàng không còn lại
              if (fto.FlightTime > 0 && (
                  fto.Id == "HAN" || 
                  fto.Id == "DAD" ||
                  fto.Id == "CXR" ||
                  fto.Id == "HUI" ||
                  fto.Id == "PQC" ||
                  fto.Id == "DLI")) {
                flightCount = 2; // 2 chuyến
              } else if (fto.Id != ftl.AirportId && fto.FlightTime > 0) {
                flightCount = 1; // 1 chuyến
              }
            }

            // Thêm chuyến bay vào CSDL
            while (flightCount > 0) {
              // Thêm chuyến bay
              flightId = airline.Id + autoId(numberId); // Lấy mã chuyến bay
              startTime = startTimeArr[r.Next(0, startTimeArr.Length)];

              context.Flights.Add(new Flight {
                Id = flightId,
                StartTime = startTime,
                FlightTime = fto.FlightTime,
                AirportFrom = ftl.AirportId,
                AirportTo = fto.Id,
                SeatsCount = 100,
                Status = 1,
                AirlineId = airline.Id,
              });

              // 8. FlightTicketCategories
              // Tính % để lấy giá theo từng hãng hàng không
              switch (airline.Id) {
                case "VJ":
                  percentByAirline = 20;
                  break;
                case "BL":
                  percentByAirline = 15;
                  break;
                case "QH":
                  percentByAirline = 10;
                  break;
                default: 
                  percentByAirline = 0;
                  break;
              }

              // Tính % để lấy giá theo từng giờ khởi hành
              if (startTime >= 540 && startTime <= 720) { // 9h -> 12h
                percentByStartTime = 5;
              } else if (startTime > 720 && startTime <= 1080) { // 12h01 -> 18h
                percentByStartTime = 10;
              } else if (startTime > 1080 && startTime <= 1320) { // 18h01 -> 22h
                percentByStartTime = 12;
              } else {
                percentByStartTime = 0;
              }

              // Thêm các loại vé của chuyến bay
              foreach (var ftc in fto.FlightTicketCategories) {
                // Price by Airline
                price = ftc.Price - (ftc.Price * percentByAirline / 100);

                // Price by StartTime
                price = price + (price * percentByStartTime / 100);

                context.FlightTicketCategories.Add(new FlightTicketCategory {
                  FlightId = flightId,
                  TicketCategoryId = ftc.TicketCategoryId,
                  Price = price
                });
              }

              // 9. DateFlights
              // Thêm các chuyến bay vào ngày bay
              for (int i = 20; i <= 61; i++) {
                context.DateFlights.Add(new DateFlight {
                  DateId = i,
                  FlightId = flightId,
                  SeatsLeft = 100,
                  Status = 1
                });
              }

              numberId++;
              flightCount--;
            }     
          }
        }
      }
      context.SaveChanges();
    }

    // Auto Id cho Mã chuyến bay
    private static string autoId(int n) {
      if (n <= 9) {
        return "00" + n;
      } else if (n <= 99) {
        return "0" + n;
      }

      return n.ToString();
    }
  }
}