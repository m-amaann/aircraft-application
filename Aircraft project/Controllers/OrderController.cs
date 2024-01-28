using Aircraft_project.Data;
using Aircraft_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                    PaymentStatus = orderDetails.PaymentStatus,
                    OrderStatus = "Order Received",
                    LastUpdated = DateTime.Now
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

        [HttpPost("UpdateOrderStatus")]
        public IActionResult UpdateOrderStatus([FromForm] int orderId, [FromForm] string newStatus)
        {
            try
            {
                var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
                if (order == null)
                {
                    return NotFound(new { message = "Order not found." });
                }

                order.OrderStatus = newStatus;
                order.LastUpdated = DateTime.Now;
                _context.SaveChanges();

                return Ok(new { message = "Order status updated successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating order status:", ex.Message);
                return BadRequest(new { error = "Failed to update the order status." });
            }
        }

        [HttpGet("GetOrderDetails")]
        public IActionResult GetOrderDetails(int orderId)
        {
            var order = _context.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return NotFound(new { message = "Order not found." });
            }

            return Ok(order);
        }
    }
}