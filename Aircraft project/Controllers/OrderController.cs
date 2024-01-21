using Aircraft_project.Data;
using Aircraft_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aircraft_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/order/testajax
        [HttpGet("TestAjax")]
        public IActionResult TestAjax()
        {
            // Your action logic
            return Ok("TestAjax endpoint reached successfully.");
        }

        [HttpPost("PlaceOrder")]
        public IActionResult PlaceOrder([FromBody] Orders orderDetails)
        {
            try
            {
                Console.WriteLine("Received order details:", orderDetails);

                // Create an order
                var order = new Orders
                {
                    UserId = orderDetails.UserId,
                    Name = orderDetails.Name,
                    Email = orderDetails.Email,
                    Phone = orderDetails.Phone,
                    Pincode = orderDetails.Pincode,
                    Address = orderDetails.Address,
                    TotalPrice = orderDetails.TotalPrice,
                    CreatedDate = DateTime.Now,
                    PaymentStatus = orderDetails.PaymentStatus
                };

                Console.WriteLine("Created order:", order);

                // Add order to the database
                _context.Orders.Add(order);
                _context.SaveChanges();

                Console.WriteLine("Order saved to the database");

                // Create order items
                foreach (var item in orderDetails.OrderItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.OrderId,
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Quantity = item.Quantity
                    };

                    // Add order item to the database
                    _context.OrderItems.Add(orderItem);
                }

                Console.WriteLine("Order items saved to the database");

                _context.SaveChanges();

                return Ok(new { orderId = order.OrderId });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error placing order:", ex.Message);

                // Handle exceptions appropriately
                return BadRequest(new { error = "Failed to place the order." });
            }
        }

        // Add this method in your ClientController
        // public IActionResult UserProfile()
        // {
        //     try
        //     {
        //         // Assuming you have a method to get the user's orders based on UserId
        //         var orders = _context.Orders.Where(o => o.UserId == 1).ToList(); // Change UserId as needed

        //         // Pass the orders to the view
        //         return View("userProfile", orders);
        //     }
        //     catch (Exception ex)
        //     {
        //         // Handle exceptions appropriately
        //         Console.Error.WriteLine($"Exception in UserProfile: {ex.Message}");
        //         return StatusCode(500, "Internal Server Error");
        //     }
        // }

    }
}
