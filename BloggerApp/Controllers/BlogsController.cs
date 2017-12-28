using BlogsDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BloggerApp.Controllers
{
    public class BlogsController : ApiController
    {
        /// <summary>
        /// Api to Get all Blogs created ! 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public HttpResponseMessage GetBlogsList()
        {
            using (BloggerAppDBEntities entities = new BloggerAppDBEntities())
            {
               
                var entity = entities.Blog_Detail.ToList();
                if(entity  != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
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
        /// <param name="id"> it specifies the BID of Blogger</param>
        /// <returns></returns>
        public HttpResponseMessage GetBlogsById(int id)
        {
            using (BloggerAppDBEntities entities = new BloggerAppDBEntities())
            {
                var blogs = entities.Blog_Detail.ToList();   
               var blogsById = blogs.Where(e => e.UID == id).ToList();

                if (blogsById != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, blogsById);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No blog found Present for Blogger having UID " + id + " not found !");
                }

            }
        }
        /// <summary>
        /// To create Blog
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        public HttpResponseMessage CreateBlog([FromBody] Blog_Detail blog)
        {
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
        /// <returns></returns>
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

    }
}
