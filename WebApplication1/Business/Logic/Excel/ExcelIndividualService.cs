using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using WebApplication1.Models;

namespace WebApplication1.Business.Logic.Excel
{
    public class ExcelIndividualService
    {
        private string _eventLocation;
        private StartListClassStep _startListClassStep;
        
        private Vaulter _vaulter;
        private string _vaulterName;
        private string _country;
        private string _armNumber;
        private int _startVaulterNumber;
        private CompetitionClass _vaulterClass;
        private int _testNumber;
        private Step _step ;
        private string _excelWorksheetNameJudgesTableA;
        private string _excelWorksheetNameJudgesTableB;
        private string _excelWorksheetNameJudgesTableC;
        private string _excelWorksheetNameJudgesTableD;
        private string _momentName;
        private string _vaultingClubName;
        private Horse _horse;
        private string _horseName;
        private string _lungerName;
        private JudgeTable _judgeTableA ;
        private JudgeTable _judgeTableB;
        private JudgeTable _judgeTableC;
        private JudgeTable _judgeTableD;
        //var excelWorksheetNameJudgesTableA = step?.ExcelWorksheetNameJudgesTableA;
        //var excelWorksheetNameJudgesTableB = step?.ExcelWorksheetNameJudgesTableB;
        //var excelWorksheetNameJudgesTableC = step?.ExcelWorksheetNameJudgesTableC;
        //var excelWorksheetNameJudgesTableD = step?.ExcelWorksheetNameJudgesTableD;
        private string _inputFileName ;
        private XLWorkbook _workbook;

        public ExcelIndividualService(Models.Contest contest, StartListClassStep startListClassStep,
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
            _inputFileName = _vaulterClass.Excelfile;
            _workbook = new XLWorkbook(_inputFileName);
            //var vaulter = horseOrder.Participant;
        }

        public void CreateExcelforIndividual()
        {
            if (_vaulterClass.ClassNr == 4)
            {
                CreateExcelForClass4();

            }
            else if (_vaulterClass.ClassNr == 5)
            {
                CreateExcelForClass5();
            }

            
            // worksheetHorse.Cell(7, "d").Value = d?.Count;

            //}
        }

        private void CreateExcelForClass4()
        {

            //var excelWorksheetNameJudgesTableA = step?.ExcelWorksheetNameJudgesTableA;
            //var excelWorksheetNameJudgesTableB = step?.ExcelWorksheetNameJudgesTableB;
            //var excelWorksheetNameJudgesTableC = step?.ExcelWorksheetNameJudgesTableC;
            //var excelWorksheetNameJudgesTableD = step?.ExcelWorksheetNameJudgesTableD;
            //string testNumber = Convert.ToString(step?.TestNumber) + step?.Name;

            var worksheet = _workbook.Worksheets.Worksheet(_excelWorksheetNameJudgesTableA?.Trim());

            SetWorksheetHorse(worksheet);

            ShowOnlyWorksheet(worksheet);
            var fileOutputname = GetOutputFilename(_judgeTableA);
            SaveExcelFile(fileOutputname);


            worksheet = _workbook.Worksheets.Worksheet(_excelWorksheetNameJudgesTableB?.Trim());

            SetWorksheetIndividuellJuniorGrund2(worksheet, _judgeTableB);

            ShowOnlyWorksheet(worksheet);
            fileOutputname = GetOutputFilename(_judgeTableB);
            SaveExcelFile(fileOutputname);


            worksheet = _workbook.Worksheets.Worksheet(_excelWorksheetNameJudgesTableC?.Trim());
            SetWorksheetIndividuellJuniorGrund2(worksheet, _judgeTableC);

            ShowOnlyWorksheet(worksheet);
            fileOutputname = GetOutputFilename(_judgeTableC);
            SaveExcelFile(fileOutputname);



            worksheet = _workbook.Worksheets.Worksheet(_excelWorksheetNameJudgesTableD?.Trim());
            SetWorksheetIndividuellJuniorGrund2(worksheet, _judgeTableD);

            ShowOnlyWorksheet( worksheet);
            fileOutputname = GetOutputFilename(_judgeTableD);
            SaveExcelFile(fileOutputname);
        }

