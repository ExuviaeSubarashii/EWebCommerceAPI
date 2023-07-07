using Commerce.Domain.Models;
using Microsoft.AspNetCore.Http;
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
        public ActionResult Login(string? userEmail,string? password)
        {
            var userCheckInQuery=_CC.Users.Where(x=>x.UserEmail == userEmail.Trim()&&x.Password.Trim()==password).FirstOrDefault();
            if (userCheckInQuery != null) { return Ok(); }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public ActionResult Register(string? userEmail, string userName,string? password) 
        {
            var userCheckInQuery = _CC.Users.Where(x => x.UserEmail == userEmail.Trim() && x.Password.Trim() == password&&x.UserName==userName.Trim()).FirstOrDefault();
            if (userCheckInQuery==null)
            {
                User user= new User()
                {
                    UserName = userName,
                    UserEmail = userEmail,
                    Password = password,
                    CreationDate = DateTime.Now,
                };  
                _CC.Users.Add(user);
                _CC.SaveChanges();
                return Ok();
            }
            else { return BadRequest(); }
        }
    }
}
