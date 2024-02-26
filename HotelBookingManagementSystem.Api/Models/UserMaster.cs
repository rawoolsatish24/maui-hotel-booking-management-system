namespace HotelBookingManagementSystem.Api.Models
{
    public class UserMaster
    {
        public int UserId { get; set; }

        public required string FullName { get; set; }

        public required string Email { get; set; }

        public required string Mobile { get; set; }

        public required string Password { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
