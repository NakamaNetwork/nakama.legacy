using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Excel;

namespace TreasureGuide.Parser
{
    public static class Program
    {
        private static Regex DigitRegex = new Regex("\\d");

        public static void Main(string[] args)
        {
            var excelApp = new Application();
            var book = excelApp.Workbooks.Open(@"C:\Users\robocafaz\Documents\clear rates.xlsx");
            var sheet = (Worksheet)book.Sheets[1];
            var range = sheet.UsedRange;

            var rows = range.Rows.Count;
            var columns = range.Columns.Count;

            var data = new List<Entry>();
            for (var x = 1; x <= rows; x++)
            {
                for (var y = 1; y <= columns; y++)
                {
                    string value = range.Cells[x, y].Value?.ToString().Replace("|", ";");
                    if (value != null)
                    {
                        string leader = range.Cells[1, y].Value?.ToString();
                        leader = leader.Replace("|", ";");
                        leader = DigitRegex.Replace(leader, "");
                        string content = range.Cells[x, 1].Value?.ToString();
                        content = content.Replace("|", ";");
                        content = DigitRegex.Replace(content, "");
                        string stage = range.Cells[x, 2].Value?.ToString();
                        if (x > 1 && y > 2)
                        {
                            if (value.Contains("[") && !value.Contains("[V"))
                            {
                                int stageId;
                                var parsed = Int32.TryParse(stage, out stageId);
                                var entry = new Entry { Content = content, Leader = leader, Data = value, StageId = parsed ? stageId : (int?)null };
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
            var output = String.Join("|", data.Select(x => x.ParseOut()));
            Debug.WriteLine(output);
        }
    }

    public class Entry
    {
        public string Content { get; set; }
        public string Leader { get; set; }
        public string Data { get; set; }
        public string Video { get; set; }
        public int? StageId { get; set; }

        public string ParseOut()
        {
            return String.Join("|",
                $"{Leader} vs. {Content}", // name
                Data, // calc
                StageId, // stage
                Data, // desc
                "Referenced from the [/r/OnePieceTC](http://reddit.com/r/onepiecetc) [Global Clear Rates Spreadsheet](https://docs.google.com/spreadsheets/d/1Hhgy6RqsjD-vkL5_HsMsyZrI3weva4tLAvECBlpEDEc/edit?usp=sharing).", // credit
                Video, // videos
                "", // ref
                "" // type
            );
        }
    }
}