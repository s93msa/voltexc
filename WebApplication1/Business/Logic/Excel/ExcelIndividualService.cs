using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using WebApplication1.Classes;
using WebApplication1.Models;

namespace WebApplication1.Business.Logic.Excel
{
    public class ExcelIndividualService : ExcelBaseService
    {
        private readonly ExcelPreCompetitionData _competitionData;
        //private string _eventLocation;
        //private StartListClassStep _startListClassStep;

        //private Vaulter _vaulter;
        //private string _vaulterName;
        //private string _country;
        //private string _armNumber;
        //private int _startVaulterNumber;
        //private CompetitionClass _vaulterClass;
        //private int _testNumber;
        //private Step _step ;
        //private string _excelWorksheetNameJudgesTableA;
        //private string _excelWorksheetNameJudgesTableB;
        //private string _excelWorksheetNameJudgesTableC;
        //private string _excelWorksheetNameJudgesTableD;
        //private string _momentName;
        //private string _vaultingClubName;
        //private Horse _horse;
        //private string _horseName;
        //private string _lungerName;
        //private JudgeTable _judgeTableA ;
        //private JudgeTable _judgeTableB;
        //private JudgeTable _judgeTableC;
        //private JudgeTable _judgeTableD;
        ////var excelWorksheetNameJudgesTableA = step?.ExcelWorksheetNameJudgesTableA;
        ////var excelWorksheetNameJudgesTableB = step?.ExcelWorksheetNameJudgesTableB;
        ////var excelWorksheetNameJudgesTableC = step?.ExcelWorksheetNameJudgesTableC;
        ////var excelWorksheetNameJudgesTableD = step?.ExcelWorksheetNameJudgesTableD;
        //private string _inputFileName ;
        //private XLWorkbook _workbook;

        //public ExcelIndividualService(Models.Contest contest, StartListClassStep startListClassStep,
        //     HorseOrder horseOrder, int startVaulterNumber, VaulterOrder vaulterOrder)
        //{
        //    var contest1 = contest;
        //    _eventLocation = contest1?.Location;

        //_startListClassStep = startListClassStep;
        //    _startVaulterNumber = startVaulterNumber;
        //    _vaulter = vaulterOrder.Participant;
        //    _vaulterName = _vaulter?.Name;
        //    _country = contest1?.Country; //TODO: country ska hämtas från klubben eller voltigören inte tävlingen
        //    _armNumber = _vaulter?.Armband;
        //    _vaulterClass = _vaulter?.VaultingClass;

        //    _testNumber = vaulterOrder.Testnumber;
        //    _step = GetCompetitionStep(_vaulterClass, _testNumber);
        //    _momentName = _step?.Name + " " + Convert.ToString(_step?.TestNumber);
        //    _excelWorksheetNameJudgesTableA = _step?.ExcelWorksheetNameJudgesTableA;
        //    _excelWorksheetNameJudgesTableB = _step?.ExcelWorksheetNameJudgesTableB;
        //    _excelWorksheetNameJudgesTableC = _step?.ExcelWorksheetNameJudgesTableC;
        //    _excelWorksheetNameJudgesTableD = _step?.ExcelWorksheetNameJudgesTableD;
        //    _vaultingClubName = _vaulter?.VaultingClub?.ClubName.Trim();
        //    _horse = horseOrder.HorseInformation;
        //    _horseName = _horse?.HorseName?.Trim();
        //    _lungerName = _horse?.Lunger?.LungerName?.Trim();
        //    _judgeTableA = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.A);
        //    _judgeTableB = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.B);
        //    _judgeTableC = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.C);
        //    _judgeTableD = GetJudge(startListClassStep.JudgeTables, JudgeTableNames.D);
        //    //var excelWorksheetNameJudgesTableA = step?.ExcelWorksheetNameJudgesTableA;
        //    //var excelWorksheetNameJudgesTableB = step?.ExcelWorksheetNameJudgesTableB;
        //    //var excelWorksheetNameJudgesTableC = step?.ExcelWorksheetNameJudgesTableC;
        //    //var excelWorksheetNameJudgesTableD = step?.ExcelWorksheetNameJudgesTableD;
        //    _inputFileName = _vaulterClass?.Excelfile;
        //    _workbook = new XLWorkbook(_inputFileName);
        //    //var vaulter = horseOrder.Participant;
        //}
        public ExcelIndividualService(ExcelPreCompetitionData competitionInformation) : base(competitionInformation)
        {
            _competitionData = competitionInformation;
 
        }

