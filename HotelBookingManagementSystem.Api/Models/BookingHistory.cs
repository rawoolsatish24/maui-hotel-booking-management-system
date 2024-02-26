namespace HotelBookingManagementSystem.Api.Models
{
    public class BookingHistory
    {
        public int BookingId { get; set; }

        public required int UserId { get; set; }

        public required string RoomType { get; set; }

        public required string BeddingType { get; set; }

        public required int NoOfAdults { get; set; }

        public required int NoOfChilds { get; set; }

        public required DateTime CheckIn { get; set; }

        public required DateTime CheckOut { get; set; }

        public required int NetAmount { get; set; }

        public string? Status { get; set; }

        public DateTime BookedOn { get; set; }
    }
}
