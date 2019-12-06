using System.Collections.Generic;
using webapi.core.Domain.Entities;

namespace webapi.core.UseCases
{
    public class FlightTimeObj
    {
        public string Id { get; set; }
        public int FlightTime { get; set; }
        public IList<FlightTicketCategory> FlightTicketCategories { get; set; }
    }

    public class FlightTimes
    {
        public string AirportId { get; set; }
        public IList<FlightTimeObj> FlightTimeObjs { get; set; }
    }

    public class FlightTimeList
    {
        public IList<FlightTimes> TimeList { get; set; }
        
        public FlightTimeList() {
          TimeList = new List<FlightTimes>() {
            new FlightTimes {
              AirportId = "SGN",
              FlightTimeObjs = fromSGNList
            },
            new FlightTimes {
              AirportId = "HAN",
              FlightTimeObjs = fromHANList
            },
            new FlightTimes {
              AirportId = "DAD",
              FlightTimeObjs = fromDADList
            },
            new FlightTimes {
              AirportId = "CXR",
              FlightTimeObjs = fromCXRList
            },
            new FlightTimes {
              AirportId = "DLI",
              FlightTimeObjs = fromDLIList
            }
          };
        }

        // From SGN to ... 
        IList<FlightTimeObj> fromSGNList = new List<FlightTimeObj>() {
          new FlightTimeObj {
            Id = "SGN",
            FlightTime = 0
          },
          new FlightTimeObj { 
            Id = "HAN",
            FlightTime = 125,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "HAN",
                TicketCategoryId = 1,
                Price = 1250000
              },
              new FlightTicketCategory  {
                FlightId = "HAN",
                TicketCategoryId = 2,
                Price = 1120000
              },
              new FlightTicketCategory  {
                FlightId = "HAN",
                TicketCategoryId = 3,
                Price = 200000
              }
            }
          },
          new FlightTimeObj { 
            Id = "DAD",
            FlightTime = 80,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "DAD",
                TicketCategoryId = 1,
                Price = 812000
              },
              new FlightTicketCategory  {
                FlightId = "DAD",
                TicketCategoryId = 2,
                Price = 645000
              },
              new FlightTicketCategory  {
                FlightId = "DAD",
                TicketCategoryId = 3,
                Price = 47000
              }
            }
          },
          new FlightTimeObj { 
            Id = "CXR",
            FlightTime = 60,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "CXR",
                TicketCategoryId = 1,
                Price = 628000
              },
              new FlightTicketCategory  {
                FlightId = "CXR",
                TicketCategoryId = 2,
                Price = 537000
              },
              new FlightTicketCategory  {
                FlightId = "CXR",
                TicketCategoryId = 3,
                Price = 35000
              }
            }
          },
          new FlightTimeObj { 
            Id = "HUI",
            FlightTime = 85,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "HUI",
                TicketCategoryId = 1,
                Price = 758000
              },
              new FlightTicketCategory  {
                FlightId = "HUI",
                TicketCategoryId = 2,
                Price = 502000
              },
              new FlightTicketCategory  {
                FlightId = "HUI",
                TicketCategoryId = 3,
                Price = 130000
              }
            }
          },
          new FlightTimeObj { 
            Id = "PQC",
            FlightTime = 65,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "PQC",
                TicketCategoryId = 1,
                Price = 758000
              },
              new FlightTicketCategory  {
                FlightId = "PQC",
                TicketCategoryId = 2,
                Price = 502000
              },
              new FlightTicketCategory  {
                FlightId = "PQC",
                TicketCategoryId = 3,
                Price = 130000
              }
            }
          },
          new FlightTimeObj { 
            Id = "DLI",
            FlightTime = 60,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "DLI",
                TicketCategoryId = 1,
                Price = 1237000
              },
              new FlightTicketCategory  {
                FlightId = "DLI",
                TicketCategoryId = 2,
                Price = 1085000
              },
              new FlightTicketCategory  {
                FlightId = "DLI",
                TicketCategoryId = 3,
                Price = 95000
              }
            }
          },
          new FlightTimeObj { 
            Id = "PXU",
            FlightTime = 70,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "PXU",
                TicketCategoryId = 1,
                Price = 738000
              },
              new FlightTicketCategory  {
                FlightId = "PXU",
                TicketCategoryId = 2,
                Price = 636000
              },
              new FlightTicketCategory  {
                FlightId = "PXU",
                TicketCategoryId = 3,
                Price = 44000
              }
            }
          },
          new FlightTimeObj { 
            Id = "VDH",
            FlightTime = 100,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "VDH",
                TicketCategoryId = 1,
                Price = 1467000
              },
              new FlightTicketCategory  {
                FlightId = "VDH",
                TicketCategoryId = 2,
                Price = 1300000
              },
              new FlightTicketCategory  {
                FlightId = "VDH",
                TicketCategoryId = 3,
                Price = 110000
              }
            }
          },
          new FlightTimeObj { 
            Id = "VCL",
            FlightTime = 80,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "VCL",
                TicketCategoryId = 1,
                Price = 1177000
              },
              new FlightTicketCategory  {
                FlightId = "VCL",
                TicketCategoryId = 2,
                Price = 1032000
              },
              new FlightTicketCategory  {
                FlightId = "VCL",
                TicketCategoryId = 3,
                Price = 88000
              }
            }
          },
          new FlightTimeObj { 
            Id = "THD",
            FlightTime = 120,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "THD",
                TicketCategoryId = 1,
                Price = 1027000
              },
              new FlightTicketCategory  {
                FlightId = "THD",
                TicketCategoryId = 2,
                Price = 904000
              },
              new FlightTicketCategory  {
                FlightId = "THD",
                TicketCategoryId = 3,
                Price = 44000
              }
            }
          },
          new FlightTimeObj {
            Id = "VCA",
            FlightTime = 0
          },
          new FlightTimeObj { 
            Id = "TBB",
            FlightTime = 120,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "TBB",
                TicketCategoryId = 1,
                Price = 608000
              },
              new FlightTicketCategory  {
                FlightId = "TBB",
                TicketCategoryId = 2,
                Price = 602000
              },
              new FlightTicketCategory  {
                FlightId = "TBB",
                TicketCategoryId = 3,
                Price = 53000
              }
            }
          },
          new FlightTimeObj { 
            Id = "VCS",
            FlightTime = 60,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "VCS",
                TicketCategoryId = 1,
                Price = 1826000
              },
              new FlightTicketCategory  {
                FlightId = "VCS",
                TicketCategoryId = 2,
                Price = 1717000
              },
              new FlightTicketCategory  {
                FlightId = "VCS",
                TicketCategoryId = 3,
                Price = 153000
              }
            }
          },
          new FlightTimeObj { 
            Id = "CAH",
            FlightTime = 60,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "CAH",
                TicketCategoryId = 1,
                Price = 1826000
              },
              new FlightTicketCategory  {
                FlightId = "CAH",
                TicketCategoryId = 2,
                Price = 1717000
              },
              new FlightTicketCategory  {
                FlightId = "CAH",
                TicketCategoryId = 3,
                Price = 153000
              }
            }
          },
          new FlightTimeObj { 
            Id = "HPH",
            FlightTime = 120,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "HPH",
                TicketCategoryId = 1,
                Price = 1467000
              },
              new FlightTicketCategory  {
                FlightId = "HPH",
                TicketCategoryId = 2,
                Price = 1300000
              },
              new FlightTicketCategory  {
                FlightId = "HPH",
                TicketCategoryId = 3,
                Price = 110000
              }
            }
          },
          new FlightTimeObj { 
            Id = "VKG",
            FlightTime = 50,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "VKG",
                TicketCategoryId = 1,
                Price = 1617000
              },
              new FlightTicketCategory  {
                FlightId = "VKG",
                TicketCategoryId = 2,
                Price = 1428000
              },
              new FlightTicketCategory  {
                FlightId = "VKG",
                TicketCategoryId = 3,
                Price = 132000
              }
            }
          },
          new FlightTimeObj { 
            Id = "VII",
            FlightTime = 110,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "VII",
                TicketCategoryId = 1,
                Price = 880000
              },
              new FlightTicketCategory  {
                FlightId = "VII",
                TicketCategoryId = 2,
                Price = 831000
              },
              new FlightTicketCategory  {
                FlightId = "VII",
                TicketCategoryId = 3,
                Price = 130000
              }
            }
          },
          new FlightTimeObj { 
            Id = "DIN",
            FlightTime = 150,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "DIN",
                TicketCategoryId = 1,
                Price = 1960000
              },
              new FlightTicketCategory  {
                FlightId = "DIN",
                TicketCategoryId = 2,
                Price = 1830000
              },
              new FlightTicketCategory  {
                FlightId = "DIN",
                TicketCategoryId = 3,
                Price = 223000
              }
            }
          },
          new FlightTimeObj { 
            Id = "UIH",
            FlightTime = 70,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "UIH",
                TicketCategoryId = 1,
                Price = 738000
              },
              new FlightTicketCategory  {
                FlightId = "UIH",
                TicketCategoryId = 2,
                Price = 636000
              },
              new FlightTicketCategory  {
                FlightId = "UIH",
                TicketCategoryId = 3,
                Price = 44000
              }
            }
          },
          new FlightTimeObj { 
            Id = "BMV",
            FlightTime = 60,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "BMV",
                TicketCategoryId = 1,
                Price = 738000
              },
              new FlightTicketCategory  {
                FlightId = "BMV",
                TicketCategoryId = 2,
                Price = 636000
              },
              new FlightTicketCategory  {
                FlightId = "BMV",
                TicketCategoryId = 3,
                Price = 44000
              }
            }
          },
          new FlightTimeObj { 
            Id = "VDO",
            FlightTime = 135,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "VDO",
                TicketCategoryId = 1,
                Price = 638000
              },
              new FlightTicketCategory  {
                FlightId = "VDO",
                TicketCategoryId = 2,
                Price = 528000
              },
              new FlightTicketCategory  {
                FlightId = "VDO",
                TicketCategoryId = 3,
                Price = 110000
              }
            }
          }
        };

        // From HAN to ... 
        IList<FlightTimeObj> fromHANList = new List<FlightTimeObj>() {
          new FlightTimeObj {
            Id = "SGN",
            FlightTime = 130,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "SGN",
                TicketCategoryId = 1,
                Price = 1233000
              },
              new FlightTicketCategory  {
                FlightId = "SGN",
                TicketCategoryId = 2,
                Price = 1090000
              },
              new FlightTicketCategory  {
                FlightId = "SGN",
                TicketCategoryId = 3,
                Price = 86000
              }
            }
          },
          new FlightTimeObj { 
            Id = "HAN",
            FlightTime = 0
          },
          new FlightTimeObj { 
            Id = "DAD",
            FlightTime = 80,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "DAD",
                TicketCategoryId = 1,
                Price = 676000
              },
              new FlightTicketCategory  {
                FlightId = "DAD",
                TicketCategoryId = 2,
                Price = 580000
              },
              new FlightTicketCategory  {
                FlightId = "DAD",
                TicketCategoryId = 3,
                Price = 38000
              }
            }
          },
          new FlightTimeObj { 
            Id = "CXR",
            FlightTime = 110,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "CXR",
                TicketCategoryId = 1,
                Price = 917000
              },
              new FlightTicketCategory  {
                FlightId = "CXR",
                TicketCategoryId = 2,
                Price = 805000
              },
              new FlightTicketCategory  {
                FlightId = "CXR",
                TicketCategoryId = 3,
                Price = 55000
              }
            }
          },
          new FlightTimeObj { 
            Id = "HUI",
            FlightTime = 70,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "HUI",
                TicketCategoryId = 1,
                Price = 1122000
              },
              new FlightTicketCategory  {
                FlightId = "HUI",
                TicketCategoryId = 2,
                Price = 983000
              },
              new FlightTicketCategory  {
                FlightId = "HUI",
                TicketCategoryId = 3,
                Price = 83000
              }
            }
          },
          new FlightTimeObj { 
            Id = "PQC",
            FlightTime = 125,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "PQC",
                TicketCategoryId = 1,
                Price = 1412000
              },
              new FlightTicketCategory  {
                FlightId = "PQC",
                TicketCategoryId = 2,
                Price = 1368000
              },
              new FlightTicketCategory  {
                FlightId = "PQC",
                TicketCategoryId = 3,
                Price = 130000
              }
            }
          },
          new FlightTimeObj { 
            Id = "DLI",
            FlightTime = 110,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "DLI",
                TicketCategoryId = 1,
                Price = 1256000
              },
              new FlightTicketCategory  {
                FlightId = "DLI",
                TicketCategoryId = 2,
                Price = 1200000
              },
              new FlightTicketCategory  {
                FlightId = "DLI",
                TicketCategoryId = 3,
                Price = 130000
              }
            }
          },
          new FlightTimeObj { 
            Id = "PXU",
            FlightTime = 95,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "PXU",
                TicketCategoryId = 1,
                Price = 1156000
              },
              new FlightTicketCategory  {
                FlightId = "PXU",
                TicketCategoryId = 2,
                Price = 1070000
              },
              new FlightTicketCategory  {
                FlightId = "PXU",
                TicketCategoryId = 3,
                Price = 126000
              }
            }
          },
          new FlightTimeObj { 
            Id = "VDH",
            FlightTime = 90,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "VDH",
                TicketCategoryId = 1,
                Price = 738000
              },
              new FlightTicketCategory  {
                FlightId = "VDH",
                TicketCategoryId = 2,
                Price = 636000
              },
              new FlightTicketCategory  {
                FlightId = "VDH",
                TicketCategoryId = 3,
                Price = 44000
              }
            }
          },
          new FlightTimeObj { 
            Id = "VCL",
            FlightTime = 90,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "VCL",
                TicketCategoryId = 1,
                Price = 693000
              },
              new FlightTicketCategory  {
                FlightId = "VCL",
                TicketCategoryId = 2,
                Price = 645000
              },
              new FlightTicketCategory  {
                FlightId = "VCL",
                TicketCategoryId = 3,
                Price = 70000
              }
            }
          },
          new FlightTimeObj { 
            Id = "THD",
            FlightTime = 0
          },
          new FlightTimeObj {
            Id = "VCA",
            FlightTime = 130,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "VCA",
                TicketCategoryId = 1,
                Price = 753000
              },
              new FlightTicketCategory  {
                FlightId = "VCA",
                TicketCategoryId = 2,
                Price = 708000
              },
              new FlightTicketCategory  {
                FlightId = "VCA",
                TicketCategoryId = 3,
                Price = 130000
              }
            }
          },
          new FlightTimeObj { 
            Id = "TBB",
            FlightTime = 100,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "TBB",
                TicketCategoryId = 1,
                Price = 582000
              },
              new FlightTicketCategory  {
                FlightId = "TBB",
                TicketCategoryId = 2,
                Price = 545000
              },
              new FlightTicketCategory  {
                FlightId = "TBB",
                TicketCategoryId = 3,
                Price = 130000
              }
            }
          },
          new FlightTimeObj { 
            Id = "VCS",
            FlightTime = 160,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "DLI",
                TicketCategoryId = 1,
                Price = 1730000
              },
              new FlightTicketCategory  {
                FlightId = "DLI",
                TicketCategoryId = 2,
                Price = 1620000
              },
              new FlightTicketCategory  {
                FlightId = "DLI",
                TicketCategoryId = 3,
                Price = 200000
              }
            }
          },
          new FlightTimeObj { 
            Id = "CAH",
            FlightTime = 190,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "CAH",
                TicketCategoryId = 1,
                Price = 1924000
              },
              new FlightTicketCategory  {
                FlightId = "CAH",
                TicketCategoryId = 2,
                Price = 1871000
              },
              new FlightTicketCategory  {
                FlightId = "CAH",
                TicketCategoryId = 3,
                Price = 250000
              }
            }
          },
          new FlightTimeObj { 
            Id = "HPH",
            FlightTime = 0
          },
          new FlightTimeObj { 
            Id = "VKG",
            FlightTime = 140,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "VKG",
                TicketCategoryId = 1,
                Price = 1269000
              },
              new FlightTicketCategory  {
                FlightId = "VKG",
                TicketCategoryId = 2,
                Price = 1190000
              },
              new FlightTicketCategory  {
                FlightId = "VKG",
                TicketCategoryId = 3,
                Price = 123000
              }
            }
          },
          new FlightTimeObj { 
            Id = "VII",
            FlightTime = 55,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "VII",
                TicketCategoryId = 1,
                Price = 518000
              },
              new FlightTicketCategory  {
                FlightId = "VII",
                TicketCategoryId = 2,
                Price = 438000
              },
              new FlightTicketCategory  {
                FlightId = "VII",
                TicketCategoryId = 3,
                Price = 22000
              }
            }
          },
          new FlightTimeObj { 
            Id = "DIN",
            FlightTime = 75,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "DIN",
                TicketCategoryId = 1,
                Price = 1617000
              },
              new FlightTicketCategory  {
                FlightId = "DIN",
                TicketCategoryId = 2,
                Price = 1428000
              },
              new FlightTicketCategory  {
                FlightId = "DIN",
                TicketCategoryId = 3,
                Price = 133000
              }
            }
          },
          new FlightTimeObj { 
            Id = "UIH",
            FlightTime = 95,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "UIH",
                TicketCategoryId = 1,
                Price = 808000
              },
              new FlightTicketCategory  {
                FlightId = "UIH",
                TicketCategoryId = 2,
                Price = 706000
              },
              new FlightTicketCategory  {
                FlightId = "UIH",
                TicketCategoryId = 3,
                Price = 44000
              }
            }
          },
          new FlightTimeObj { 
            Id = "BMV",
            FlightTime = 105,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "BMV",
                TicketCategoryId = 1,
                Price = 917000
              },
              new FlightTicketCategory  {
                FlightId = "BMV",
                TicketCategoryId = 2,
                Price = 805000
              },
              new FlightTicketCategory  {
                FlightId = "BMV",
                TicketCategoryId = 3,
                Price = 55000
              }
            }
          },
          new FlightTimeObj { 
            Id = "VDO",
            FlightTime = 0
          }
        };

        IList<FlightTimeObj> fromDADList = new List<FlightTimeObj>() {
          new FlightTimeObj {
            Id = "SGN",
            FlightTime = 80,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "SGN",
                TicketCategoryId = 1,
                Price = 528000
              },
              new FlightTicketCategory  {
                FlightId = "SGN",
                TicketCategoryId = 2,
                Price = 448000
              },
              new FlightTicketCategory  {
                FlightId = "SGN",
                TicketCategoryId = 3,
                Price = 22000
              }
            }
          },
          new FlightTimeObj {
            Id = "HAN",
            FlightTime = 80,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "HAN",
                TicketCategoryId = 1,
                Price = 528000
              },
              new FlightTicketCategory  {
                FlightId = "HAN",
                TicketCategoryId = 2,
                Price = 448000
              },
              new FlightTicketCategory  {
                FlightId = "HAN",
                TicketCategoryId = 3,
                Price = 22000
              }
            }
          },
          new FlightTimeObj {
            Id = "CXR",
            FlightTime = 60,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "CXR",
                TicketCategoryId = 1,
                Price = 560000
              },
              new FlightTicketCategory  {
                FlightId = "CXR",
                TicketCategoryId = 2,
                Price = 518000
              },
              new FlightTicketCategory  {
                FlightId = "CXR",
                TicketCategoryId = 3,
                Price = 80000
              }
            }
          },
          new FlightTimeObj {
            Id = "DLI",
            FlightTime = 75,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "DLI",
                TicketCategoryId = 1,
                Price = 957000
              },
              new FlightTicketCategory  {
                FlightId = "DLI",
                TicketCategoryId = 2,
                Price = 834000
              },
              new FlightTicketCategory  {
                FlightId = "DLI",
                TicketCategoryId = 3,
                Price = 66000
              }
            }
          },
        };

        IList<FlightTimeObj> fromCXRList = new List<FlightTimeObj>() {
          new FlightTimeObj {
            Id = "SGN",
            FlightTime = 65,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "SGN",
                TicketCategoryId = 1,
                Price = 738000
              },
              new FlightTicketCategory  {
                FlightId = "SGN",
                TicketCategoryId = 2,
                Price = 636000
              },
              new FlightTicketCategory  {
                FlightId = "SGN",
                TicketCategoryId = 3,
                Price = 44000
              }
            }
          },
          new FlightTimeObj {
            Id = "HAN",
            FlightTime = 110,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "HAN",
                TicketCategoryId = 1,
                Price = 442000
              },
              new FlightTicketCategory  {
                FlightId = "HAN",
                TicketCategoryId = 2,
                Price = 402000
              },
              new FlightTicketCategory  {
                FlightId = "HAN",
                TicketCategoryId = 3,
                Price = 130000
              }
            }
          },
          new FlightTimeObj {
            Id = "DAD",
            FlightTime = 65,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "DAD",
                TicketCategoryId = 1,
                Price = 518000
              },
              new FlightTicketCategory  {
                FlightId = "DAD",
                TicketCategoryId = 2,
                Price = 438000
              },
              new FlightTicketCategory  {
                FlightId = "DAD",
                TicketCategoryId = 3,
                Price = 22000
              }
            }
          }
        };

        IList<FlightTimeObj> fromDLIList = new List<FlightTimeObj>() {
          new FlightTimeObj {
            Id = "SGN",
            FlightTime = 55,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "SGN",
                TicketCategoryId = 1,
                Price = 628000
              },
              new FlightTicketCategory  {
                FlightId = "SGN",
                TicketCategoryId = 2,
                Price = 537000
              },
              new FlightTicketCategory  {
                FlightId = "SGN",
                TicketCategoryId = 3,
                Price = 33000
              }
            }
          },
          new FlightTimeObj {
            Id = "HAN",
            FlightTime = 110,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "HAN",
                TicketCategoryId = 1,
                Price = 847000
              },
              new FlightTicketCategory  {
                FlightId = "HAN",
                TicketCategoryId = 2,
                Price = 808000
              },
              new FlightTicketCategory  {
                FlightId = "HAN",
                TicketCategoryId = 3,
                Price = 130000
              }
            }
          },
          new FlightTimeObj {
            Id = "DAD",
            FlightTime = 70,
            FlightTicketCategories = new List<FlightTicketCategory> {
              new FlightTicketCategory  {
                FlightId = "DAD",
                TicketCategoryId = 1,
                Price = 957000
              },
              new FlightTicketCategory  {
                FlightId = "DAD",
                TicketCategoryId = 2,
                Price = 834000
              },
              new FlightTicketCategory  {
                FlightId = "DAD",
                TicketCategoryId = 3,
                Price = 66000
              }
            }
          },
        };
    }
}