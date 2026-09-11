using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_2.Apps.Works.Automation.SingleTask.IsTrashRemoveDay
{
    internal class WorkAutomationRoutineTrashCheck : IWorkAutomationRoutine
    {
        public async Task<WorkAutomationRoutineResult> StartAsync(WorkAutomationRoutineArguments arguments)
        {
            WorkAutomationRoutineResult result = new WorkAutomationRoutineResult();
            StringBuilder printString = new StringBuilder();
            DayOfWeek weekDay = arguments.Time.DayOfWeek;

            switch (weekDay)
            {
                case DayOfWeek.Sunday: printString.AppendLine("오늘 일반 쓰레기 버리는 날입니다."); break;
                case DayOfWeek.Monday: printString.AppendLine("오늘 비닐류 / 플라스틱 버리는 날입니다."); break;
                case DayOfWeek.Tuesday: printString.AppendLine("오늘 일반 쓰레기 버리는 날입니다."); break;
                case DayOfWeek.Wednesday: printString.AppendLine("오늘 페트병 / 종이류 / 캔 버리는 날입니다."); break;
                case DayOfWeek.Thursday: printString.AppendLine("오늘 일반 쓰레기 버리는 날입니다."); break;
                case DayOfWeek.Friday: printString.AppendLine("오늘 폐기물 처리가 없습니다."); break;
                case DayOfWeek.Saturday: printString.AppendLine("오늘 폐기물 처리가 없습니다."); break;
                default: break;
            }

            result.SetDiscordMessage(printString.ToString());
            return result;
        }
    }
}
