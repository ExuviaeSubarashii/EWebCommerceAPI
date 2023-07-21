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
        public JsonResult GetItems(string itemName)
        {
            return new JsonResult(_CC.Items.Where(x => x.ItemName == itemName).ToList());
        }
        [HttpPost]
        [Route("SaveCartList")]
        public ActionResult SaveCartList(string ordererName, string itemNames, string itemAmounts, string totalPrice)
        {
            // Process the received itemNames, itemAmounts, and totalPrice as needed

            // Example: Split the comma-separated strings into arrays
            string[] itemNameArray = itemNames.Split(',');

            string[] itemCountArray = itemAmounts.Split(',');
            for (int i = 0; i < itemNameArray.Length; i++)
            {
                Orders newOrder = new Orders()
                {
                    ItemName = itemNameArray[i],
                    ItemAmount = itemCountArray[i],
                    OrderDate = DateTime.Now,
                    OrdererName = ordererName,
                    TotalPrice = totalPrice
                };

                var itemStockQuery = _CC.Items.Where(x => x.ItemName == itemNameArray[i]).FirstOrDefault();

                itemStockQuery.ItemStock = itemStockQuery.ItemStock - Convert.ToInt32(itemCountArray[i]);

                _CC.Orders.Add(newOrder);
                _CC.SaveChanges();
            }
            return Ok();
        }

        [HttpPost]
        [Route("AddNewItem")]
        public async Task<ActionResult> AddNewItem(string itemName, int? itemPrice, string itemPriceTag, string? itemImage)
        {
            if (!string.IsNullOrEmpty(itemName) && itemPrice != null && !string.IsNullOrEmpty(itemPriceTag) && !string.IsNullOrEmpty(itemImage))
            {
                Item newItem = new Item()
                {
                    ItemName = itemName.Trim(),
                    ItemImage = itemImage.Trim(),
                    ItemPrice = itemPrice,
                    ItemPriceTag = itemPriceTag.Trim()
                };
                _CC.Items.Add(newItem);
                _CC.SaveChanges();
                return Ok();
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
    }
}
