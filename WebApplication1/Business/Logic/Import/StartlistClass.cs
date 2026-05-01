using System.Collections.Generic;

namespace VoltigeCore.Business.Logic.Import
{
    public class StartlistClass
    {
        public int startListClassId;
        public List<HorseLoungerVaulters> horseLoungerVaultersList;

        public StartlistClass()
        {
            horseLoungerVaultersList = new List<HorseLoungerVaulters>();
        }
    }
}
