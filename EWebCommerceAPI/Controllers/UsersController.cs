using Commerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace EWebCommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CommerceContext _CC;
        public UsersController(CommerceContext CC)
        {
            _CC = CC;
        }
        [HttpPost]
        [Route("Login")]
        public ActionResult Login(string userEmail, string userPassword)
        {
            var userCheckInQuery = _CC.Users.Where(x => x.UserEmail == userEmail.Trim() && x.Password.Trim() == userPassword).FirstOrDefault();
            if (userCheckInQuery != null) { return Ok(); }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        [Route("Register")]
        public ActionResult Register(string? userEmail, string? userName, string? userPassword)
        {
            var userCheckInQuery = _CC.Users.Where(x => x.UserEmail == userEmail.Trim() && x.Password.Trim() == userPassword && x.UserName == userName.Trim()).FirstOrDefault();
            if (userCheckInQuery == null)
            {
                User user = new User()
                {
                    UserName = userName,
                    UserEmail = userEmail,
                    Password = userPassword,
                    CreationDate = DateTime.Now,
                };
                _CC.Users.Add(user);
                _CC.SaveChanges();
                return Ok();
            }
            else { return BadRequest(); }
        }
        [HttpPost]
        [Route("AddToWishList")]
        public ActionResult AddToWishList(string? userName, int? wishId)
        {
            if (wishId == null) { return BadRequest(); }

            var UserWishList = _CC.Users.Where(x => x.UserName == userName).FirstOrDefault();
            if (UserWishList.WishList.Length < 0)
            {
                UserWishList.WishList += wishId;
            }
            else if (UserWishList.WishList.Contains(wishId.ToString()))
            {
                string[] acceptedfriendRequest = null;
                List<string> sw = new List<string>();

                var query = _CC.Users.Where(x => x.UserName.Trim() == userName).FirstOrDefault();
                var query2 = _CC.Users.Where(x => x.UserName == userName).ToList();
                foreach (var item in query2)
                {
                    acceptedfriendRequest = item.WishList.Split(',');
                }

                List<String> list = acceptedfriendRequest.ToList();
                list.Remove(wishId.ToString());
                string[] columns = list.ToArray();
                var newfriendList = string.Join(",", columns);
                query.WishList = newfriendList;
                _CC.SaveChanges();
                return Ok();
            }
            UserWishList.WishList = wishId + "," + UserWishList.WishList;

            _CC.SaveChanges();
            return Ok();
        }
        [HttpGet]
        [Route("GetMyWishList")]
        public ActionResult GetMyWishList(string? userName)
        {
            List<Item> items = new List<Item>();

            var getUserWishes = _CC.Users.Where(x => x.UserName == userName).ToList();
            string[] wishArray;
            foreach (var item in getUserWishes)
            {
                wishArray = item.WishList?.Split(',');

                for (int i = 0; i < wishArray.Length; i++)
                {
                    var fullwishes = _CC.Items.Where(x => x.ItemId == Convert.ToInt32(wishArray[i])).ToList();
                    items.AddRange(fullwishes);
                }
            }
            return new JsonResult(items.ToList());
        }
        [HttpGet]
        [Route("DeleteFromWishlist")]
        public ActionResult Delete(int? wishId) { return Ok(); }
    }
}
