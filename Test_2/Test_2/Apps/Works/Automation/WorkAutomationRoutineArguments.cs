using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_2.Apps.Works.Automation
{
    /// <summary>
    ///     자동화 업무 시 맥락을 제공합니다.
    /// </summary>
    [Serializable]
    internal class WorkAutomationRoutineArguments
    {
        public DateTime Time => m_now;

        DateTime m_now;
        bool m_isSealed = false;
        string discordMessageArguments = "";

        public WorkAutomationRoutineArguments()
        {
            m_now = DateTime.Now;
        }

        public void SetDiscordMessageArguments(string value)
        {
            if (m_isSealed) return;
            discordMessageArguments = value;
        }

        public void Seal() => m_isSealed = true;
        
    }
}
