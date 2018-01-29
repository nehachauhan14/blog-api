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

        [TestMethod]
        public void Update_with_correct_data()
        {
            Blog_Detail response = controller.UpdateBlogById(1009, GenerateRandomBlog());
            Assert.AreSame(GenerateRandomBlog(), response);
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
