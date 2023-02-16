using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking
{
    public class TInventory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FlightNumber { get; set; }

        public String? Airline { get; set; }

        public String? LocFrom { get; set; }

        public String? LocTo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public String? ScheduledDays { get; set; }

        public int BusinessSeats { get; set; }

        public int NonBusinessSeats { get; set; }

        public int TicketCost { get; set; }

        public int Rows { get; set; }

        public String? Meal { get; set; }
    }
}
