using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_2.Apps.Works.Automation
{
    /// <summary>
    ///     루틴 결과를 알려줍니다.
    /// </summary>
    internal class WorkAutomationRoutineResult
    {
        public bool IsSuccess { get => m_isSuccess; }
        public string Message { get => m_message; }
        public string DiscordMessage { get => m_discordMessage; }
        public Exception? Exception { get => m_exception; }

        bool m_isSealed = false;
        bool m_isSuccess;
        string m_message;
        string m_discordMessage;
        Exception? m_exception;

        public WorkAutomationRoutineResult() { }

        public void SetIsSuccess(bool value)
        {
            if (m_isSealed) return;
            m_isSuccess = value;
        }

        public void SetMessage(string value)
        {
            if (m_isSealed) return;
            m_message = value;
        }

        public void SetDiscordMessage(string value)
        {
            if (m_isSealed) return;
            m_discordMessage = value;
        }

        public void SetException(Exception value)
        {
            if (m_isSealed) return;
            m_exception = value;
        }

        public void Seal() => m_isSealed = true;
    }
}
