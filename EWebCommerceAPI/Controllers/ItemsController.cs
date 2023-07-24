using Commerce.Domain.Dtos;
using Commerce.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            if (resultQuery.Count > 0) 
            {
                return new JsonResult(resultQuery);
            }
            else if (itemName==null)
            {
                return new JsonResult(_CC.Items.ToList());
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

                for (int i = 0; i < itemNameArray.Length; i++)
                {
                    Orders newOrder = new Orders()
                    {
                        ItemName = itemNameArray[i],
                        ItemAmount = itemCountArray[i],
                        OrderDate = DateTime.Now,
                        OrdererName = orderRequest.OrdererName,
                        TotalPrice = orderRequest.TotalPrice.ToString()
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
        public async Task<ActionResult> AddNewItem(string userName,string itemName, int? itemPrice, string itemPriceTag, string? itemImage)
        {
            if (!string.IsNullOrEmpty(itemName) && itemPrice != null && !string.IsNullOrEmpty(itemPriceTag) && !string.IsNullOrEmpty(itemImage))
            {
                Item newItem = new Item()
                {
                    ItemName = itemName.Trim(),
                    ItemPrice = itemPrice,
                    ItemImage = itemImage.Trim(),
                    ItemPriceTag = itemPriceTag.Trim(),
                    SellerUser = userName.Trim()
                };
                _CC.Items.Add(newItem);
                _CC.SaveChanges();
                return new JsonResult(_CC.Items.Where(x=>x.SellerUser==userName.Trim()));
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
        [Route("GetMyListings")]
        public JsonResult GetMyListings(string userName)
        {
            return new JsonResult(_CC.Items.Where(x => x.SellerUser == userName.Trim()).ToList());

        }
    }
}
