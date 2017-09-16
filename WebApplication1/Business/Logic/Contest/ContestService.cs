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
using DocumentFormat.OpenXml.Drawing.Diagrams;
using WebApplication1.Classes;
using WebApplication1.Models;

namespace WebApplication1.Business.Logic.Contest
{
    public class ContestService
    {
        private static Models.Contest _contest;
        private static Dictionary<string, int> _stepsJudges = null;

        public static Models.Contest GetInstance()
        {
            if (_contest != null)
                return _contest;

            return GetAllDataFromDataBase();
        }

        public static Dictionary<string,int> GetJudgesPerStep()
        {

            if (_stepsJudges != null)
                return _stepsJudges;

            _stepsJudges = new Dictionary<string, int>();

            foreach (var startListClassStep in GetInstance().StartListClassStep)
            {
                //var judgeTables = startListClassStep.JudgeTables;
                foreach (var carriage in startListClassStep.StartList)
                {
                    if (carriage.IsTeam && carriage.VaultingTeam != null)
                    {
                        var vaultingClass = carriage.VaultingTeam.VaultingClass;
                        if (vaultingClass == null)
                            continue;

                        var testNumber = carriage.TeamTestnumber;
                        //var step = ExcelPreCompetitionData.GetCompetitionStep(vaultingClass, testNumber);
                        //var momentName = step?.Name;
                        string classNr = vaultingClass.ClassNr.ToString();
                        string testNumberString = testNumber.ToString();

                        AddToStepsJudgesList(classNr, testNumberString, startListClassStep);
                    }
                    else
                    {
                        foreach (var vaulter in carriage.Vaulters)
                        {
                            string classNr = vaulter.Participant.VaultingClass.ClassNr.ToString();
                            string testNumberString = vaulter.Testnumber.ToString();
                            AddToStepsJudgesList(classNr, testNumberString, startListClassStep);
                        }
                    }
                }
                
            }

            return _stepsJudges;
        }

        private static void AddToStepsJudgesList(string classNr, string testNumberString, StartListClassStep startListClassStep)
        {
            var key = classNr + "_" + testNumberString;
            if (_stepsJudges.ContainsKey(key) || startListClassStep == null)
                return;

            _stepsJudges[key] = startListClassStep.StartListClassStepId;
        }

        private static Models.Contest GetAllDataFromDataBase()
        {
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

        public static Models.Contest GetNewDataFromDatabase()
        {
            return GetAllDataFromDataBase();
        }

        
        public static string GetVaulterAndClassId(Vaulter participant)
        {
            return "id_" + participant.VaultingClass?.CompetitionClassId + "_" + participant.VaulterId;
        }

        public static string GetTeamAndClassId(Team team)
        {
            return "id_" + team.VaultingClass?.CompetitionClassId + "_" +
                   team.TeamId;
        }

        
    }
}

