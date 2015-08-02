using System.Collections.Generic;

namespace UrbanDecathlon.Models
{
    public class Template
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public virtual IList<Event> Events { get; set; }

        public virtual IList<Athlete> Athletes { get; set; }
	}
}