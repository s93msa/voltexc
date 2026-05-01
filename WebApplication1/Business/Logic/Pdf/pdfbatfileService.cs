using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VoltigeCore.Business.Logic.Contest;
using VoltigeCore.Models;

namespace VoltigeCore.Business.Logic.Pdf
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
                    startListNamesPerJudgetable.Add(startListClassDate, new Dictionary<JudgeTableNames, List<string>>());

                var startListClass = startListNamesPerJudgetable[startListClassDate];
                foreach (var judgeTableEntry in startListClassStep.JudgeTables)
                {
                    var judgeTable = judgeTableEntry.JudgeTableName;
                    if (!startListNamesPerJudgetable[startListClassDate].ContainsKey(judgeTable))
                        startListNamesPerJudgetable[startListClassDate].Add(judgeTable, new List<string>());
                    startListClass[judgeTable].Add(startListClassStep.Name.Trim().Replace("–", ""));
                }
            }
            return startListNamesPerJudgetable;
        }

        public static void WriteBatfile(string filename, Dictionary<JudgeTableNames, List<string>> startlistNames)
        {
            var rows = new List<string>();
            foreach (var judgeTable in startlistNames)
            {
                foreach (var startlistclassName in judgeTable.Value)
                    rows.Add(judgeTable.Key + "\\" + startlistclassName);
            }
            if (rows.Count() == 0) return;

            using (var sw = new StreamWriter(File.Open(AppConfig.OutputPath + $"createpdf{filename}.bat", FileMode.CreateNew), Encoding.GetEncoding("ibm850")))
            {
                foreach (var row in rows)
                {
                    var relPath = row.Replace("/", "");
                    sw.WriteLine("..\\..\\printSheets\\printSheets.exe \"" + relPath + "\"");
                }
            }
        }
    }
}