        public void CreateExcelforIndividual()
        {
            CreateExcelFromValuesJudgeA();
            CreateExcelFromValuesJudgeB();
            CreateExcelFromValuesJudgeC();
            CreateExcelFromValuesJudgeD();
            //if (_competitionData.VaulterClass.ClassNr == 3)
            //{
            //    CreateExcelForClass3();

            //}
            //else if (_competitionData.VaulterClass.ClassNr == 4)
            //{
            //    CreateExcelForClass4();

            //}
            //else if (_competitionData.VaulterClass.ClassNr == 5)
            //{
            //    CreateExcelForClass5();
            //}
        }

        //private void CreateExcelForClass3()
        //{
        //    CreateExcelFromValuesJudgeA(SetWorksheetHorse);
        //    CreateExcelFromValuesJudgeB(SetWorksheetIndividuellJuniorGrund2);
        //    CreateExcelFromValuesJudgeC(SetWorksheetIndividuellJuniorGrund2);
        //    CreateExcelFromValuesJudgeD(SetWorksheetIndividuellJuniorGrund2);

        //    //var worksheet = _competitionData.Workbook.Worksheets.Worksheet(_competitionData.ExcelWorksheetNameJudgesTableA?.Trim());

        //    //SetWorksheetHorse(worksheet);

        //    //ShowOnlyWorksheet(worksheet);
        //    //var fileOutputname = GetOutputFilename(_competitionData.JudgeTableA);
        //    //SaveExcelFile(fileOutputname);


        //    //worksheet = _competitionData.Workbook.Worksheets.Worksheet(_competitionData.ExcelWorksheetNameJudgesTableB?.Trim());

        //    //SetWorksheetIndividuellJuniorGrund2(worksheet, _competitionData.JudgeTableB);

        //    //ShowOnlyWorksheet(worksheet);
        //    //fileOutputname = GetOutputFilename(_competitionData.JudgeTableB);
        //    //SaveExcelFile(fileOutputname);


        //    //worksheet = _competitionData.Workbook.Worksheets.Worksheet(_competitionData.ExcelWorksheetNameJudgesTableC?.Trim());
        //    //SetWorksheetIndividuellJuniorGrund2(worksheet, _competitionData.JudgeTableC);

        //    //ShowOnlyWorksheet(worksheet);
        //    //fileOutputname = GetOutputFilename(_competitionData.JudgeTableC);
        //    //SaveExcelFile(fileOutputname);



        //    //worksheet = _competitionData.Workbook.Worksheets.Worksheet(_competitionData.ExcelWorksheetNameJudgesTableD?.Trim());
        //    //SetWorksheetIndividuellJuniorGrund2(worksheet, _competitionData.JudgeTableD);

        //    //ShowOnlyWorksheet(worksheet);
        //    //fileOutputname = GetOutputFilename(_competitionData.JudgeTableD);
        //    //SaveExcelFile(fileOutputname);
        //}

        //private void CreateExcelForClass4()
        //{
        //    CreateExcelFromValuesJudgeA(SetWorksheetHorse);
        //    CreateExcelFromValuesJudgeB(SetWorksheetIndividuellJuniorGrund2);
        //    CreateExcelFromValuesJudgeC(SetWorksheetIndividuellJuniorGrund2);
        //    CreateExcelFromValuesJudgeD(SetWorksheetIndividuellJuniorGrund2);

        //    //var worksheet = _competitionData.Workbook.Worksheets.Worksheet(_competitionData.ExcelWorksheetNameJudgesTableA?.Trim());

        //    //SetWorksheetHorse(worksheet);

