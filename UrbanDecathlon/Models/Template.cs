using System.Collections.Generic;

namespace UrbanDecathlon.Models
{
    public class Template
	{
        public int Id { get; set; }

        public int Name { get; set; }

        public int Password { get; set; }

        public IList<Event> Events { get; set; }
	}
}