using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyVideoManager.Models
{
    public enum Rank : int { Bad, Normal, Good, };

    public class UserWork
    {
        public long UserWorkId { get; set; }
        public string UserId { get; set; }
        public long WorkId { get; set; }
        public int Watched { get; set; }
        public bool IsFavorite { get; set; }
        public Rank? Ranking { get; set; }
        public User User { get; set; }
        public Work Work { get; set; }
    }
}
