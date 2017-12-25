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
        public HttpResponseMessage Get()
        {
            using (BloggerAppDBEntities entities = new BloggerAppDBEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.User_Details.ToList());
            }
        }

    }
}