        //Individuella miniorer
        private  void CreateExcelForClass5()
        {

            //var excelWorksheetNameJudgesTableA = step?.ExcelWorksheetNameJudgesTableA;
            //var excelWorksheetNameJudgesTableB = step?.ExcelWorksheetNameJudgesTableB;
            //var excelWorksheetNameJudgesTableC = step?.ExcelWorksheetNameJudgesTableC;
            //var excelWorksheetNameJudgesTableD = step?.ExcelWorksheetNameJudgesTableD;

           //string testNumber = Convert.ToString(step?.TestNumber) + step?.Name;

            var worksheet = _workbook.Worksheets.Worksheet(_excelWorksheetNameJudgesTableA?.Trim());

            SetWorksheetHorse(worksheet);

            ShowOnlyWorksheet( worksheet);
            var fileOutputname = GetOutputFilename(_judgeTableA);
            SaveExcelFile(fileOutputname);


            worksheet = _workbook.Worksheets.Worksheet(_excelWorksheetNameJudgesTableB?.Trim());

            SetWorksheetIndividuellMiniorGrund1(worksheet,  _judgeTableB);

            ShowOnlyWorksheet(worksheet);
            fileOutputname = GetOutputFilename(_judgeTableB);
            SaveExcelFile(fileOutputname);


            worksheet = _workbook.Worksheets.Worksheet(_excelWorksheetNameJudgesTableC?.Trim());
            SetWorksheetIndividuellMiniorGrund1(worksheet, _judgeTableC);

            ShowOnlyWorksheet(worksheet);
            fileOutputname = GetOutputFilename(_judgeTableC);
            SaveExcelFile(fileOutputname);



            worksheet = _workbook.Worksheets.Worksheet(_excelWorksheetNameJudgesTableD?.Trim());
            SetWorksheetIndividuellMiniorGrund1(worksheet, _judgeTableD);

            ShowOnlyWorksheet(worksheet);
            fileOutputname = GetOutputFilename(_judgeTableD);
            SaveExcelFile(fileOutputname);
        }

       

        private void ShowOnlyWorksheet( IXLWorksheet worksheet)
        {
            IXLWorksheets workbookWorksheets = _workbook?.Worksheets;
            if (workbookWorksheets == null)
                return;

            foreach (var currrentWorksheet in workbookWorksheets)
            {
                if (currrentWorksheet == worksheet)
                {
                    currrentWorksheet.Visibility = XLWorksheetVisibility.Visible;
                    currrentWorksheet.TabActive = true;
                }
                else
                    currrentWorksheet.Hide(); //.Visibility = XLWorksheetVisibility.Hidden;
            }
        }

        private void SetWorksheetHorse(IXLWorksheet worksheet)
        {
            const string tableName = "A";

            //var eventLocation = _contest?.Location;
            //var vaulterName = Vaulter.Name;
            //var vaulterClass = vau.VaultingClass;
            //var armNumber = participant.Armband;

            SetValueInWorksheet(worksheet, 3, "c", GetStepDate());
            SetValueInWorksheet(worksheet, 4, "c", _eventLocation);
            SetValueInWorksheet(worksheet, 5, "c", _vaulterName);
            SetValueInWorksheet(worksheet, 6, "c", _vaultingClubName);
            SetValueInWorksheet(worksheet, 7, "c", _country);
            SetValueInWorksheet(worksheet, 8, "c", _horseName);
            SetValueInWorksheet(worksheet, 9, "c", _lungerName);

            SetValueInWorksheet(worksheet, 1, "l", GetStartNumberForVaulterString());
            SetValueInWorksheet(worksheet, 2, "l", tableName);
            SetValueInWorksheet(worksheet, 3, "l", _vaulterClass.ClassNr + " (" + _vaulterClass.ClassName + ")");
            SetValueInWorksheet(worksheet, 4, "l", _momentName);
            SetValueInWorksheet(worksheet, 6, "l", _armNumber);

            SetValueInWorksheet(worksheet, 29, "c", GetJudgeName(_judgeTableA));


        }

        private void SetWorksheetIndividuellMiniorGrund1(IXLWorksheet worksheet, JudgeTable judgeTable)
        {

            //var country = contest?.Country;
            //var eventLocation = contest?.Location;
            //var vaulterName = participant.Name;
            //var vaulterClass = participant.VaultingClass;
            //var armNumber = participant.Armband;

            SetValueInWorksheet(worksheet, 4, "c", GetStepDate());
            SetValueInWorksheet(worksheet, 5, "c", _eventLocation);
            SetValueInWorksheet(worksheet, 6, "c", _vaulterName);
            SetValueInWorksheet(worksheet, 7, "c", _vaultingClubName);
            SetValueInWorksheet(worksheet, 8, "c", _country);
            SetValueInWorksheet(worksheet, 9, "c", _horseName);
            SetValueInWorksheet(worksheet, 10, "c", _lungerName);

            SetValueInWorksheet(worksheet, 2, "l", GetStartNumberForVaulterString());
            SetValueInWorksheet(worksheet, 3, "l", GetJudgeTableName(judgeTable));
            SetValueInWorksheet(worksheet, 4, "l", _vaulterClass.ClassNr + " (" + _vaulterClass.ClassName + ")");
            SetValueInWorksheet(worksheet, 5, "l", _momentName);
            SetValueInWorksheet(worksheet, 7, "l", _armNumber);

            SetValueInWorksheet(worksheet, 32, "c", GetJudgeName(judgeTable));


        }

