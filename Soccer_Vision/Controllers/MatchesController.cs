using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
                int result = dal.CreateMatch(match.UserID, match.MatchName, match.MatchDate, match.MatchTime, match.FieldID, match.CityID, match.IsPrivate, match.MatchKey, match.MaxPlayers, match.PlayTime);
                return Content(HttpStatusCode.Created, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
            

        }

        [Route("CreateMatchWithGroup")]
        public IHttpActionResult PostCreateMatchWithGroup([FromBody]MatchWithGroup match)
        {
            try
            {
                DAL dal = new DAL();
                int result = dal.createMatchWithGroup(match.UserID, match.MatchName, match.MatchDate, match.MatchTime, match.FieldID, match.CityID, match.IsPrivate, match.MatchKey, match.MaxPlayers, match.PlayTime, match.UsersInGroup);
                return Content(HttpStatusCode.Created, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }


        }

        [Route("RequestJoinMatch")]
        public IHttpActionResult PostRequestJoinMatch([FromBody]RequestJoinMatchClass value)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.RequestJoinMatch(value.userID, value.matchID, value.matchDate, value.matchTime, value.playTime, value.cityID, value.fieldID, value.maxPlayer);
                if (result == "You have requested to join the match")
                    return Content(HttpStatusCode.OK, result);
                else
                    return Content(HttpStatusCode.BadRequest, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("ExitMatch")]
        public IHttpActionResult PutExitMatch([FromBody]DetailsForExitMatch value)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.ExitMatch(value.UserID, value.MatchID, value.AdminID, value.MatchDate, value.PlayersJoined);
                if (result == "Done!")
                    return Content(HttpStatusCode.OK, result);
                else
                    return Content(HttpStatusCode.BadRequest, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("CancelMatch")]
        public IHttpActionResult DeleteMatch([FromBody]Matchs value)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.CancelMatch(value.MatchID, value.MatchDate);
                if (result == "Done!")
                    return Content(HttpStatusCode.OK, result);
                else
                    return Content(HttpStatusCode.BadRequest, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("CancelRequest")]
        public IHttpActionResult DeleteRequest([FromBody]Matchs value)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.CancelRequest(value.UserID, value.MatchID);
                if (result == "Done!")
                    return Content(HttpStatusCode.OK, result);
                else
                    return Content(HttpStatusCode.BadRequest, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("CancelInvite")]
        public IHttpActionResult DeleteInvite([FromBody]Matchs value)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.CancelInvite(value.UserID, value.MatchID);
                if (result == "Done!")
                    return Content(HttpStatusCode.OK, result);
                else
                    return Content(HttpStatusCode.BadRequest, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("acceptRequestJoinMatch")]
        public IHttpActionResult PutAcceptRequestJoinMatch([FromBody]RequestJoinMatchClass value)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.AcceptRequestJoinMatch(value.userID, value.matchID, value.matchDate, value.matchTime, value.playTime, value.cityID, value.fieldID, value.maxPlayer, value.IsInvite);
                if (result == "Done!")
                    return Content(HttpStatusCode.OK, result);
                else
                    return Content(HttpStatusCode.BadRequest, result);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("InviteFriendToMatch")]
        public IHttpActionResult PostInviteFriendToMatch([FromBody]RequestJoinMatchClass value)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.inviteFriendToMatch(value.matchID, value.userID, value.matchDate, value.matchTime, value.playTime, value.cityID, value.fieldID, value.maxPlayer);
                if (result == "You have invited him to join your match!")
                    return Content(HttpStatusCode.OK, result);
                else
                    return Content(HttpStatusCode.BadRequest, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("{matchID}/getMatch")]
        public IHttpActionResult GetMatch(int matchID)
        {
            try
            {
                DAL dal = new DAL();
                Matchs match = dal.getMatchDetails(matchID);
                return Content(HttpStatusCode.OK, match);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("getActiveMatches")]
        public IHttpActionResult GetActiveMatches()
        {
            try
            {
                DAL dal = new DAL();
                DataTable matches = dal.ActiveMatchesTable();
                return Content(HttpStatusCode.OK, matches);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("getCitiesAndFields")]
        public IHttpActionResult GetCitiesFields()
        {
            try
            {
                DAL DAL = new DAL();
                var result = DAL.getCitiesAndFieldsTables();
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest("Something went wrong");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [Route("getUsersInMatchTable")]
        public IHttpActionResult GetUsersInMatchTable()
        {
            try
            {
                DAL DAL = new DAL();
                var result = DAL.getUsersInMatchList();
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest("No users In the Match");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [Route("getUsersInvitedToMatchTable")]
        public IHttpActionResult GetUsersInvitedToMatchTable()
        {
            try
            {
                DAL DAL = new DAL();
                var result = DAL.getUsersInvitedToMatchTable();
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest("No users In the Match");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [Route("uploadMatchpicture")]
        public Task<HttpResponseMessage> Post()
        {
            string fileNameForSQL = null;

            MultipartFormDataContent form = new MultipartFormDataContent();
            string outputForNir = "start---";
            List<string> savedFilePath = new List<string>();
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string rootPath = HttpContext.Current.Server.MapPath("~/uploadFiles");
            var provider = new MultipartFormDataStreamProvider(rootPath);
            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(t =>
                {
                    if (t.IsCanceled || t.IsFaulted)
                    {
                        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                    }
                    int counter = 0;
                    foreach (MultipartFileData item in provider.FileData)
                    {
                        if (counter == 0)
                        {
                            try
                            {
                                outputForNir += " ---here";
                                string name = item.Headers.ContentDisposition.FileName.Replace("\"", "");
                                outputForNir += " ---here2=" + name;

                                //need the guid because in react native in order to refresh an inamge it has to have a new name
                                string newFileName = Path.GetFileNameWithoutExtension(name) + "_" + CreateDateTimeWithValidChars() + Path.GetExtension(name);
                                fileNameForSQL = newFileName;
                                //string newFileName = Path.GetFileNameWithoutExtension(name) + "_" + Guid.NewGuid() + Path.GetExtension(name);
                                //string newFileName = name + "" + Guid.NewGuid();
                                outputForNir += " ---here3" + newFileName;

                                //delete all files begining with the same name
                                string[] names = Directory.GetFiles(rootPath);
                                foreach (var fileName in names)
                                {
                                    if (Path.GetFileNameWithoutExtension(fileName).IndexOf(Path.GetFileNameWithoutExtension(name)) != -1)
                                    {
                                        File.Delete(fileName);
                                    }
                                }

                                //File.Move(item.LocalFileName, Path.Combine(rootPath, newFileName));
                                File.Copy(item.LocalFileName, Path.Combine(rootPath, newFileName), true);
                                File.Delete(item.LocalFileName);
                                outputForNir += " ---here4";

                                Uri baseuri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, string.Empty));
                                outputForNir += " ---here5";
                                string fileRelativePath = "~/uploadFiles/" + newFileName;
                                outputForNir += " ---here6 imageName=" + fileRelativePath;
                                Uri fileFullPath = new Uri(baseuri, VirtualPathUtility.ToAbsolute(fileRelativePath));
                                outputForNir += " ---here7" + fileFullPath.ToString();
                                savedFilePath.Add(fileFullPath.ToString());
                            }
                            catch (Exception ex)
                            {
                                outputForNir += " ---excption=" + ex.Message;
                                string message = ex.Message;
                            }
                        }
                        counter++;
                    }
                    foreach (var key in provider.FormData.AllKeys)
                    {
                        foreach (var val in provider.FormData.GetValues(key))
                        {
                            Console.WriteLine(string.Format("{0}: {1}", key, val));
                            string StrCon = ConfigurationManager.ConnectionStrings["LIVEDNS"].ConnectionString;
                            SqlConnection con = new SqlConnection(StrCon);
                            try
                            {
                                con.Open();
                                SqlCommand com = new SqlCommand($"UPDATE FinalProject_Kasem_MatchesTB SET Match_Picture = '{fileNameForSQL}' WHERE Match_ID = {int.Parse(val)}", con);
                                int result = com.ExecuteNonQuery();
                                Console.WriteLine(result);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                outputForNir += " ---excption=" + e.Message;
                                string message = e.Message;
                            }
                            finally
                            {
                                if (con != null)
                                {
                                    if (con.State == ConnectionState.Open)
                                        con.Close();
                                }
                            }

                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.Created, "nirchen " + savedFilePath[0] + "!" + provider.FileData.Count + "!" + outputForNir + ":)");
                });
            return task;
        }

        private string CreateDateTimeWithValidChars()
        {
            return DateTime.Now.ToString().Replace('/', '_').Replace(':', '-').Replace(' ', '_');
        }
    }
}
