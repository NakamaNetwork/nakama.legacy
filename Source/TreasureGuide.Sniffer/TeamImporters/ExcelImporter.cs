using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using TreasureGuide.Sniffer.TeamImporters.Models;

namespace TreasureGuide.Sniffer.TeamImporters
{
    public class ExcelImporter : TeamImporter
    {
        private static readonly Regex DigitRegex = new Regex("\\d");
        protected override string Output { get; } = "Excel";

        protected override async Task<IEnumerable<TeamEntry>> GetTeams()
        {
            var excelApp = new Application();
            var book = excelApp.Workbooks.Open(@"C:\Users\robocafaz\Documents\clear rates.xlsx");
            var sheet = (Worksheet)book.Sheets[1];
            var range = sheet.UsedRange;

            var rows = range.Rows.Count;
            var columns = range.Columns.Count;

            var data = new List<TeamEntry>();
            for (var x = 1; x <= rows; x++)
            {
                for (var y = 1; y <= columns; y++)
                {
                    string value = range.Cells[x, y]?.ToString().Replace("|", ";");
                    if (value != null)
                    {
                        string leader = range.Cells[1, y]?.ToString();
                        leader = leader.Replace("|", ";");
                        leader = DigitRegex.Replace(leader, "");
                        string content = range.Cells[x, 1]?.ToString();
                        content = content.Replace("|", ";");
                        content = DigitRegex.Replace(content, "");
                        string stage = range.Cells[x, 2]?.ToString();
                        if (x > 1 && y > 2)
                        {
                            if (value.Contains("[") && !value.Contains("[V"))
                            {
                                int stageId;
                                var parsed = Int32.TryParse(stage, out stageId);
                                var entry = new TeamEntry { Content = content, Leader = leader, CalcLink = value, Desc = value, StageId = parsed ? stageId : (int?)null };
                                if (value.Contains("yout"))
                                {
                                    entry.Video = value;
                                }
                                data.Add(entry);
                            }
                        }
                    }
                }
            }
            return data;
        }
    }
}
