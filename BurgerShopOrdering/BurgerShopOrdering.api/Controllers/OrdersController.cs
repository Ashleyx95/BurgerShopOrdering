using BurgerShopOrdering.api.Dtos.Common;
using BurgerShopOrdering.api.Dtos.Orders;
using BurgerShopOrdering.core.Entities;
using BurgerShopOrdering.core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BurgerShopOrdering.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderService<Order> orderService, ICrudService<OrderItem> orderItemService, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        private readonly IOrderService<Order> _orderService = orderService;
        private readonly ICrudService<OrderItem> _orderItemService = orderItemService;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            IEnumerable<Order> orders;

            if (User.IsInRole("Admin"))
            {
                var result = await _orderService.GetAllAsync();

                if (!result.Success || result.Data == null)
                {
                    return BadRequest(ApiResponse<object>.FailureResponse("Bestellingen konden niet worden opgehaald.", result.Errors));
                }

                orders = result.Data;
            }
            else
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<object>.FailureResponse("Gebruikers id kon niet worden bepaald."));
                }

                var result = await _orderService.GetOrdersByUserAsync(userId);

                if (!result.Success || result.Data == null)
                {
                    return BadRequest(ApiResponse<object>.FailureResponse("Bestellingen konden niet worden opgehaald.", result.Errors));
                }

                orders = result.Data;
            }

            var orderResponseDtos = orders.Select(order => new OrderResponseDto
            {
                Id = order.Id,
                Name = order.Name,
                TotalPrice = order.TotalPrice,
                TotalQuantity = order.Quantity,
                NameUser = $"{order.ApplicationUser.FirstName} {order.ApplicationUser.LastName}",
                ApplicationUserId = order.ApplicationUserId,
                Status = order.Status.ToString(),
                DateOrdered = order.DateOrdered,
                DateDelivered = order.DateDelivered,
                OrderItems = order.OrderItems.Select(oi => new OrderItemResponseDto
                {
                    Id = oi.Id,
                    ProductName = oi.Product.Name,
                    Price = oi.Price,
                    Quantity = oi.Quantity,
                }).ToList()
            }).ToList();

            return Ok(ApiResponse<List<OrderResponseDto>>.SuccessResponse(orderResponseDtos, "Bestellingen opgehaald."));
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _orderService.GetByIdAsync(id);

            if (!result.Success || result.Data == null)
            {
                return NotFound(ApiResponse<object>.FailureResponse("Bestelling niet gevonden.", result.Errors));
            }

            var order = result.Data;

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!User.IsInRole("Admin") && order.ApplicationUserId != userId)
            {
                return Forbid();
            }

            var orderResponseDto = new OrderResponseDto
            {
                Id = order.Id,
                Name = order.Name,
                TotalPrice = order.TotalPrice,
                TotalQuantity = order.Quantity,
                ApplicationUserId = order.ApplicationUserId,
                NameUser = order.ApplicationUser.FirstName + " " + order.ApplicationUser.LastName,
                Status = order.Status.ToString(),
                DateOrdered = order.DateOrdered,
                DateDelivered = order.DateDelivered,
                OrderItems = order.OrderItems.Select(oi => new OrderItemResponseDto
                {
                    Id = oi.Id,
                    ProductName = oi.Product.Name,
                    Price = oi.Price,
                    Quantity = oi.Quantity,
                }).ToList()
            };

            return Ok(ApiResponse<OrderResponseDto>.SuccessResponse(orderResponseDto, "Bestelling werd opgehaald."));
        }
        [HttpGet("{status}")]
        [Authorize]
        public async Task<IActionResult> Get(OrderStatus status)
        {
            IEnumerable<Order> orders;

            if (User.IsInRole("Admin"))
            {
                var result = await _orderService.GetByStatusAsync(status);

                if (!result.Success || result.Data == null)
                {
                    return BadRequest(ApiResponse<object>.FailureResponse("Bestellingen konden niet worden opgehaald.", result.Errors));
                }

                orders = result.Data;
            }
            else
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ApiResponse<object>.FailureResponse("Gebruikers id kon niet worden bepaald."));
                }

                var result = await _orderService.GetOrdersByUserAndStatusAsync(userId, status);

                if (!result.Success || result.Data == null)
                {
                    return BadRequest(ApiResponse<object>.FailureResponse("Bestellingen konden niet worden opgehaald.", result.Errors));
                }

                orders = result.Data;
            }

            var orderResponseDtos = orders.Select(order => new OrderResponseDto
            {
                Id = order.Id,
                Name = order.Name,
                TotalPrice = order.TotalPrice,
                TotalQuantity = order.Quantity,
                NameUser = $"{order.ApplicationUser.FirstName} {order.ApplicationUser.LastName}",
                ApplicationUserId = order.ApplicationUserId,
                Status = order.Status.ToString(),
                DateOrdered = order.DateOrdered,
                DateDelivered = order.DateDelivered,
                OrderItems = order.OrderItems.Select(oi => new OrderItemResponseDto
                {
                    Id = oi.Id,
                    ProductName = oi.Product.Name,
                    Price = oi.Price,
                    Quantity = oi.Quantity,
                }).ToList()
            }).ToList();

            return Ok(ApiResponse<List<OrderResponseDto>>.SuccessResponse(orderResponseDtos, "Bestellingen opgehaald."));
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Add(OrderCreateRequestDto orderCreateRequestDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<object>.FailureResponse("Ongeldige invoer.", errors));
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(ApiResponse<object>.FailureResponse("Gebruikers id kon niet worden bepaald."));

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Unauthorized(ApiResponse<object>.FailureResponse("Gebruiker niet gevonden."));

            string orderName = $"Bestelling {user.FirstName} {user.LastName} - {DateTime.Now:dd/MM/yy HH:mm}";

            var order = new Order(userId, orderName, orderCreateRequestDto.TotalPrice, orderCreateRequestDto.TotalQuantity);

            foreach (var item in orderCreateRequestDto.OrderItems)
            {
                var orderItem = new OrderItem(order.Id, item.ProductId, item.Quantity, item.Price);
                order.OrderItems.Add(orderItem);
            }

            var result = await _orderService.AddAsync(order);

            if (result.Success)
            {
                return CreatedAtAction(nameof(Get), new { id = order.Id }, ApiResponse<object>.SuccessResponse(null, "Bestelling is toegevoegd."));
            }

            return BadRequest(ApiResponse<object>.FailureResponse("Bestelling kon niet worden toegevoegd.", result.Errors));
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(OrderUpdateRequestDto orderUpdateRequestDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<object>.FailureResponse("Ongeldige invoer.", errors));
            }

            var orderResult = await _orderService.GetByIdAsync(orderUpdateRequestDto.Id);

            if (!orderResult.Success || orderResult.Data == null)
            {
                return NotFound(ApiResponse<object>.FailureResponse("Bestelling werd niet gevonden.", orderResult.Errors));
            }

            var order = orderResult.Data;

            order.Status = orderUpdateRequestDto.Status;

            if (orderUpdateRequestDto.Status == OrderStatus.Afgehaald)
            {
                order.DateDelivered = DateTime.Now;
            }

            var result = await _orderService.UpdateAsync(order);

            if (result.Success)
            {
                return Ok(ApiResponse<object>.SuccessResponse(null, "Bestelling werd geüpdatet."));
            }

            return BadRequest(ApiResponse<object>.FailureResponse("Updaten van bestelling is niet gelukt.", result.Errors));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var orderResult = await _orderService.GetByIdAsync(id);

            if (!orderResult.Success || orderResult.Data == null)
            {
                return BadRequest(ApiResponse<object>.FailureResponse("Bestelling niet gevonden.", orderResult.Errors));
            }

            var order = orderResult.Data;

            foreach (var oi in order.OrderItems)
            {
                await _orderItemService.DeleteAsync(oi);
            }

            var result = await _orderService.DeleteAsync(order);

            if (result.Success)
            {
                return Ok(ApiResponse<object>.SuccessResponse(null, "Bestelling is verwijderd."));
            }

            return BadRequest(ApiResponse<object>.FailureResponse("Verwijderen van bestelling is niet gelukt.", result.Errors));
        }
    }
}
