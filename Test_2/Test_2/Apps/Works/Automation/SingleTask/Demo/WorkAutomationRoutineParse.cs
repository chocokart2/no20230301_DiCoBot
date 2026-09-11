using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Test_2.Apps.Works.Automation.SingleTask.SolvedAcCoinNotification.WorkAutomationRoutineSolvedAcCoinNotification;

namespace Test_2.Apps.Works.Automation.SingleTask.Demo
{
    internal class WorkAutomationRoutineParse : IWorkAutomationRoutine
    {
        private class MTest
        {
            public int Rate { get; set; }
        }

        public async Task<WorkAutomationRoutineResult> StartAsync(WorkAutomationRoutineArguments arguments)
        {
            string m_json = "{\"Rate\":1500}";

            MTest recv = JsonSerializer.Deserialize<MTest>(m_json);

            var one = new WorkAutomationRoutineResult();
            one.SetDiscordMessage($"파싱 결과값 : {recv.Rate}");

            return one;
        }
    }
}
