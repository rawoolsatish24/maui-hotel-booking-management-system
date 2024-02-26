using SQLite;

namespace HotelBookingManagementSystem.Api.ViewModels
{
    public class UserMasterViewModel
    {
        [PrimaryKey, AutoIncrement]
        public int UserId { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Mobile { get; set; }

        public string? Password { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}