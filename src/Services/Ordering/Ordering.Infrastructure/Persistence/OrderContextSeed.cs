using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
            }
        }
        
        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order> {
                new Order(){
                    UserName = "swn",
                    FirstName = "Mehmet",
                    LastName = "Ozkaya",
                    EmailAddress = "ezozkme@gmail.com",
                    AddressLine = "Bahcelievler", 
                    Country = "Turkey", 
                    TotalPrice = 350,
                    CVV = "5589",
                    CardName = "Master Card",
                    CardNumber = "5424 1801 2345 6789",
                    Expiration = "12/30/2025",
                    PaymentMethod = 3,
                    LastModifiedBy ="",
                    CreatedBy = "",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    State = "Tamil Nadu",
                    ZipCode = "600028"
                }
            };
        }
    }
}
