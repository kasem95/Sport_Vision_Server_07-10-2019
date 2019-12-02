using Soccer_Vision.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Soccer_Vision.Controllers
{
    [RoutePrefix("api/Friends")]
    public class FriendsController : ApiController
    {
        [Route("{userID}/getFriendsList")]
        public IHttpActionResult GetFriendsList(int userID)
        {
            try
            {
                DAL dal = new DAL();
                DataTable friends = dal.FriendsList(userID);
                if (friends != null)
                    return Content(HttpStatusCode.OK, friends);
                else
                    return Content(HttpStatusCode.OK, "You don't have friends :(");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }

        [Route("getFriendsTable")]
        public IHttpActionResult GetFriendsTable()
        {
            try
            {
                DAL dal = new DAL();
                DataTable friends = dal.getFriendsTable();
                if (friends != null)
                    return Content(HttpStatusCode.OK, friends);
                else
                    return Content(HttpStatusCode.OK, "You don't have friends :(");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }

        [Route("{userID}/getFriendsRespondsList")]
        public IHttpActionResult GetFriendsRespondsList(int userID)
        {
            try
            {
                DAL dal = new DAL();
                DataTable friends = dal.FriendsRespondsList(userID);
                if (friends != null)
                    return Content(HttpStatusCode.OK, friends);
                else
                    return Content(HttpStatusCode.OK, "You don't have Friends Requests To Respond :(");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }

        [Route("{userID}/getFriendsRequestsTable")]
        public IHttpActionResult GetFriendsRequestsTable(int userID)
        {
            try
            {
                DAL dal = new DAL();
                DataTable users = dal.FriendsRequestsList(userID);
                if (users != null)
                    return Content(HttpStatusCode.OK, users);
                else
                    return Content(HttpStatusCode.OK, "You haven't send any friend request!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }

        [Route("{userID}/getUsersList")]
        public IHttpActionResult GetUsersList(int userID)
        {
            try
            {
                DAL dal = new DAL();
                DataTable users = dal.getUsersSortedAscending(userID);
                if (users != null)
                    return Content(HttpStatusCode.OK, users);
                else
                    return Content(HttpStatusCode.OK, "No Users!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }
    }
}