        //    //ShowOnlyWorksheet(worksheet);
        //    //var fileOutputname = GetOutputFilename(_competitionData.JudgeTableA);
        //    //SaveExcelFile(fileOutputname);


        //    //worksheet = _competitionData.Workbook.Worksheets.Worksheet(_competitionData.ExcelWorksheetNameJudgesTableB?.Trim());

        //    //SetWorksheetIndividuellJuniorGrund2(worksheet, _competitionData.JudgeTableB);

        //    //ShowOnlyWorksheet(worksheet);
        //    //fileOutputname = GetOutputFilename(_competitionData.JudgeTableB);
        //    //SaveExcelFile(fileOutputname);


        //    //worksheet = _competitionData.Workbook.Worksheets.Worksheet(_competitionData.ExcelWorksheetNameJudgesTableC?.Trim());
        //    //SetWorksheetIndividuellJuniorGrund2(worksheet, _competitionData.JudgeTableC);

        //    //ShowOnlyWorksheet(worksheet);
        //    //fileOutputname = GetOutputFilename(_competitionData.JudgeTableC);
        //    //SaveExcelFile(fileOutputname);



        //    //worksheet = _competitionData.Workbook.Worksheets.Worksheet(_competitionData.ExcelWorksheetNameJudgesTableD?.Trim());
        //    //SetWorksheetIndividuellJuniorGrund2(worksheet, _competitionData.JudgeTableD);

        //    //ShowOnlyWorksheet( worksheet);
        //    //fileOutputname = GetOutputFilename(_competitionData.JudgeTableD);
        //    //SaveExcelFile(fileOutputname);
        //}

        ////Individuella miniorer
        //private  void CreateExcelForClass5()
        //{

        //    //var excelWorksheetNameJudgesTableA = step?.ExcelWorksheetNameJudgesTableA;
        //    //var excelWorksheetNameJudgesTableB = step?.ExcelWorksheetNameJudgesTableB;
        //    //var excelWorksheetNameJudgesTableC = step?.ExcelWorksheetNameJudgesTableC;
        //    //var excelWorksheetNameJudgesTableD = step?.ExcelWorksheetNameJudgesTableD;

        //   //string testNumber = Convert.ToString(step?.TestNumber) + step?.Name;

        //    //var excelWorksheetNameJudgesTable = _competitionData.ExcelWorksheetNameJudgesTableA?.Trim();
        //    // var worksheet = _competitionData.Workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTable);
        //    //   SetWorksheetHorse(worksheet);
        //    //CreateExcelFromValues(SetWorksheetHorse, excelWorksheetNameJudgesTable, _competitionData.JudgeTableA);

        //    //ShowOnlyWorksheet( worksheet);
        //    //var fileOutputname = GetOutputFilename(_competitionData.JudgeTableA);
        //    //SaveExcelFile(fileOutputname);
        //    CreateExcelFromValuesJudgeA();
        //    CreateExcelFromValuesJudgeB();
        //    CreateExcelFromValuesJudgeC();
        //    CreateExcelFromValuesJudgeD();

        //    //excelWorksheetNameJudgesTable = _competitionData.ExcelWorksheetNameJudgesTableC?.Trim();
        //    //CreateExcelFromValues(SetWorksheetIndividuellMiniorGrund1, excelWorksheetNameJudgesTable, _competitionData.JudgeTableC);

        //    //worksheet = _competitionData.Workbook.Worksheets.Worksheet(_competitionData.ExcelWorksheetNameJudgesTableC?.Trim());
        //    //SetWorksheetIndividuellMiniorGrund1(worksheet, _competitionData.JudgeTableC);

        //    //ShowOnlyWorksheet(worksheet);
        //    //fileOutputname = GetOutputFilename(_competitionData.JudgeTableC);
        //    //SaveExcelFile(fileOutputname);

        //    //excelWorksheetNameJudgesTable = _competitionData.ExcelWorksheetNameJudgesTableD?.Trim();
        //    //CreateExcelFromValues(SetWorksheetIndividuellMiniorGrund1, excelWorksheetNameJudgesTable, _competitionData.JudgeTableD);

