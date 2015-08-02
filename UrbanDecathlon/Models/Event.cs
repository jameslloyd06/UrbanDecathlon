using System.Collections.Generic;

namespace UrbanDecathlon.Models
{
    public class Event
	{
        public int Id { get; set; }

        public string Name { get; set; }
        
        public virtual IList<Position> Positions { get; set; }
	}
}