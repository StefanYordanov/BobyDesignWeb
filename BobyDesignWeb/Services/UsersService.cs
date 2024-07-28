using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Data;
using BobyDesignWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BobyDesignWeb.Services
{
    public class UsersService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<PageViewModel<UserViewModel>> Search(int page, string? searchPhrase, string? role)
        {
            page = page <= 0 ? 1 : page;
            int pageSize = 20;
            searchPhrase = searchPhrase != null ? searchPhrase.ToLower() : string.Empty;
            IQueryable<ApplicationUser> usersQuery;
            if (string.IsNullOrWhiteSpace(role) || role.ToLower() == "all")
            {
                usersQuery = _userManager.Users;
            }
            else if (role.ToLower() == "none")
            {
                usersQuery = from user in _context.Users
                             join userRole in _context.UserRoles on user.Id equals userRole.UserId into ur
                             from urLeft in ur.DefaultIfEmpty()
                             where urLeft == null
                             select user;
            }
            else
            {
                usersQuery = from user in _context.Users
                             join userRole in _context.UserRoles on user.Id equals userRole.UserId
                             join dbRole in _context.Roles on userRole.RoleId equals dbRole.Id
                             where role.Normalize() == dbRole.NormalizedName
                             select user;
            }
            usersQuery = usersQuery.Where(u => u.UserName.ToLower().Contains(searchPhrase) ||
                u.Email.ToLower().Contains(searchPhrase) ||
                (u.FirstName + ' ' + u.LastName).ToLower().Contains(searchPhrase) ||
                u.PhoneNumber.ToLower().Contains(searchPhrase)
            );

            int usersCount = usersQuery.Count();

            var users = await usersQuery.OrderBy(x => x.UserName).Skip((page - 1) * pageSize).Take(pageSize).Select(x => new UserViewModel()
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhoneNumber = x.PhoneNumber,
            }).ToListAsync();

            return new PageViewModel<UserViewModel>()
            {
                Items = users,
                PagesCount = (int)Math.Ceiling(usersCount / Convert.ToDouble(pageSize)),
                CurrentPage = page,
            };
        }

        [HttpGet]
        public string[] Roles(ClaimsPrincipal user)
        {
            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return Array.Empty<string>();
            }
            var possibleRoles = new string[]
            { UserRolesConstants.Admin, UserRolesConstants.Seller};

            return possibleRoles.Where(x => user.IsInRole(x)).ToArray();
        }

        [Authorize(Roles = UserRolesConstants.Admin)]
        [HttpGet]
        public RoleItem[] RolesByUserId(string userId)
        {
            var query =
                from dbRole in _context.Roles
                join userRole in (_context.UserRoles.Where(x => x.UserId == userId)) on dbRole.Id equals userRole.RoleId into ur
                from urLeft in ur.DefaultIfEmpty()
                select new RoleItem
                {
                    Role = dbRole.Name,
                    Active = urLeft != null
                };

            return query.ToArray();
        }

        [Authorize(Roles = UserRolesConstants.Admin)]
        [HttpPost]
        public async Task EditUserRoles(UserRolesModel userRoles)
        {
            var user = await _userManager.FindByIdAsync(userRoles.UserId);
            if (user == null)
            {
                throw new ArgumentException("Не е намерен потребител");
            }

            var adminRole = userRoles.Roles.FirstOrDefault(x => x.Role == UserRolesConstants.Admin);

            if (adminRole != null && !adminRole.Active)
            {
                var admins = await _userManager.GetUsersInRoleAsync(UserRolesConstants.Admin);
                if (admins.Count == 1 && admins.First().Id == user.Id)
                {
                    throw new ArgumentException("Последният наличен админ не може да бъде премахнат");
                }
            }

            foreach (var role in userRoles.Roles)
            {
                if (role.Active)
                {
                    await _userManager.AddToRoleAsync(user, role.Role);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Role);
                }
            }
        }
    }
}
