using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using WebApplication1.Business.Logic.Contest;
using WebApplication1.Models;

namespace WebApplication1.Business.Logic.Pdf
{
    public class pdfbatfileService
    {
        public static Dictionary<DateTime, Dictionary<JudgeTableNames, List<string>>> GetStartListNames()
        {
            var startListNamesPerJudgetable = new Dictionary<DateTime, Dictionary<JudgeTableNames, List<string>>>();

            var contest = ContestService.GetContestInstance();
            foreach (var startListClassStep in contest.GetActiveStartListClassStep().OrderBy(x => x.StartOrder))
            {
                var startListClassDate = startListClassStep.Date.Date;
                if (!startListNamesPerJudgetable.ContainsKey(startListClassDate))
                {
                    startListNamesPerJudgetable.Add(startListClassDate, new Dictionary<JudgeTableNames, List<string>>());
                }
                var startListClass = startListNamesPerJudgetable[startListClassDate];
                foreach (var JudgeTable in startListClassStep.JudgeTables)
                {
                    var judgeTable = JudgeTable.JudgeTableName;
                    if (!startListNamesPerJudgetable[startListClassDate].ContainsKey(judgeTable))
                    {
                        startListNamesPerJudgetable[startListClassDate].Add(judgeTable, new List<string>());
                    }
                    startListClass[judgeTable].Add(startListClassStep.Name.Trim().Replace("–", ""));
                }
            }    


            return startListNamesPerJudgetable;
        }

        public static void  WriteBatfile(string filename, Dictionary<JudgeTableNames, List<string>> startlistNames)
        {
            var rows = new List<string>();
            foreach (var judgeTable in startlistNames)
            {
                foreach (var startlistclassName in judgeTable.Value)
                {
                    rows.Add(judgeTable.Key + "\\" + startlistclassName);
                }
            }
            if(rows.Count() == 0)
            {
                return;
            }
            using (var sw = new StreamWriter(File.Open($@"C:\episerver\voltige\VoltigeClosedXML\output\createpdf{filename}.bat", FileMode.CreateNew), Encoding.GetEncoding("ibm850")))
            {
                foreach (var row in rows)
                {
                    var relPath = row.Replace("/", "");
                    sw.WriteLine("..\\..\\printSheets\\printSheets.exe \"" + relPath + "\"");
                }
            }


            //Encoding iso = Encoding.GetEncoding("ISO-8859-1");



            //// Create a new file     
            //using (FileStream fs = File.Create("test.bat"))
            //{
            //    foreach (var row in rows)
            //    {
            //        // Add some text to file    
            //        Byte[] title = iso.GetBytes("..\\..\\printSheets\\printSheets.exe \"" + row +  "\"");
            //        fs.Write(title, 0, title.Length);
            //    }
            //}


        }
    }
}