using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingManagementSystem.Api.ViewModels
{
    public class BookingHistoryViewModel
    {
        [PrimaryKey, AutoIncrement]
        public int BookingId { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }

        public string? RoomType { get; set; }

        public string? BeddingType { get; set; }

        public int NoOfAdults { get; set; }

        public int NoOfChilds { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public int NetAmount { get; set; }

        public string Status { get; set; } = "PENDING";

        public DateTime BookedOn { get; set; } = DateTime.Now;
    }
}
