using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Business.Logic.Import
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