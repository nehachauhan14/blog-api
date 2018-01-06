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
    public class BlogsController : ApiController
    {
        /// <summary>
        /// Api to Get all Blogs created ! 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Blogs/GetBlogsList")]
        public HttpResponseMessage GetBlogsList()
        {
            using (BloggerAppDBEntities entities = new BloggerAppDBEntities())
            {
                var blogs = entities.Blog_Detail.OrderByDescending(x => x.DateOfUpdation).ToList();
                if (blogs != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, blogs);
                }
               else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No Blog found ! ");
                }
            }
        }
        /// <summary>
        /// To Get Blogs created by  specific Blogger 
        /// </summary>
        /// <param name="id"> it specifies the UID of Blogger</param>
        /// <returns></returns>

        [HttpGet]
        [Route("api/Blogs/GetBlogsById")]
        [Authorize]
        public HttpResponseMessage GetBlogsById()
        {
            using (BloggerAppDBEntities entities = new BloggerAppDBEntities())
            {
                var identity = this.User.Identity as ClaimsIdentity;
                var nonRoleClaims = identity.Claims.Where(x => x.Type != ClaimsIdentity.DefaultRoleClaimType).Select(x => new { Type = x.Type, Value = x.Value }).ToList();
                var userName = nonRoleClaims[0].Value;
                var uid = Int32.Parse(nonRoleClaims[1].Value);  
                var blogs = entities.Blog_Detail.ToList();
                var blogsById = blogs.Where(e => e.UID == uid).OrderByDescending(x => x.DateOfUpdation).ToList();

                if (blogsById != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, blogsById);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No blogs found for Blogger having username : " + userName );
                }

                }
        }
/// <summary>
/// This api returns the blog corresponding to bid
/// </summary>
/// <param name="bid">unique id for Blog</param>
/// <returns></returns>
        [HttpGet]
        [Route("api/Blogs/GetBlogsByBid/{bid}")]
        public HttpResponseMessage GetBlogsByBid(int bid)
        {
            using (BloggerAppDBEntities entities = new BloggerAppDBEntities())
            {
                
                var blogs = entities.Blog_Detail.ToList();
                var blogsById = blogs.Where(e => e.BID == bid).ToList();

                if (blogsById != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, blogsById);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No blog found Present for Blogger having UID " + bid + " not found !");
                }

            }
        }


        /// <summary>
        /// To create Blog
        /// </summary>
        /// <param name="blog">blog object</param>
        /// <returns></returns>
        [Authorize]
        public HttpResponseMessage CreateBlog([FromBody] Blog_Detail blog)
        {
            var identity = this.User.Identity as ClaimsIdentity;
            var nonRoleClaims = identity.Claims.Where(x => x.Type != ClaimsIdentity.DefaultRoleClaimType).Select(x => new { Type = x.Type, Value = x.Value }).ToList();
            var uid = Int32.Parse(nonRoleClaims[1].Value);
            blog.UID = uid; 
            try
            {
                using (BloggerAppDBEntities entities = new BloggerAppDBEntities())
                {
                    blog.DateOfUpdation = DateTime.Now;
                    entities.Blog_Detail.Add(blog);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, blog);
                    message.Headers.Location = new Uri(Request.RequestUri + blog.Title.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        /// <summary>
        /// To delete blog based on its id i.e. BID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Blogs/DeleteBlogById/{bid}")]
        public HttpResponseMessage DeleteBlogById(int bid)
        {

            using (BloggerAppDBEntities entity = new BloggerAppDBEntities())
            {
                var blogToDelete = entity.Blog_Detail.FirstOrDefault(e => e.BID == bid);
                if (blogToDelete != null)
                {
                    entity.Blog_Detail.Remove(blogToDelete);
                    entity.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Blog with BID = " + bid + " Not found ! ");
                }
            }
        }
        /// <summary>
        /// To update Blog
        /// </summary>
        /// <param name="id"></param>
        /// <param name="blog"></param>
        [HttpPut]
        [Route("api/Blogs/UpdateBlogById/{bid}")]
        public HttpResponseMessage UpdateBlogById(int bid, [FromBody] Blog_Detail blog)
        {
            try
            {
                using (BloggerAppDBEntities entity = new BloggerAppDBEntities())
                {
                    var blogToEdit = entity.Blog_Detail.FirstOrDefault(e => e.BID == bid);
                    if (blogToEdit != null)
                    {
                        blogToEdit.Title = blog.Title;
                        blogToEdit.Blog_Content = blog.Blog_Content;
                        blogToEdit.DateOfUpdation = DateTime.Now;
                        entity.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, blogToEdit);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Blog with BID = " + bid + " Not found ! ");
                    }
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetBlogsByFilter(string filter)
        {
            using (BloggerAppDBEntities entities = new BloggerAppDBEntities())
            {
                if (filter == "All")
                {
                    var blogs = entities.Blog_Detail.OrderByDescending(x => x.DateOfUpdation).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, blogs);
                }
                else
                {
                    var userId = entities.User_Details.Where(x => x.UserName == filter).FirstOrDefault().UID;

                    var blogs = entities.Blog_Detail.Where(x => x.UID == userId).OrderByDescending(x => x.DateOfUpdation).ToList();
                    if (blogs != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, blogs);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "No blog found for Blogger " + filter);
                    }
                }
            }

        }
    }
}
