using HotelBookingManagementSystem.Api.Data;
using HotelBookingManagementSystem.Api.Models;
using HotelBookingManagementSystem.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMasterController : ControllerBase
    {
        DBContext objDBContext = new();

        [HttpPost]
        public async Task<int> CreateUser(UserMaster objUserMaster)
        {
            try
            {
                UserMasterViewModel newUserMaster = new UserMasterViewModel
                {
                    FullName = DBContext.FilterInput(objUserMaster.FullName),
                    Email = DBContext.FilterInput(objUserMaster.Email),
                    Mobile = DBContext.FilterInput(objUserMaster.Mobile),
                    Password = DBContext.FilterInput(objUserMaster.Password)
                };

                if (await emailExists(0, newUserMaster.Email)) { return 0; }

                await DBContext.dbConnect.InsertAsync(newUserMaster);
                return newUserMaster.UserId;
            }
            catch {}
            return -1;
        }

        [HttpGet("ValidateLogin")]
        public async Task<UserMaster?> ValidateLogin(String email, String password)
        {
            try
            {
                UserMasterViewModel loginUser = await DBContext.dbConnect.FindWithQueryAsync<UserMasterViewModel>("SELECT * FROM UserMasterViewModel WHERE Email=? AND Password=? LIMIT 1", new object[] { DBContext.FilterInput(email), DBContext.FilterInput(password) });
                if (loginUser != null)
                {
                    return new UserMaster
                    {
                        UserId = loginUser.UserId,
                        FullName = loginUser.FullName,
                        Email = loginUser.Email,
                        Mobile = loginUser.Mobile,
                        Password = loginUser.Password,
                        CreatedOn = loginUser.CreatedOn
                    };
                }
            }
            catch { }
            return null;
        }

        [HttpGet("SearchUser")]
        public async Task<UserMaster?> SearchUser(String userId)
        {
            try
            {
                UserMasterViewModel searchUser = await DBContext.dbConnect.FindWithQueryAsync<UserMasterViewModel>("SELECT * FROM UserMasterViewModel WHERE UserId=? LIMIT 1", new object[] { userId });
                if (searchUser != null)
                {
                    return new UserMaster
                    {
                        UserId = searchUser.UserId,
                        FullName = searchUser.FullName,
                        Email = searchUser.Email,
                        Mobile = searchUser.Mobile,
                        Password = searchUser.Password,
                        CreatedOn = searchUser.CreatedOn
                    };
                }
            }
            catch { }
            return null;
        }

        [HttpPut]
        public async Task<UserMaster?> UpdateUser(UserMaster objUserMaster)
        {
            try
            {
                UserMasterViewModel updateUserMaster = new UserMasterViewModel
                {
                    UserId = objUserMaster.UserId,
                    FullName = DBContext.FilterInput(objUserMaster.FullName),
                    Email = DBContext.FilterInput(objUserMaster.Email),
                    Mobile = DBContext.FilterInput(objUserMaster.Mobile),
                    Password = DBContext.FilterInput(objUserMaster.Password),
                    CreatedOn = objUserMaster.CreatedOn
                };

                if (!(await emailExists(updateUserMaster.UserId, updateUserMaster.Email)))
                {
                    await DBContext.dbConnect.UpdateAsync(updateUserMaster);
                    return new UserMaster
                    {
                        UserId = updateUserMaster.UserId,
                        FullName = updateUserMaster.FullName,
                        Email = updateUserMaster.Email,
                        Mobile = updateUserMaster.Mobile,
                        Password = updateUserMaster.Password,
                        CreatedOn = updateUserMaster.CreatedOn
                    };
                }
            }
            catch { }
            return null;
        }

        [HttpDelete]
        public async Task<int> DeleteUser(String userId)
        {
            try
            {
                await DBContext.dbConnect.DeleteAsync(new UserMasterViewModel{ UserId = int.Parse(userId), });
                return 1;
            }
            catch { }
            return -1;
        }

        private async Task<bool> emailExists(int userId, String email)
        {
            int emailExists = await DBContext.dbConnect.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM UserMasterViewModel WHERE UserId!=? AND Email=?", new object[] { userId, email });
            return emailExists == 1;
        }
    }
}
