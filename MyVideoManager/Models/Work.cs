using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyVideoManager.Models
{
    public class Work
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Episode { get; set; }
        public Catagory Catagory { get; set; }
        public ICollection<UserWork> Favs { get; set; }
    }
}
