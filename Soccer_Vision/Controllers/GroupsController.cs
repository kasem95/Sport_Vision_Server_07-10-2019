using Soccer_Vision.Models;
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

namespace Soccer_Vision.Controllers
{
    [RoutePrefix("api/Groups")]
    public class GroupsController : ApiController
    {
        [Route("getGroups")]
        public IHttpActionResult GetGroups()
        {
            try
            {
                DAL dal = new DAL();
                DataTable groups = dal.getGroupsTable();
                return Content(HttpStatusCode.OK, groups);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        public IHttpActionResult PostCreateGroup([FromBody]Group group)
        {
            try
            {
                DAL dal = new DAL();
                int result = dal.createGroup(group.Admin_ID, group.Group_Name, group.Max_Players);
                return Content(HttpStatusCode.Created, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }


        }

        [Route("getUsersInGroupTable")]
        public IHttpActionResult GetUsersInGroupTable()
        {
            try
            {
                DAL DAL = new DAL();
                var result = DAL.getUsersInGroupList();
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest("No users In the Groups");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [Route("getUsersInvitedToGroupTable")]
        public IHttpActionResult GetUsersInvitedToGroupTable()
        {
            try
            {
                DAL DAL = new DAL();
                var result = DAL.getUsersInvitedToGroupTable();
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest("No users Invited to Groups");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [Route("CancelGroup")]
        public IHttpActionResult DeleteGroup([FromBody]Group value)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.CancelGroup(value.Group_ID);
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

        [Route("ExitGroup")]
        public IHttpActionResult PutExitMatch([FromBody]DetailsForExitGroup value)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.ExitGroup(value.UserID,value.GroupID,value.AdminID,value.Users_Joined);
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

        [Route("CancelGroupRequest")]
        public IHttpActionResult DeleteRequest([FromBody]Group value)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.CancelGroupRequest(value.Admin_ID, value.Group_ID);
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

        [Route("CancelGroupInvite")]
        public IHttpActionResult DeleteInvite([FromBody]Group value)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.CancelGroupInvite(value.Admin_ID, value.Group_ID);
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

        [Route("RequestJoinGroup")]
        public IHttpActionResult PostRequestJoinGroup([FromBody]RequestJoinGroupClass value)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.RequestJoinGroup(value.userID, value.groupID, value.maxPlayer);
                if (result == "You have requested to join the group")
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

        [Route("acceptRequestJoinGroup")]
        public IHttpActionResult PutAcceptRequestJoinGroup([FromBody]RequestJoinGroupClass value)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.AcceptRequestJoinGroup(value.userID,value.groupID,value.maxPlayer,value.IsInvite);
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

        [Route("InviteFriendToGroup")]
        public IHttpActionResult PostInviteFriendToGroup([FromBody]RequestJoinGroupClass value)
        {
            try
            {
                DAL dal = new DAL();
                string result = dal.inviteFriendToGroup(value.groupID, value.userID, value.maxPlayer);
                if (result == "You have invited him to join your group!")
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

        [Route("uploadGrouppicture")]
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
                                SqlCommand com = new SqlCommand($"UPDATE FinalProject_Kasem_GroupsTB SET Group_Picture = '{fileNameForSQL}' WHERE Group_ID = {int.Parse(val)}", con);
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
