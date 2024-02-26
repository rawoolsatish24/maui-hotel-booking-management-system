using HotelBookingManagementSystem.Api.ViewModels;
using Microsoft.VisualBasic.FileIO;
using SQLite;

namespace HotelBookingManagementSystem.Api.Data
{
    public class DBContext
    {
        private const string DBName = "HBMS.db3";
        public static SQLiteAsyncConnection dbConnect = new SQLiteAsyncConnection(Path.Combine(FileSystem.CurrentDirectory, DBName));

        public DBContext()
        {
            dbConnect.CreateTableAsync<UserMasterViewModel>().Wait();
            dbConnect.CreateTableAsync<BookingHistoryViewModel>().Wait();
        }

        public static String FilterInput(String input) { return input.Replace("\'", "").Replace("\"", "").Trim().ToUpper(); }
    }
}
