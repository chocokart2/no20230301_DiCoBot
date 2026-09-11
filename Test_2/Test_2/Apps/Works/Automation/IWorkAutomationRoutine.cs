using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_2.Apps.Works.Automation
{
    /// <summary>
    ///     업무 자동화를 실시하는 루틴 객체입니다.
    /// </summary>
    internal interface IWorkAutomationRoutine
    {
        Task<WorkAutomationRoutineResult> StartAsync(WorkAutomationRoutineArguments arguments);
    }
}
