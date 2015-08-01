namespace UrbanDecathlon.Models
{
    public class Position
	{
        public int Id { get; set; }

        public int Order { get; set; }

        public double Score { get; set; }

        public Athlete Athlete { get; set; }
	}
}