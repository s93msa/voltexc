namespace VoltigeCore.Models
{
    public class Horse
    {
        public int HorseId { get; set; }
        public string HorseName { get; set; }
        public int HorseTdbId { get; set; }
        public int? LungerId { get; set; }
        public virtual Lunger Lunger { get; set; }
    }
}
