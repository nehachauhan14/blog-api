using BlogsDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BloggerApp.Controllers
{
    public class BloggerController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetBlogersList()
        {
            using (BloggerAppDBEntities entities = new BloggerAppDBEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.User_Details.ToList());
            }
        }

        /// <summary>
        ///  need to change this API to handle error or duplicate Uname isssue 
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("api/Blogger/RegisterUser")]
        public HttpResponseMessage RegisterUser([FromBody] User_Details newUser)
        {
                try
                {
                using (BloggerAppDBEntities entities = new BloggerAppDBEntities())
                    {
                    var user = entities.User_Details.FirstOrDefault(d => d.UserName == newUser.UserName);
                    if (user == null)
                    {
                        entities.User_Details.Add(newUser);
                        entities.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.Created, newUser);
                        message.Headers.Location = new Uri(Request.RequestUri + newUser.UserName.ToString());
                        return message;
                    }
                    else
                    {
                        var message = Request.CreateResponse(HttpStatusCode.Conflict, "User already exists!");
                        return message;
                    }

                }   
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
 




    }
}
