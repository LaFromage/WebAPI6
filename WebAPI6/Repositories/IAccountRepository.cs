﻿using Microsoft.AspNetCore.Identity;
using WebAPI6.Models;

namespace WebAPI6.Repositories
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);
        
    }
}
