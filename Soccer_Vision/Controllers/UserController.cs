using Soccer_Vision.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Soccer_Vision.Controllers
{
    [RoutePrefix("api/Users")]
    public class UserController : ApiController
    {

        [Route("{email},{pass}/login")]
        public IHttpActionResult GetUserLoginID(string email, string pass)
        {
            try
            {
                DAL dal = new DAL();
                User user = dal.Login(email, pass);
                if (user == null)
                    return Content(HttpStatusCode.NotFound, "User does not exist");
                else
                    return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }

        public IHttpActionResult PostRegister([FromBody]User user)
        {
            try
            {
                DAL dal = new DAL();
                int result = dal.Register(user.Username, user.Email, user.Password);
                if (result > 0)
                    return Content(HttpStatusCode.Created, result);
                else if (result == -2)
                    return BadRequest("Invalid email and password");
                else if (result == -3)
                    return BadRequest("Invalid email");
                else if (result == -4)
                    return BadRequest("Invalid password");
                else if (result == -5)
                    return Ok("User already exists");
                else
                    return BadRequest("Something went wrong");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [Route("getUsers")]
        public IHttpActionResult GetUsers()
        {
            try
            {
                DAL dal = new DAL();
                DataTable users = dal.getUsers();
                if (users == null)
                    return Content(HttpStatusCode.NotFound, "User does not exist");
                else
                    return Ok(users);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }

        [Route("ChangeUsername")]
        public IHttpActionResult PutChangeUsername([FromBody]User value)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.changeUsername(value.UserID, value.Username);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("ChangePassword")]
        public IHttpActionResult PutChangePassword([FromBody]User value)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.changePassword(value.UserID, value.Password);
                return Content(HttpStatusCode.OK, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("{userID}/getUser")]
        public IHttpActionResult GetUserAfterChanges(int userID)
        {
            try
            {
                DAL dal = new DAL();
                User user = dal.getUserAfterChanges(userID);
                if (user == null)
                    return Content(HttpStatusCode.NotFound, "User does not exist");
                else
                    return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }

    }
}
