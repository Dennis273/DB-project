using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyVideoManager.Models
{
    public class Catagory
    {
        [Key]
        public string Name { get; set; }
        public User Administrator { get; set; }
        ICollection<Work> CatagoryWorks { get; set; }
    }
}