        private void SetWorksheetIndividuellJuniorGrund2(IXLWorksheet worksheet, JudgeTable judgeTable)
        {

            //var country = contest?.Country;
            //var eventLocation = contest?.Location;
            //var vaulterName = participant.Name;
            //var vaulterClass = participant.VaultingClass;
            //var armNumber = participant.Armband;

            SetValueInWorksheet(worksheet, 4, "c", GetStepDate());
            SetValueInWorksheet(worksheet, 5, "c", _eventLocation);
            SetValueInWorksheet(worksheet, 6, "c", _vaulterName);
            SetValueInWorksheet(worksheet, 7, "c", _vaultingClubName);
            SetValueInWorksheet(worksheet, 8, "c", _country);
            SetValueInWorksheet(worksheet, 9, "c", _horseName);
            SetValueInWorksheet(worksheet, 10, "c", _lungerName);

            SetValueInWorksheet(worksheet, 2, "l", GetStartNumberForVaulterString());
            SetValueInWorksheet(worksheet, 3, "l", GetJudgeTableName(judgeTable));
            SetValueInWorksheet(worksheet, 4, "l", _vaulterClass.ClassNr + " (" + _vaulterClass.ClassName + ")");
            SetValueInWorksheet(worksheet, 5, "l", _momentName);
            SetValueInWorksheet(worksheet, 7, "l", _armNumber);

            SetValueInWorksheet(worksheet, 32, "c", GetJudgeName(judgeTable));


        }

       

        private void SetWorksheetIndkürtekn2_3(IXLWorksheet worksheet, JudgeTable judgeTable)
        {

            //var country = contest?.Country;
            //var eventLocation = contest?.Location;
            //var vaulterName = participant.Name;
            //var vaulterClass = participant.VaultingClass;
            //var armNumber = participant.Armband;

            SetValueInWorksheet(worksheet, 4, "c", GetStepDate());
            SetValueInWorksheet(worksheet, 5, "c", _eventLocation);
            SetValueInWorksheet(worksheet, 6, "c", _vaulterName);
            SetValueInWorksheet(worksheet, 7, "c", _vaultingClubName);
            SetValueInWorksheet(worksheet, 8, "c", _country);
            SetValueInWorksheet(worksheet, 9, "c", _horseName);
            SetValueInWorksheet(worksheet, 10, "c", _lungerName);

            SetValueInWorksheet(worksheet, 2, "l", _startVaulterNumber.ToString());
            SetValueInWorksheet(worksheet, 3, "l", GetJudgeTableName(judgeTable));
            SetValueInWorksheet(worksheet, 4, "l", _vaulterClass.ClassNr + " (" + _vaulterClass.ClassName + ")");
            SetValueInWorksheet(worksheet, 5, "l", _momentName);
            SetValueInWorksheet(worksheet, 7, "l", _armNumber);

            SetValueInWorksheet(worksheet, 37, "c", judgeTable?.JudgeName);


        }



       

        

        private static void SetValueInWorksheet(IXLWorksheet worksheet, int row, string column, string value)
        {
            worksheet.Cell(row, column).Value = value;
        }

        private static void SetValueInWorksheet(IXLWorksheet worksheet, string cellName, string value)
        {
            var linkTocell = worksheet.Workbook.NamedRange(cellName);
           //var g = GetNamedCell(worksheet, cellName);
            worksheet.Cell(linkTocell.RefersTo.Split('!')[1]).Value = value;
            //          worksheet.Workbook.Range(cellName).Cells();
            //= value;
        }

        private void SaveExcelFile(string outputFileName)
        {
            string fileoutputname = @"C:\Temp\Test_Voligemallar\output\" + outputFileName + ".xlsx";
            _workbook.SaveAs(fileoutputname);
        }

        private string GetOutputFilename(JudgeTable judgeTabel)
        {
            return _startListClassStep.Date.ToShortDateString() + @"\" + judgeTabel.JudgeTableName + @"\" + _startListClassStep.Name + @"\" + _vaulter.Name + ".xlsx";
        }

        public static IXLCell GetNamedCell(IXLWorksheet worksheet, string namedCell)
        {
            IXLNamedRange xlNamedRange = worksheet.NamedRange(namedCell);
            if (xlNamedRange == null)
                return (IXLCell)null;
            IXLRange xlRange = xlNamedRange.Ranges.FirstOrDefault<IXLRange>();
            if (xlRange == null)
                return (IXLCell)null;
            return xlRange.FirstCell();
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

        private string GetStartNumberForVaulterString()
        {
            return _startVaulterNumber.ToString();
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

        private string GetJudgeName(JudgeTable judgeTable)
        {
            return judgeTable?.JudgeName;
        }

        private string GetStepDate()
        {
                return _startListClassStep.Date.ToShortDateString();
        }

        private static string GetJudgeTableName(JudgeTable judgeTable)
        {
            return judgeTable?.JudgeTableName.ToString();
        }
    }
}