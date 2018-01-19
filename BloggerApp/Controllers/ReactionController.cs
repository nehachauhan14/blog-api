using BlogsDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace BloggerApp.Controllers
{
    public class ReactionController : ApiController
    {
        [HttpGet]
        [Route("api/Reaction/GetIsLiked")]
        public HttpResponseMessage GetIsLiked(int UID , int BID)
        {
            var isLiked = false; 
            using (BloggerAppDBEntities entities = new BloggerAppDBEntities())
            {
                try
                {
                    var blogs = entities.reaction_info.Where(e => (e.UID == UID) && (e.BID == BID)).ToList();
                    isLiked = (blogs.Count == 1) ? true : false; 
                    return Request.CreateResponse(HttpStatusCode.Found, isLiked);
                    
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Reaction/LikeOrUnlikeBlog")]
        public HttpResponseMessage LikeOrUnlikeBlog(int BID)
        {
            //var identity = this.User.Identity as ClaimsIdentity;
            //var nonRoleClaims = identity.Claims.Where(x => x.Type != ClaimsIdentity.DefaultRoleClaimType).Select(x => new { Type = x.Type, Value = x.Value }).ToList();
            //var userName = nonRoleClaims[0].Value;
            //var uid = Int32.Parse(nonRoleClaims[1].Value);
            var uid = 1007;
            var BlogToLike = new reaction_info();
            BlogToLike.BID = BID;
            BlogToLike.UID = uid;
            BlogToLike.isLiked = true;

           
            using (BloggerAppDBEntities entities = new BloggerAppDBEntities())
            {
                var blog = entities.reaction_info.Where(e => (e.UID == uid) && (e.BID == BID)).ToList();

                try
                {

                    if (blog.Count == 0)
                    {
                        entities.reaction_info.Add(BlogToLike);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        entities.reaction_info.Remove(BlogToLike);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
                catch(Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
            }

        }
}