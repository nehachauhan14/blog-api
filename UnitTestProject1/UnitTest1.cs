using Microsoft.VisualStudio.TestTools.UnitTesting;
using BloggerApp.Controllers;
using BlogsDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net;
using System.Net.Http;


namespace BloggerApp.Controllers.Tests
{
    [TestClass()]
    public class UnitTest1
    {
        [TestMethod()]
        public void GetBlogsList()
        {
            BlogsController BlogsControllerobj = new BlogsController();
            //Blog_Detail b = new Blog_Detail();
            //b.BID = 1;
            //b.Blog_Content = "testing api";
            //b.DateOfUpdation = DateTime.Now;
            var result = BlogsControllerobj.GetBlogsList();
            // comp = BlogsControllerobj.UpdateBlogById(1, b);

            //  Assert.Equals()


        }
    }
}