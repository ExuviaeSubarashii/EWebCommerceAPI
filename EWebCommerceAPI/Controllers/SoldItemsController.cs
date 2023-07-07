using Commerce.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EWebCommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoldItemsController : ControllerBase
    {
        private readonly CommerceContext _CC;
        public SoldItemsController(CommerceContext CC)
        {
            _CC = CC;
        }
        [HttpPost]
        public ActionResult AddSoldItem(string buyerName, string soldItemsList, int? fullPrice, string priceTag)
        {
            if (!string.IsNullOrEmpty(buyerName) && fullPrice != null && !string.IsNullOrEmpty(soldItemsList) && !string.IsNullOrEmpty(priceTag))
            {
                SoldItem soldItem = new SoldItem()
                {
                    BuyerName = buyerName,
                    SoldItems = soldItemsList,
                    PriceTag = priceTag,
                    FullPrice = fullPrice
                };
                _CC.SoldItems.Add(soldItem);
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