        //    //worksheet = _competitionData.Workbook.Worksheets.Worksheet(_competitionData.ExcelWorksheetNameJudgesTableD?.Trim());
        //    //SetWorksheetIndividuellMiniorGrund1(worksheet, _competitionData.JudgeTableD);

        //    //ShowOnlyWorksheet(worksheet);
        //    //fileOutputname = GetOutputFilename(_competitionData.JudgeTableD);
        //    //SaveExcelFile(fileOutputname);
        //}

        private void CreateExcelFromValuesJudgeA()
        {
            var excelWorksheetNameJudgesTable = _competitionData.ExcelWorksheetNameJudgesTableA?.Trim();
            CreateExcelFromValues(excelWorksheetNameJudgesTable, _competitionData.JudgeTableA);
        }
        private void CreateExcelFromValuesJudgeB()
        {
            var excelWorksheetNameJudgesTable = _competitionData.ExcelWorksheetNameJudgesTableB?.Trim();
            CreateExcelFromValues(excelWorksheetNameJudgesTable, _competitionData.JudgeTableB);
        }
        private void CreateExcelFromValuesJudgeC()
        {
            var excelWorksheetNameJudgesTable = _competitionData.ExcelWorksheetNameJudgesTableC?.Trim();
            CreateExcelFromValues(excelWorksheetNameJudgesTable, _competitionData.JudgeTableC);
        }

        private void CreateExcelFromValuesJudgeD()
        {
            var excelWorksheetNameJudgesTable = _competitionData.ExcelWorksheetNameJudgesTableD?.Trim();
            CreateExcelFromValues(excelWorksheetNameJudgesTable, _competitionData.JudgeTableD);
        }

        private void CreateExcelFromValues(string excelWorksheetNameJudgesTable, JudgeTable judgeTable)
        {
            var worksheet = _competitionData.Workbook.Worksheets.Worksheet(excelWorksheetNameJudgesTable);

            SetWorksheetIndividuell(worksheet, judgeTable);

            ShowOnlyWorksheet(worksheet);
            var fileOutputname = GetOutputFilename(judgeTable);
            SaveExcelFile(fileOutputname);
        }


        //private void ShowOnlyWorksheet( IXLWorksheet worksheet)
        //{
        //    IXLWorksheets workbookWorksheets = _competitionData._workbook?.Worksheets;
        //    if (workbookWorksheets == null)
        //        return;

        //    foreach (var currrentWorksheet in workbookWorksheets)
        //    {
        //        if (currrentWorksheet == worksheet)
        //        {
        //            currrentWorksheet.Visibility = XLWorksheetVisibility.Visible;
        //            currrentWorksheet.TabActive = true;
        //        }
        //        else
        //            currrentWorksheet.Hide(); //.Visibility = XLWorksheetVisibility.Hidden;
        //    }
        //}

            

        private void SetWorksheetIndividuell(IXLWorksheet worksheet, JudgeTable judgeTable)
        {
            switch (worksheet.Name)

            {
                case "Häst, individuell":
                    SetWorksheetHorse(worksheet, judgeTable);
                    break;
                case "Individuell minior grund 1":
                    SetWorksheetIndividuellMiniorGrund1(worksheet,judgeTable);
                    break;
                case "Individuell junior grund 2":
                    SetWorksheetIndividuellJuniorGrund2(worksheet, judgeTable);                    
                    break;
                case "Individuell senior grund 3":
                    SetWorksheetIndividuellSeniorGrund3(worksheet, judgeTable);
                    break;
                case "Ind kür tekn 1":
                    SetWorksheetIndkürtekn1(worksheet, judgeTable);
                    break;
                case "Ind kür tekn 2 3":
                    SetWorksheetIndkürtekn2_3(worksheet, judgeTable);
                    break;
                case "Individuell kür artistisk":
                    SetWorksheetIndKurArtistisk(worksheet, judgeTable);
                    break;
                case "Individuell tekniska övningar":
                    SetWorksheetIndTekniskaOvningar(worksheet, judgeTable);
                    break;
                case "Individuellt tekniskt artistisk":
                    SetWorksheetIndTekniskArtistisk(worksheet, judgeTable);
                    break;
                default:
                    SetWorksheetIndividuellDefault(worksheet, judgeTable);
                    break;
            }


        }

