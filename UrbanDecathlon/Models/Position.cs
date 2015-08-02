namespace UrbanDecathlon.Models
{
    public class Position
	{
        public int Id { get; set; }

        public int Order { get; set; }

        public double Score { get; set; }

        public virtual Athlete Athlete { get; set; }
	}
}