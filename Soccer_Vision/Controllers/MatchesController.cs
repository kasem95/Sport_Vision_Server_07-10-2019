using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Soccer_Vision.Models;

namespace Soccer_Vision.Controllers
{
    [RoutePrefix("api/Matches")]
    public class MatchesController : ApiController
    {
        public IHttpActionResult PostCreateMatch([FromBody]Matchs match)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.CreateMatch(match.UserID, match.MatchName, match.MatchDate, match.MatchTime, match.FieldID, match.CityID, match.IsPrivate, match.MatchKey, match.MaxPlayers);
                if (result == "Match successfully created")
                    return Content(HttpStatusCode.Created, result);
                else
                    return Content(HttpStatusCode.BadRequest, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
            

        }

        public IHttpActionResult PutJoinMatch(int userID,int matchID)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.JoinMatch(userID, matchID);
                if (result == "You have joiined the match")
                    return Content(HttpStatusCode.OK, result);
                else
                    return Content(HttpStatusCode.BadRequest, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }
    }
}
