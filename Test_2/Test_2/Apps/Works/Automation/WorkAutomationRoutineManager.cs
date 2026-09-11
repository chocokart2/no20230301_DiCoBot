using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_2.Apps.Works.Automation.SingleTask.Demo;
using Test_2.Apps.Works.Automation.SingleTask.IsTrashRemoveDay;
using Test_2.Apps.Works.Automation.SingleTask.SolvedAcCoinNotification;

namespace Test_2.Apps.Works.Automation
{
    /// <summary>
    ///     해당 클래스는 자동화를 지원합니다.
    /// </summary>
    internal class WorkAutomationRoutineManager
    {
        /// <summary>
        ///     루틴 목록을 나타냅니다. 일반적으로 파일 입출력을 통해 초기화됩니다.
        /// </summary>
        protected List<IWorkAutomationRoutine> routines;

        public WorkAutomationRoutineManager()
        {
            InitRoutine();
            //routines.Add(new WorkAutomationRoutineParse());
        }

        protected virtual void InitRoutine()
        {
            routines = new List<IWorkAutomationRoutine>();
            routines.Add(new WorkAutomationRoutineDemo());
            routines.Add(new WorkAutomationRoutineTrashCheck());
            routines.Add(new WorkAutomationRoutineSolvedAcCoinNotification());
        }

        public async Task<List<WorkAutomationRoutineResult>> DoRoutine(WorkAutomationRoutineArguments arguments)
        {
            var tasks = routines.Select(async (routine, index) =>
            {
                try
                {
                    return await routine.StartAsync(arguments);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Routine[{index}] 실행 중 오류 발생: {ex.Message}");
                    return null; // 예외 발생 시 null 반환
                }
            });

            return (await Task.WhenAll(tasks)).ToList();
        }
    }
}
