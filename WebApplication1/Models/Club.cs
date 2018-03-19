using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Club
    {
        public int ClubId { get; set; }
        public string ClubName{ get; set; }

        [Index]
        public int ClubTdbId { get; set; }

    }
}