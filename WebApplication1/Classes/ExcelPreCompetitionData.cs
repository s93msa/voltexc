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
        public string _eventLocation;
        public StartListClassStep _startListClassStep;

        public Vaulter _vaulter;
        public string _vaulterName;
        public string _country;
        public string _armNumber;
        public int _startVaulterNumber;
        public CompetitionClass _vaulterClass;
        public int _testNumber;
        public Step _step;
        public string _excelWorksheetNameJudgesTableA;
        public string _excelWorksheetNameJudgesTableB;
        public string _excelWorksheetNameJudgesTableC;
        public string _excelWorksheetNameJudgesTableD;
        public string _momentName;
        public string _vaultingClubName;
        public Horse _horse;
        public string _horseName;
        public string _lungerName;
        public JudgeTable _judgeTableA;
        public JudgeTable _judgeTableB;
        public JudgeTable _judgeTableC;
        public JudgeTable _judgeTableD;
        //var excelWorksheetNameJudgesTableA = step?.ExcelWorksheetNameJudgesTableA;
        //var excelWorksheetNameJudgesTableB = step?.ExcelWorksheetNameJudgesTableB;
        //var excelWorksheetNameJudgesTableC = step?.ExcelWorksheetNameJudgesTableC;
        //var excelWorksheetNameJudgesTableD = step?.ExcelWorksheetNameJudgesTableD;
        public string _inputFileName;
        public XLWorkbook _workbook;

        public ExcelPreCompetitionData(Models.Contest contest, StartListClassStep startListClassStep,
             HorseOrder horseOrder, int startVaulterNumber, VaulterOrder vaulterOrder)
        {
            var contest1 = contest;
            _eventLocation = contest1?.Location;

            _startListClassStep = startListClassStep;
            _startVaulterNumber = startVaulterNumber;
            _vaulter = vaulterOrder.Participant;
            _vaulterName = _vaulter?.Name;
            _country = contest1?.Country; //TODO: country ska hämtas från klubben eller voltigören inte tävlingen
            _armNumber = _vaulter?.Armband;
            _vaulterClass = _vaulter?.VaultingClass;

            _testNumber = vaulterOrder.Testnumber;
            _step = GetCompetitionStep(_vaulterClass, _testNumber);
            _momentName = _step?.Name + " " + Convert.ToString(_step?.TestNumber);
            _excelWorksheetNameJudgesTableA = _step?.ExcelWorksheetNameJudgesTableA;
            _excelWorksheetNameJudgesTableB = _step?.ExcelWorksheetNameJudgesTableB;
            _excelWorksheetNameJudgesTableC = _step?.ExcelWorksheetNameJudgesTableC;
            _excelWorksheetNameJudgesTableD = _step?.ExcelWorksheetNameJudgesTableD;
            _vaultingClubName = _vaulter?.VaultingClub?.ClubName.Trim();
            _horse = horseOrder.HorseInformation;
            _horseName = _horse?.HorseName?.Trim();
            _lungerName = _horse?.Lunger?.LungerName?.Trim();
            _judgeTableA = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.A);
            _judgeTableB = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.B);
            _judgeTableC = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.C);
            _judgeTableD = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.D);
            //var excelWorksheetNameJudgesTableA = step?.ExcelWorksheetNameJudgesTableA;
            //var excelWorksheetNameJudgesTableB = step?.ExcelWorksheetNameJudgesTableB;
            //var excelWorksheetNameJudgesTableC = step?.ExcelWorksheetNameJudgesTableC;
            //var excelWorksheetNameJudgesTableD = step?.ExcelWorksheetNameJudgesTableD;
            _inputFileName = _vaulterClass?.Excelfile;
            _workbook = new XLWorkbook(_inputFileName);
            //var vaulter = horseOrder.Participant;
        }
        public string GetStepDate()
        {
            return _startListClassStep.Date.ToShortDateString();
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