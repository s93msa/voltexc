using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using WebApplication1.Classes;
using WebApplication1.Controllers;

namespace WebApplication1.Models
{

    public class Contest
    {
        public int ContestId { get; set; }
        public string Location{ get; set; }
        public string Country { get; set; } //TODO: Country of club not contest
        public virtual List<StartListClassStep> StartListClassStep { get; set; }


        //public object Clone()
        //{
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        if (this.GetType().IsSerializable)
        //        {
        //            BinaryFormatter formatter = new BinaryFormatter();
        //            formatter.Serialize(stream, this);
        //            stream.Position = 0;
        //            return formatter.Deserialize(stream);
        //        }
        //        return null;
        //    }
        //}
    }
}