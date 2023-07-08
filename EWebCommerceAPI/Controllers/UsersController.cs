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
        [Route("Login")]
        public ActionResult Login(string userEmail,string userPassword)
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
        public ActionResult Register(string? userEmail, string? userName,string? userPassword) 
        {
            var userCheckInQuery = _CC.Users.Where(x => x.UserEmail == userEmail.Trim() && x.Password.Trim() == userPassword && x.UserName==userName.Trim()).FirstOrDefault();
            if (userCheckInQuery==null)
            {
                User user= new User()
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
    }
}
