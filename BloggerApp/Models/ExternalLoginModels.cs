using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BloggerApp.Models
{
    public class ExternalLoginModels
    {
        public class ExternalLoginViewModel
        {
            public string Name { get; set; }

            public string Url { get; set; }

            public string State { get; set; }
        }

        public class RegisterExternalBindingModel
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            public int id { get; set; }

            [Required]
            public string email { get; set; }

        }
    }
}