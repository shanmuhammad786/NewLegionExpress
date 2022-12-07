using System;
using System.Collections.Generic;
using System.Text;

namespace legionexpress.Models
{
    public class AuthenticationResponseModel
    {
        public AuthKey result { get; set; }
        public string errorMessage { get; set; }
        public bool hasError { get; set; }
    }

    public class AuthKey
    {
        public string apiKey { get; set; }
    }
}
