using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using WebApplication1.Models;

namespace WebApplication1.Classes
{

    public class ExcelPreCompetitionData
    {
        public string EventLocation { get; }
        public StartListClassStep ListClassStep { get; }


        public Vaulter Vaulter1 { get; }
        public string VaulterName { get; }
        public string ArmNumber { get; }

        
        public string TeamName { get; }

        public Team  Team1{ get; }


        public string Country { get; }
       
        public int StartVaulterNumber { get; }
        public CompetitionClass VaultingClass { get; }
        public int TestNumber { get; }
        public Step Step1 { get; }
        public string ExcelWorksheetNameJudgesTableA { get; }
        public string ExcelWorksheetNameJudgesTableB { get; }
        public string ExcelWorksheetNameJudgesTableC { get; }
        public string ExcelWorksheetNameJudgesTableD { get; }
        public string MomentName { get; }
        public StepType MomentType { get; }
        public string VaultingClubName { get; }
        public Horse Horse1 { get; }
        public string HorseName { get; }
        public string LungerName { get; }
        public JudgeTable JudgeTableA { get; }
        public JudgeTable JudgeTableB { get; }
        public JudgeTable JudgeTableC { get; }
        public JudgeTable JudgeTableD { get; }
        //var excelWorksheetNameJudgesTableA = step?.ExcelWorksheetNameJudgesTableA;
        //var excelWorksheetNameJudgesTableB = step?.ExcelWorksheetNameJudgesTableB;
        //var excelWorksheetNameJudgesTableC = step?.ExcelWorksheetNameJudgesTableC;
        //var excelWorksheetNameJudgesTableD = step?.ExcelWorksheetNameJudgesTableD;
        public string InputFileName { get; }
        public XLWorkbook Workbook { get; }

      


