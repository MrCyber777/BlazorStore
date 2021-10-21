using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorStore.Data.Services
{
    public class RoleManagerService
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleManagerService(ApplicationDbContext db, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            List<IdentityRole> allRoles = await _db.Roles.ToListAsync();
            return allRoles;
            //return _roleManager.Roles.ToList();
        }

        public async Task<List<IdentityUser>> GetAllUserRolesAsync()
        {
            List<IdentityUser> allUsersWithRoles = await _db.Users.ToListAsync();
            return allUsersWithRoles;
        }

        public async Task<bool> CreateUpdateRoleAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;
            if (!await _roleManager.RoleExistsAsync(name))
                await _roleManager.CreateAsync(new IdentityRole(name));
            else
            {
                var result = await _roleManager.FindByNameAsync(name);
                if (result is null)
                    return false;
                result.Name = name;
                await _roleManager.UpdateAsync(result);
            }
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditUserRoleAsync(string userId, string roleName)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return false;
            var applicationUser = await _userManager.FindByIdAsync(userId);
            if (applicationUser is not null)
            {
                var role = await _userManager.GetRolesAsync(applicationUser);
                await DeleteUserRoleAsync(userId, role.First());
                await _userManager.AddToRoleAsync(applicationUser, roleName);
                await _db.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public async Task<bool> DeleteUserRoleAsync(string userId, string roleName)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return false;
            var applicationUser = await _userManager.FindByIdAsync(userId);
            if (applicationUser is not null)
            {
                await _userManager.RemoveFromRoleAsync(applicationUser, roleName);
                await _db.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        //public async Task<bool> UpdateRoleAsync(string name)
        //{
        //    if (string.IsNullOrWhiteSpace(name))
        //        return false;

        //    await _db.SaveChangesAsync();
        //    return true;
        //}
        public async Task<bool> DeleteRoleAsync(string name)
        {
            IdentityRole role = await _roleManager.FindByNameAsync(name);
            if (role is not null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return true;
        }
    }
}