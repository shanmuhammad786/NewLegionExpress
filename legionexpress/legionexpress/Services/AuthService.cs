using legionexpress.Models;
using legionexpress.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace legionexpress.Services
{
    internal class AuthService : MobileServiceBase
    {
        public async Task<AuthenticationResponseModel> GenerateToken(LoginModel objRequest)
        {
            string url = $"/api/v1/user/login";

            return await PostUnauthorized<AuthenticationResponseModel, LoginModel>(objRequest, url);
        }
    }
}
