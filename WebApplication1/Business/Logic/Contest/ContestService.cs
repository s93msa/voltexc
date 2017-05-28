//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using ClosedXML.Excel;
//using WebApplication1.Classes;
//using WebApplication1.Models;

using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using AutoMapper;
using WebApplication1.Classes;
using WebApplication1.Models;

namespace WebApplication1.Business.Logic.Contest
{
    public class ContestService
    {
        private static Models.Contest _contest;

        public static Models.Contest GetInstance()
        {
            if (_contest != null)
                return _contest;

            using (var db = new VaultingContext())
            {

                var contests = db.Contests;

                var contest = contests.ToList()[0];



                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                var jsonString = jsonSerializer.Serialize(contest);
                    // För att ladda alla multipla nestlade entiteter som annars skulle lazy loadas och vara utanför scopet när databas connectionen stängs

                _contest = jsonSerializer.Deserialize<Models.Contest>(jsonString);


            }
            return _contest;

        }
    }
}

