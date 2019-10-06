using Soccer_Vision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
            catch(Exception e)
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
                string result = dal.Register(user.Username, user.Email, user.Password);
                if (result == "Registration complete")
                    return Content(HttpStatusCode.Created, result);
                else
                    return BadRequest(result);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

    }
}
