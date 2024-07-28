using BobyDesignWeb.Data;
using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Models;
using BobyDesignWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;

namespace BobyDesignWeb.Controllers
{
    
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService) 
        {
            this._usersService = usersService;
        }

        [HttpGet]
        [Authorize(Roles = UserRolesConstants.Admin)]
        public Task<PageViewModel<UserViewModel>> Search(int page, string? searchPhrase, string? role)
        {    
            return _usersService.Search(page, searchPhrase, role);
        }
        
        [HttpGet]
        public string[] Roles()
        {
            return _usersService.Roles(User);
        }

        [Authorize(Roles = UserRolesConstants.Admin)]
        [HttpGet]
        public RoleItem[] RolesByUserId(string userId)
        {
            return _usersService.RolesByUserId(userId);
        }

        [Authorize(Roles = UserRolesConstants.Admin)]
        [HttpPost]
        public async Task<ActionResult> EditUserRoles([FromBody]UserRolesModel userRoles)
        {
            await _usersService.EditUserRoles(userRoles);
            return Ok();
        }
    }
}
