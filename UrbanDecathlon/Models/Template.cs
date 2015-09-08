using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrbanDecathlon.Models
{
    public class Template
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public virtual IList<Event> Events { get; set; }

        public virtual IList<Athlete> Athletes { get; set; }

        [NotMapped]
        public bool IsNew { get; set; }
	}
}