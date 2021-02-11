using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksAPI.Models;
using TasksAPI.Repositories.Interfaces;

namespace TasksAPI.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        /*
         * ".Result": Método utilizado em métodos assincronos para fazer a conversão para não assincrono.
         */

        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser Get(string email, string password)
        {
            var user = _userManager.FindByEmailAsync(email).Result;
            if (_userManager.CheckPasswordAsync(user, password).Result)
            {
                return user;
            }
            else
            {
                throw new Exception("Usuário não localizado!");
            }
        }

        public ApplicationUser Get(string id)
        {
            return _userManager.FindByIdAsync(id).Result;
        }

        public void Add(ApplicationUser user, string password)
        {
            var result = _userManager.CreateAsync(user, password).Result;
            if (result.Succeeded)
            {
                //return
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    sb.Append(error.Description);
                }

                throw new Exception($"Usuário não cadastrado! {sb.ToString()}");
            }
        }

    }
}
