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
        public string Country { get; }
        public string ArmNumber { get; }
        public int StartVaulterNumber { get; }
        public CompetitionClass VaulterClass { get; }
        public int TestNumber { get; }
        public Step Step1 { get; }
        public string ExcelWorksheetNameJudgesTableA { get; }
        public string ExcelWorksheetNameJudgesTableB { get; }
        public string ExcelWorksheetNameJudgesTableC { get; }
        public string ExcelWorksheetNameJudgesTableD { get; }
        public string MomentName { get; }
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

        public ExcelPreCompetitionData(Models.Contest contest, StartListClassStep startListClassStep,
             HorseOrder horseOrder, int startVaulterNumber, VaulterOrder vaulterOrder)
        {
            var contest1 = contest;
            EventLocation = contest1?.Location;

            ListClassStep = startListClassStep;
            StartVaulterNumber = startVaulterNumber;
            Vaulter1 = vaulterOrder.Participant;
            VaulterName = Vaulter1?.Name;
            Country = contest1?.Country; //TODO: country ska hämtas från klubben eller voltigören inte tävlingen
            ArmNumber = Vaulter1?.Armband;
            VaulterClass = Vaulter1?.VaultingClass;

            TestNumber = vaulterOrder.Testnumber;
            Step1 = GetCompetitionStep(VaulterClass, TestNumber);
            MomentName = Step1?.Name + " " + Convert.ToString(Step1?.TestNumber);
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
            InputFileName = VaulterClass?.Excelfile;
            Workbook = new XLWorkbook(InputFileName);
            //var vaulter = horseOrder.Participant;
        }
        public string GetStepDate()
        {
            return ListClassStep.Date.ToShortDateString();
        }
        private static Step GetCompetitionStep(CompetitionClass vaulterClass, int testNumber)
        {
            foreach (var step in vaulterClass.Steps)
            {
                if (testNumber == step.TestNumber)
                    return step;
            }

            return null;
        }

        private static JudgeTable GetJudge(List<JudgeTable> judgeTables, JudgeTableNames tableName)
        {
            foreach (var table in judgeTables)
            {
                if (table.JudgeTableName == tableName)
                    return table;
            }
            return null;
        }

       
    }


}