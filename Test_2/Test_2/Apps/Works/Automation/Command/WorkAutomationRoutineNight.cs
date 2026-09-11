using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_2.Apps.Works.Automation.SingleTask.IsTrashRemoveDay;

namespace Test_2.Apps.Works.Automation.Command
{
    internal class WorkAutomationRoutineNight : WorkAutomationRoutineManager
    {
        protected override void InitRoutine()
        {
            base.InitRoutine();
            routines.Add(new WorkAutomationRoutineTrashCheck());
        }
    }
}
