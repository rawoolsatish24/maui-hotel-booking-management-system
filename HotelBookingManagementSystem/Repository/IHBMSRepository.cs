using HotelBookingManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingManagementSystem.Repository
{
    public interface IHBMSRepository
    {
        Task<int> CreateUser(UserMaster objUserMaster);
        Task<Object> ValidateLogin(String email, String password);
        Task<Object> SearchUser(String userId);
        Task<Object> UpdateUser(UserMaster objUserMaster);
        Task<int> DeleteUser(String userId);

        Task<int> AddBooking(BookingHistory objBookingHistory);
        Task<Object> GetUserBookingHistory(String userId);
        Task<int> UpdateBooking(BookingHistory objBookingHistory);
        Task<int> DeleteBookings(String userId);
    }
}