        private void SetWorksheetIndividuellDefault(IXLWorksheet worksheet, JudgeTable judgeTable)
        {

            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 2);

            SetJudgeName(worksheet, 32, judgeTable);


        }

        //private void SetWorksheetHorse(IXLWorksheet worksheet)
        //{
        //    var judgeTable = _competitionData.JudgeTableA;
        //    SetWorksheetHorse(worksheet, judgeTable);
        //}
        private void SetWorksheetHorse(IXLWorksheet worksheet, JudgeTable judgeTable)
        {
            SetFirstInformationGroup(worksheet, 3);

            SetVaulterInformation(worksheet, judgeTable, 1);
     
            SetJudgeName(worksheet, 29, judgeTable);   
        }

        

        private void SetWorksheetIndividuellMiniorGrund1(IXLWorksheet worksheet, JudgeTable judgeTable)
        {

            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 2);
            SetJudgeName(worksheet, 32, judgeTable);
        }

        private void SetWorksheetIndividuellJuniorGrund2(IXLWorksheet worksheet, JudgeTable judgeTable)
        {

            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 2);

            SetJudgeName(worksheet, 32, judgeTable);


        }

        private void SetWorksheetIndividuellSeniorGrund3(IXLWorksheet worksheet, JudgeTable judgeTable)
        {

            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 2);

            SetJudgeName(worksheet, 32, judgeTable);


        }

        private void SetWorksheetIndkürtekn1(IXLWorksheet worksheet, JudgeTable judgeTable)
        {


            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 2);

            SetJudgeName(worksheet, 37, judgeTable);
        }

        private void SetWorksheetIndkürtekn2_3(IXLWorksheet worksheet, JudgeTable judgeTable)
        {


            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 2);

            SetJudgeName(worksheet, 37, judgeTable);
        }
        private void SetWorksheetIndKurArtistisk(IXLWorksheet worksheet, JudgeTable judgeTable)
        {


            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 2);

            SetJudgeName(worksheet, 27, judgeTable);
        }
        private void SetWorksheetIndTekniskaOvningar(IXLWorksheet worksheet, JudgeTable judgeTable)
        {


            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 2);

            SetJudgeName(worksheet, 34, judgeTable);
        }

        private void SetWorksheetIndTekniskArtistisk(IXLWorksheet worksheet, JudgeTable judgeTable)
        {


            SetFirstInformationGroup(worksheet, 4);
            SetVaulterInformation(worksheet, judgeTable, 3);

            SetJudgeName(worksheet, 28, judgeTable);
        }


        private void SetVaulterInformation(IXLWorksheet worksheet, JudgeTable judgeTable, int startRow)
        {
            var startNumber = GetStartNumberForVaulterString();
            SetInformationGroup2(worksheet, judgeTable, startRow, startNumber);
        }

        //private void SetFirstInformationGroup(IXLWorksheet worksheet, int startRow)
        //{
        //    SetValueInWorksheet(worksheet, startRow, "c", _competitionData.GetStepDate());
        //    SetValueInWorksheet(worksheet, startRow + 1, "c", _competitionData._eventLocation);
        //    SetValueInWorksheet(worksheet, startRow + 2, "c", _competitionData._vaulterName);
        //    SetValueInWorksheet(worksheet, startRow + 3, "c", _competitionData._vaultingClubName);
        //    SetValueInWorksheet(worksheet, startRow + 4, "c", _competitionData._country);
        //    SetValueInWorksheet(worksheet, startRow + 5, "c", _competitionData._horseName);
        //    SetValueInWorksheet(worksheet, startRow + 6, "c", _competitionData._lungerName);
        //}

        ////private void SetVaulterInformation(IXLWorksheet worksheet, JudgeTable judgeTable, int startRow)
        ////{
        ////    string tableName = GetJudgeTableName(judgeTable);
        ////    SetVaulterInformation(worksheet, tableName, startRow);
        ////}
        //private void SetJudgeName(IXLWorksheet worksheet, int row, JudgeTable judgeTable)
        //{
        //    SetValueInWorksheet(worksheet, row, "c", GetJudgeName(judgeTable));
        //}

        //private void SetVaulterInformation(IXLWorksheet worksheet, JudgeTable judgeTable, int startRow)
        //{
        //    string tableName = GetJudgeTableName(judgeTable);
        //    SetValueInWorksheet(worksheet, startRow, "l", GetStartNumberForVaulterString());
        //    SetValueInWorksheet(worksheet, startRow + 1, "l", tableName);
        //    SetValueInWorksheet(worksheet, startRow + 2, "l", _competitionData._vaulterClass.ClassNr + " (" + _competitionData._vaulterClass.ClassName + ")");
        //    SetValueInWorksheet(worksheet, startRow + 3, "l", _competitionData._momentName);
        //    SetValueInWorksheet(worksheet, startRow + 4, "l", _competitionData._armNumber);
        //}

        //private static void SetValueInWorksheet(IXLWorksheet worksheet, int row, string column, string value)
        //{
        //    worksheet.Cell(row, column).Value = value;
        //}

        //private static void SetValueInWorksheet(IXLWorksheet worksheet, string cellName, string value)
        //{
        //    var linkTocell = worksheet.Workbook.NamedRange(cellName);
        //   //var g = GetNamedCell(worksheet, cellName);
        //    worksheet.Cell(linkTocell.RefersTo.Split('!')[1]).Value = value;
        //    //          worksheet.Workbook.Range(cellName).Cells();
        //    //= value;
        //}

        //private void SaveExcelFile(string outputFileName)
        //{
        //    if (outputFileName == null)
        //        return;

        //    string fileoutputname = @"C:\Temp\Test_Voligemallar\output\" + outputFileName + ".xlsx";
        //    _competitionData._workbook.SaveAs(fileoutputname);
        //}

        //private string GetOutputFilename(JudgeTable judgeTabel)
        //{
        //    return _competitionData._startListClassStep.Date.ToShortDateString() + @"\" + judgeTabel.JudgeTableName + @"\" + _competitionData._startListClassStep.Name + @"\" + _competitionData._vaulter.Name + ".xlsx";
        //}

        //public static IXLCell GetNamedCell(IXLWorksheet worksheet, string namedCell)
        //{
        //    IXLNamedRange xlNamedRange = worksheet.NamedRange(namedCell);
        //    if (xlNamedRange == null)
        //        return (IXLCell)null;
        //    IXLRange xlRange = xlNamedRange.Ranges.FirstOrDefault<IXLRange>();
        //    if (xlRange == null)
        //        return (IXLCell)null;
        //    return xlRange.FirstCell();
        //}

        //private static Step GetCompetitionStep(CompetitionClass vaulterClass, int testNumber)
        //{
        //    foreach (var step in vaulterClass.Steps)
        //    {
        //        if (testNumber == step.TestNumber)
        //            return step;
        //    }

        //    return null;
        //}

        private string GetStartNumberForVaulterString()
        {
            return _competitionData.StartVaulterNumber.ToString();
        }

        //private static JudgeTable GetJudge(List<JudgeTable> judgeTables, JudgeTableNames tableName)
        //{
        //    foreach (var table in judgeTables)
        //    {
        //        if (table.JudgeTableName == tableName)
        //            return table;
        //    }
        //    return null;
        //}

        //private string GetJudgeName(JudgeTable judgeTable)
        //{
        //    return judgeTable?.JudgeName;
        //}

        //private string GetStepDate()
        //{
        //        return _startListClassStep.Date.ToShortDateString();
        //}

        //private static string GetJudgeTableName(JudgeTable judgeTable)
        //{
        //    return judgeTable?.JudgeTableName.ToString();
        //}
    }
}