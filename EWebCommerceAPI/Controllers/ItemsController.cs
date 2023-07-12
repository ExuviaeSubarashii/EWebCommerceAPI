using Commerce.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
           return new JsonResult(_CC.Items.Where(x=>x.ItemName==itemName).ToList());
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
    }
}
