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
using System.Web.Http.Cors;

namespace Soccer_Vision.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [Route("uploadpicture")]
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
                                SqlCommand com = new SqlCommand($"UPDATE FinalProject_Kasem_UsersTB SET ProfilePIC = '{fileNameForSQL}' WHERE User_ID = {int.Parse(val)}",con);
                                int result = com.ExecuteNonQuery();
                                Console.WriteLine(result);
                            }
                            catch(Exception e)
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