        private SortedList<int, Vaulter> _vaultersList = null;
        private Team _team = null;
        public ExcelPreCompetitionData(Models.Contest contest, StartListClassStep startListClassStep,
             HorseOrder horseOrder, int startVaulterNumber, VaulterOrder vaulterOrder)
        {
            var contest1 = contest;
            EventLocation = contest1?.Location;

            ListClassStep = startListClassStep;
            StartVaulterNumber = startVaulterNumber;
            Vaulter1 = vaulterOrder.Participant;
            VaulterName = Vaulter1?.Name?.Trim();
            Country = contest1?.Country; //TODO: country ska hämtas från klubben eller voltigören inte tävlingen
            ArmNumber = Vaulter1?.Armband;
            VaultingClass = Vaulter1?.VaultingClass;

            TestNumber = vaulterOrder.Testnumber;
            Step1 = GetCompetitionStep(contest.TypeOfContest, VaultingClass, TestNumber);
            MomentName = Step1?.Name;
            MomentType = Step1?.TypeOfStep;
            ExcelWorksheetNameJudgesTableA = Step1?.ExcelWorksheetNameJudgesTableA;
            ExcelWorksheetNameJudgesTableB = Step1?.ExcelWorksheetNameJudgesTableB;
            ExcelWorksheetNameJudgesTableC = Step1?.ExcelWorksheetNameJudgesTableC;
            ExcelWorksheetNameJudgesTableD = Step1?.ExcelWorksheetNameJudgesTableD;
            VaultingClubName = Vaulter1?.VaultingClub?.ClubName.Trim();
            Horse1 = horseOrder.HorseInformation;
            HorseName = Horse1?.HorseName?.Trim();
            LungerName = Horse1?.Lunger?.LungerName?.Trim();
            JudgeTableA = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.A);
            JudgeTableB = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.B);
            JudgeTableC = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.C);
            JudgeTableD = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.D);
            //var excelWorksheetNameJudgesTableA = step?.ExcelWorksheetNameJudgesTableA;
            //var excelWorksheetNameJudgesTableB = step?.ExcelWorksheetNameJudgesTableB;
            //var excelWorksheetNameJudgesTableC = step?.ExcelWorksheetNameJudgesTableC;
            //var excelWorksheetNameJudgesTableD = step?.ExcelWorksheetNameJudgesTableD;
            InputFileName = Step1.OverrideExcelfileName ?? VaultingClass?.Excelfile;
            var workingdirectory = HttpContext.Current.Server.MapPath("~");
            Workbook = new XLWorkbook(workingdirectory + InputFileName);
            //var vaulter = horseOrder.Participant;
        }

        public ExcelPreCompetitionData(Models.Contest contest, StartListClassStep startListClassStep,
             HorseOrder horseOrder, int startVaulterNumber, Team team)
        {
            var contest1 = contest;
            EventLocation = contest1?.Location;

            ListClassStep = startListClassStep;
            StartVaulterNumber = startVaulterNumber;
           // Vaulter1 = vaulterOrder.Participant;
            _team = team;
            TeamName = team?.Name?.Trim();
            Team1 = team;
            Country = contest1?.Country; //TODO: country ska hämtas från klubben eller voltigören inte tävlingen
            //ArmNumber = Vaulter1?.Armband;
            VaultingClass = team?.VaultingClass;
            
            TestNumber = horseOrder.TeamTestnumber;
            Step1 = GetCompetitionStep(contest.TypeOfContest, VaultingClass, TestNumber);
            MomentName = Step1?.Name;
            MomentType = Step1?.TypeOfStep;
            ExcelWorksheetNameJudgesTableA = Step1?.ExcelWorksheetNameJudgesTableA;
            ExcelWorksheetNameJudgesTableB = Step1?.ExcelWorksheetNameJudgesTableB;
            ExcelWorksheetNameJudgesTableC = Step1?.ExcelWorksheetNameJudgesTableC;
            ExcelWorksheetNameJudgesTableD = Step1?.ExcelWorksheetNameJudgesTableD;
            VaultingClubName = team?.VaultingClub?.ClubName.Trim();
            Horse1 = horseOrder.HorseInformation;
            HorseName = Horse1?.HorseName?.Trim();
            LungerName = Horse1?.Lunger?.LungerName?.Trim();
            JudgeTableA = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.A);
            JudgeTableB = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.B);
            JudgeTableC = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.C);
            JudgeTableD = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.D);
            //var excelWorksheetNameJudgesTableA = step?.ExcelWorksheetNameJudgesTableA;
            //var excelWorksheetNameJudgesTableB = step?.ExcelWorksheetNameJudgesTableB;
            //var excelWorksheetNameJudgesTableC = step?.ExcelWorksheetNameJudgesTableC;
            //var excelWorksheetNameJudgesTableD = step?.ExcelWorksheetNameJudgesTableD;
            InputFileName = Step1.OverrideExcelfileName ?? VaultingClass?.Excelfile;
            var workingdirectory = HttpContext.Current.Server.MapPath("~");
            Workbook = new XLWorkbook(workingdirectory + InputFileName);
            //WorkbookOverrideA = GetOverrideExcelfile(workingdirectory, Step1.OverrideExcelfileA);
            //WorkbookOverrideB = GetOverrideExcelfile(workingdirectory, Step1.OverrideExcelfileA);
            //WorkbookOverrideC = GetOverrideExcelfile(workingdirectory, Step1.OverrideExcelfileA);
            //WorkbookOverrideD = GetOverrideExcelfile(workingdirectory, Step1.OverrideExcelfileA);


            //var vaulter = horseOrder.Participant;
        }

        

        public SortedList<int, Vaulter> GetTeamVaultersSorted()
        {
            if (_vaultersList != null)
                return _vaultersList;

            _vaultersList = new SortedList<int, Vaulter>();

            foreach (var vaulter in _team.VaultersList)
            {
                _vaultersList.Add(vaulter.StartNumber, vaulter.Participant);
            }

            return _vaultersList;
        }

        public string GetName()
        {
            return TeamName?? VaulterName;
        }
        public string GetStepDate()
        {
            return ListClassStep.Date.ToShortDateString();
        }
        public static Step GetCompetitionStep(ContestType contestType, CompetitionClass vaulterClass, int testNumber)
        {
            foreach (var step in vaulterClass.GetCompetitionSteps(contestType))
            {
                if (testNumber == step.TestNumber)
                    return step;
            }

            return null;
        }

        //public XLWorkbook GetWorkbook()
        //{
        //    var excelfile = Step1.OverrideExcelfileName;
        //    if (string.IsNullOrEmpty(excelfile))
        //        return Workbook;

        //    return excelfile;
        //}

        private static JudgeTable GetJudge(List<JudgeTable> judgeTables, JudgeTableNames tableName)
        {
            foreach (var table in judgeTables)
            {
                if (table.JudgeTableName == tableName)
                    return table;
            }
            return null;
        }
        //private XLWorkbook GetOverrideExcelfile(string workingdirectory, string overrideExcelfile)
        //{
        //    if (string.IsNullOrWhiteSpace(overrideExcelfile))
        //        return new XLWorkbook(workingdirectory + overrideExcelfile);
        //    return null;
        //}

    }


}