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

        [HttpGet("GetOrderDetail/{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetail(int orderId)
        {
            if (_context.OrderDetail == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail.ToListAsync();
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

                    foreach (var item in orderModel.OrderDetail)
                    {
                        item.OrderId = order.OrderId;                        
                        item.AnimalId = item.AnimalId;

                        /* -- Pendiente realizar los calculos -- */

                        //item.Amount = item.Amount;
                        //item.Freight = item.Freight;
                        //item.Subtotal = item.Subtotal;
                        
                        _context.OrderDetail.Add(item);
                        await _context.SaveChangesAsync();
                    }

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
