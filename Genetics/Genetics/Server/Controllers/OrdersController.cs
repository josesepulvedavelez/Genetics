using Genetics.Server.Dal;
using Genetics.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Genetics.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AnimalContext _context;
        public OrdersController(AnimalContext animalContext) 
        { 
            _context = animalContext;
        }

        [HttpGet("GetAnimal")]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimal()
        {
            if (_context.Animal == null)
            {
                return NotFound();
            }

            var animals = await _context.Animal.OrderBy(a => a.Breed).ToListAsync();
            return animals;
        }

        [HttpGet("GetOrder")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            if (_context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.ToListAsync();
            return order;
        }

        [HttpGet("GetOrder/{OrderId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder(int orderId)
        {
            if (_context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.Where(x => x.OrderId == orderId).ToListAsync();
            return order;
        }

        [HttpGet("GetOrderDetail/{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetail(int orderId)
        {
            if (_context.OrderDetail == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail.Where(x => x.OrderId == orderId).ToListAsync();
            return orderDetail;
        }

        [HttpPost("AddOrder")]
        public async Task<ActionResult> AddOrder([FromBody] OrderModel orderModel)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Order order = new Order
                    {
                        Date = DateTime.Now,
                        TotalDiscount = 0,
                        TotalPurchase = 0
                    };

                    _context.Order.Add(order);
                    await _context.SaveChangesAsync();

                    int lastId = order.OrderId;
                    int totalAmount = 0;

                    foreach (var item in orderModel.lstOrderDetail)
                    {
                        Animal animal = await _context.Animal.FindAsync(item.AnimalId);

                        if (animal != null)
                        {
                            decimal subtotal = item.Amount * animal.Price; // Calcular el subtotal usando el precio del Animal

                            // Aplicar descuento del 5% si Amount es mayor a 5 y menor 10
                            if (item.Amount > 5 && item.Amount < 10)
                            {
                                decimal discount = subtotal * 0.05m;
                                subtotal -= discount;
                            }

                            OrderDetail orderDetail = new OrderDetail
                            {
                                AnimalId = item.AnimalId,
                                OrderId = lastId,
                                Amount = item.Amount,
                                Discount = item.Discount,
                                Freight = item.Freight,
                                Subtotal = subtotal
                            };

                            _context.OrderDetail.Add(orderDetail);
                            await _context.SaveChangesAsync();

                            totalAmount += item.Amount;
                        }
                    }

                    decimal totalDiscount = 0;

                    // Aplicar descuento adicional del 3% si el totalAmount es mayor a 10
                    if (totalAmount > 10)
                    {
                        decimal additionalDiscount = order.TotalPurchase * 0.03m;
                        totalDiscount += additionalDiscount;
                    }

                    // Establecer Freight según el totalAmount
                    decimal freight = totalAmount > 20 ? 0 : 1000;

                    // Actualizar los campos en la entidad Order
                    order.TotalDiscount = totalDiscount;
                    order.TotalPurchase = order.TotalPurchase - totalDiscount + freight;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Ok();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine("Error al guardar la transacción: " + ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
        }

    }
}
