using Commerce.Domain.Dtos;
using Commerce.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace EWebCommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly CommerceContext _CC;
       
        public ItemsController(CommerceContext CC)
        {
            _CC = CC;
        }
        [HttpGet]
        [Route("GetAllItems")]
        public JsonResult GetAllItems()
        {
            return new JsonResult(_CC.Items.ToList());
        }
        [HttpGet]
        [Route("GetSpecificItem")]
        public JsonResult GetItems(string? itemName)
        {
            var resultQuery = _CC.Items.Where(x => x.ItemName == itemName).ToList();
            if (itemName == null)
            {
                return new JsonResult(_CC.Items.ToList());
            }
            if (resultQuery.Count > 0) 
            {
                return new JsonResult(resultQuery);
            }
            else { return new JsonResult(null); }
        }
        [HttpPost]
        [Route("SaveCartList")]
        public IActionResult SaveCartList([FromBody] OrderRequest orderRequest)
            {
            var CheckIfCardExists = _CC.PaymentInformations.Select(x =>
                x.CardNumber == orderRequest.CardNumber &&
                x.ExpirationDate == orderRequest.ExpirationDate &&
                x.SecurityCode == orderRequest.SecurityCode &&
                x.PostalCode == orderRequest.PostalCode).FirstOrDefault();

            if (CheckIfCardExists)
            {
                string[] itemNameArray = orderRequest.ItemNames.Split(',');
                string[] itemCountArray = orderRequest.ItemAmounts.Split(',');
                string[] itemPriceArray=orderRequest.TotalPrice.Split(",");

                for (int i = 0; i < itemNameArray.Length; i++)
                {
                    Orders newOrder = new Orders()
                    {
                        ItemName = itemNameArray[i],
                        ItemAmount = itemCountArray[i],
                        OrderDate = DateTime.Now,
                        OrdererName = orderRequest.OrdererName,
                        TotalPrice = itemPriceArray[i],
                        OrderGuid = Guid.NewGuid().ToString(),
                    };

                    var itemStockQuery = _CC.Items.FirstOrDefault(x => x.ItemName == itemNameArray[i]);

                    itemStockQuery.ItemStock = itemStockQuery.ItemStock - Convert.ToInt32(itemCountArray[i]);

                    _CC.Orders.Add(newOrder);
                    _CC.SaveChanges();
                }

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        
        [HttpPost]
        [Route("AddNewItem")]
        public async Task<ActionResult> AddNewItem([FromForm] NewItemRequest NIT)
        {
            
            if (!string.IsNullOrEmpty(NIT.itemName) && NIT.itemPrice != null)
            {
                
                Item newItem = new Item()
                {
                    ItemName = NIT.itemName.Trim(),
                    ItemPrice = NIT.itemPrice,
                    //install Microsoft.AspNetCore.Http.Features check NewItemRequest class for more info
                    //ItemImage = NIT.itemImage,
                    ItemStock = NIT.itemStock,
                    ItemPriceTag="TRY",
                    SellerUser = NIT.userName.Trim()
                };
                _CC.Items.Add(newItem);
                _CC.SaveChanges();
                return new JsonResult(_CC.Items.Where(x=>x.SellerUser==NIT.userName.Trim()));
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("GetMyOrders")]
        public JsonResult GetMyOrders(string userName)
        {
            return new JsonResult(_CC.Orders.Where(x => x.OrdererName == userName).ToList());
        }
        [HttpGet]
        [Route("GetMyOrdersByGuid")]
        public JsonResult GetMyOrdersByGuid(string userName,string itemGuid)
        {
            return new JsonResult(_CC.Orders.Where(x => x.OrdererName == userName&&x.OrderGuid==itemGuid).ToList());
        }
        [HttpGet]
        [Route("GetMyListings")]
        public JsonResult GetMyListings(string userName)
        {
            return new JsonResult(_CC.Items.Where(x => x.SellerUser == userName.Trim()).ToList());

        }
    }
}
