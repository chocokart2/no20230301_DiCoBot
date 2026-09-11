using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_2.Apps.Works.Automation.SingleTask.Demo
{
    internal class WorkAutomationRoutineDemo : IWorkAutomationRoutine
    {
        public async Task<WorkAutomationRoutineResult> StartAsync(WorkAutomationRoutineArguments arguments)
        {
            Console.WriteLine("루틴 실행스");

            WorkAutomationRoutineResult result = new WorkAutomationRoutineResult();
            result.SetIsSuccess(true);
            result.SetMessage("데모 루틴이 성공적으로 완료되었습니다.");
            result.SetDiscordMessage("이것은 디스코드 서버 시연용 데모 루틴 메시지입니다.");
            return result;
        }
    }
}
