using HotelBookingManagementSystem.Api.Data;
using HotelBookingManagementSystem.Api.Models;
using HotelBookingManagementSystem.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingHistoryController : ControllerBase
    {
        DBContext objDBContext = new();

        [HttpPost]
        public async Task<int> AddBooking(BookingHistory objBookingHistory)
        {
            try
            {
                BookingHistoryViewModel newBookingHistory = new BookingHistoryViewModel
                {
                    UserId = objBookingHistory.UserId,
                    RoomType = objBookingHistory.RoomType,
                    BeddingType = objBookingHistory.BeddingType,
                    NoOfAdults = objBookingHistory.NoOfAdults,
                    NoOfChilds = objBookingHistory.NoOfChilds,
                    CheckIn = objBookingHistory.CheckIn,
                    CheckOut = objBookingHistory.CheckOut,
                    NetAmount = objBookingHistory.NetAmount,
                };

                await DBContext.dbConnect.InsertAsync(newBookingHistory);
                return newBookingHistory.BookingId;
            }
            catch { }
            return -1;
        }

        [HttpGet]
        public async Task<IEnumerable<BookingHistory>?> GetUserBookingHistory(String userId)
        {
            List<BookingHistory> userBookingHistory = new List<BookingHistory>();
            try
            {
                IEnumerable<BookingHistoryViewModel> result;
                if (userId == "admin") { result = await DBContext.dbConnect.QueryAsync<BookingHistoryViewModel>("SELECT * FROM BookingHistoryViewModel ORDER BY BookedOn DESC", Array.Empty<object>()); }
                else { result = await DBContext.dbConnect.QueryAsync<BookingHistoryViewModel>("SELECT * FROM BookingHistoryViewModel WHERE UserId=? ORDER BY BookedOn DESC", new object[] { userId }); }
                if (result != null)
                {
                    foreach (BookingHistoryViewModel currentBookingHistory in result)
                    {
                        userBookingHistory.Add(new BookingHistory
                        {
                            BookingId = currentBookingHistory.BookingId,
                            UserId = currentBookingHistory.UserId,
                            RoomType = currentBookingHistory.RoomType,
                            BeddingType = currentBookingHistory.BeddingType,
                            NoOfAdults = currentBookingHistory.NoOfAdults,
                            NoOfChilds = currentBookingHistory.NoOfChilds,
                            CheckIn = currentBookingHistory.CheckIn,
                            CheckOut = currentBookingHistory.CheckOut,
                            NetAmount = currentBookingHistory.NetAmount,
                            Status = currentBookingHistory.Status,
                            BookedOn = currentBookingHistory.BookedOn,
                        });
                    }
                }
                return userBookingHistory;
            } catch { return null; }
        }

        [HttpPut]
        public async Task<int> UpdateBooking(BookingHistory objBookingHistory)
        {
            try
            {
                BookingHistoryViewModel updateBookingHistory = new BookingHistoryViewModel
                {
                    BookingId = objBookingHistory.BookingId,
                    UserId = objBookingHistory.UserId,
                    RoomType = objBookingHistory.RoomType,
                    BeddingType = objBookingHistory.BeddingType,
                    NoOfAdults = objBookingHistory.NoOfAdults,
                    NoOfChilds = objBookingHistory.NoOfChilds,
                    CheckIn = objBookingHistory.CheckIn,
                    CheckOut = objBookingHistory.CheckOut,
                    NetAmount = objBookingHistory.NetAmount,
                    Status = objBookingHistory.Status,
                    BookedOn = objBookingHistory.BookedOn,
                };

                return await DBContext.dbConnect.UpdateAsync(updateBookingHistory);
            }
            catch { }
            return -1;
        }

        [HttpDelete]
        public async Task<int> DeleteBookings(String userId)
        {
            try
            {
                await DBContext.dbConnect.ExecuteScalarAsync<int>("DELETE FROM BookingHistoryViewModel WHERE UserId=?", new object[] { userId });
                return 1;
            }
            catch { }
            return -1;
        }
    }
}
