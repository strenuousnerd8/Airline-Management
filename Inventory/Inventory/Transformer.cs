using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace Inventory
{
    public class Transformer
    {
        private readonly string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Inventory;Trusted_Connection=True;";

        protected  SqlConnection Connection { get; set; }

        public Transformer()
        {
            Connection = new SqlConnection(_connectionString);
        }

        public string GetValues(int id)
        {
            using (SqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT [Airline], [LocFrom], [LocTo], [StartDate], [EndDate], [ScheduledDays], [BusinessSeats], [NonBusinessSeats], [TicketCost], [Rows], [Meal] from [dbo].[TInventory] Where [FlightNumber] = @FlightNumber;";
                cmd.Parameters.AddWithValue(@"FlightNumber", id);
                cmd.Connection.Open();

                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    ArrayList res = new ArrayList();
                    var Airline = "";

                    while (reader.Read())
                    {
                        Airline = "Airline:\t" + reader["Airline"].ToString() + "\n";
                        res.Add(Airline);
                        Airline = "Location From:\t" + reader["LocFrom"].ToString() + "\n";
                        res.Add(Airline);
                        Airline = "Location To:\t" + reader["LocTo"].ToString() + "\n";
                        res.Add(Airline);
                        Airline = "Start Date:\t" + reader["StartDate"].ToString() + "\n";
                        res.Add(Airline);
                        Airline = "End Date:\t" + reader["EndDate"].ToString() + "\n";
                        res.Add(Airline);
                        Airline = "Scheduled Days:\t" + reader["ScheduledDays"].ToString() + "\n";
                        res.Add(Airline);
                        Airline = "Business Seats:\t" + reader["BusinessSeats"].ToString() + "\n";
                        res.Add(Airline);
                        Airline = "Non Business Seats\t" + reader["NonBusinessSeats"].ToString() + "\n";
                        res.Add(Airline);
                        Airline = "Ticket Cost:\t" + reader["TicketCost"].ToString() + "\n";
                        res.Add(Airline);
                        Airline = "Rows:\t" + reader["Rows"].ToString() + "\n";
                        res.Add(Airline);
                        Airline = "Meal:\t" + reader["Meal"].ToString() + "\n";
                        res.Add(Airline);
                    }
                    string ResultString = String.Join("", res.ToArray());
                    return ResultString;
                }
            }
        }
    }
}
