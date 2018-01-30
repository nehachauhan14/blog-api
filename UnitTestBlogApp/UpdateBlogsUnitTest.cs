using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BloggerApp.Controllers;
using BlogsDataAccess;

namespace UnitTestBlogApp
{
    [TestClass]
    public class UpdateBlogsUnitTest : BaseUnitTest
    {
        BlogsController controller = new BlogsController();
        int correctBid = 1009;
        int incorrectBid = 0;

        [TestMethod]
        public void Update_CorrectBID_ShouldUpdate()
        {
            Blog_Detail RandomGeneratedBlog = GenerateRandomBlog();

            Blog_Detail response = controller.UpdateBlogById(correctBid , RandomGeneratedBlog);
            bool BlogsAreEqual = response.Title == RandomGeneratedBlog.Title && response.Blog_Content == RandomGeneratedBlog.Blog_Content;

            Assert.IsTrue(BlogsAreEqual, "Blog having correct BID has been updated successfully! ");
        }

        [TestMethod]
        public void Update_IncorrectBID_shouldReturnNull()
        {
            Blog_Detail response = controller.UpdateBlogById(incorrectBid, GenerateRandomBlog());
            Assert.AreEqual(null, response.Title);
            Assert.AreEqual(null, response.Blog_Content);
        }

        [TestMethod]
        public void Update_TitleIsValid_shouldupdate()
        {
            var updatedBlog = GenerateRandomBlog();
            var updatedTitle = updatedBlog.Title;
            var updatedContent = updatedBlog.Blog_Content;

            Blog_Detail response = controller.UpdateBlogById(correctBid, updatedBlog);
            Assert.AreEqual(updatedTitle, response.Title);
          
        }

        [TestMethod]
        public void Update_BlogContentIsValid_ShouldUpdate()
        {
            var updatedBlog = GenerateRandomBlog();
            var updatedTitle = updatedBlog.Title;
            var updatedContent = updatedBlog.Blog_Content;
       
            Blog_Detail response = controller.UpdateBlogById(correctBid, updatedBlog);
            Assert.AreEqual(updatedContent, response.Blog_Content);
        }




        #region Model Creator
        private Blog_Detail GenerateRandomBlog()
        {
            return new Blog_Detail
            {
               
                Title = GenerateRandomString(10),
                Blog_Content = GenerateRandomString(100),
                DateOfUpdation = DateTime.Now
            };
        }
        #endregion
    }
}
