using System.ComponentModel.DataAnnotations;

namespace Booking
{
    public class TBooking
    {
        [Key]
        public Int64 Pnr { get; set; }

        public String? Name { get; set; }

        public String? Email { get; set; }

        public String? PassengerDetails { get; set; }

        public DateTime BookingDate { get; set; }

        public String? Meal { get; set; }

        public int seatNumber { get; set; }
    }
}
