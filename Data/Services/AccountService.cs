using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Management;
using BlazorStore.Data.Models;

namespace BlazorStore.Data.Services
{
    public class AccountService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountService(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<List<IdentityUser>> GetAllUsersAsync()
        {
            List<IdentityUser> listOfUsers = await _userManager.Users.ToListAsync();
            return listOfUsers;
        }
        public string GetUserIPOrNull()
        {
            IPHostEntry hostIpInfo = Dns.GetHostEntry(Dns.GetHostName());
            string currentIp = Convert.ToString(hostIpInfo.AddressList.FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));
            return (!string.IsNullOrWhiteSpace(currentIp)) ? currentIp : null;
        }
        public string GetUserMACOrNull()
        {
            ManagementClass mc = new("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string macAddress = string.Empty;

            foreach (var item in moc)
            {
                if (string.IsNullOrEmpty(macAddress))
                {
                    if ((bool)item["IPEnabled"])
                    {
                        macAddress = item["MacAddress"].ToString();
                        item.Dispose();
                        break;
                    }
                }
                item.Dispose();
            }

            return (!string.IsNullOrEmpty(macAddress)) ? macAddress : null;
        }
        public async Task<bool> AddressIsLocked(string userAddress, bool isIp)
        {
            if (isIp)
            {
                var blockedIP = await _db.IPBlackLists.FirstOrDefaultAsync(x => x.Address == userAddress);
                return (blockedIP is null) ? false : true;
            }
            else
            {
                var blockedMac = await _db.MacBlackLists.FirstOrDefaultAsync(x => x.Address == userAddress);
                return (blockedMac is null) ? false : true;
            }
        }
        public async Task<string> BanByIpOrMac(string id, bool isIp)
        {
            var userFromDB = await _userManager.FindByIdAsync(id);

            if (userFromDB is null)
                return "User not found";

            userFromDB.LockoutEnd = DateTime.Now.AddYears(1000);

            if (isIp)
            {
                var user = userFromDB as ApplicationUser;
                IPBlackList iPBlackList = new()
                {
                    Address = user.IP,
                    UserId = user.Id
                };
                await _db.IPBlackLists.AddAsync(iPBlackList);
                await _db.SaveChangesAsync();
                return $"Ip address {user.IP} successfully banned!";
            }
            else
            {
                var user = userFromDB as ApplicationUser;
                MacBlackList macBlackList = new()
                {                    
                    Address = user.Mac,
                    UserId = user.Id
                };
                await _db.MacBlackLists.AddAsync(macBlackList);
                await _db.SaveChangesAsync();
                return "User has been banned by MAC successfully!";
            }            
        }

    }
}
