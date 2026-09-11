using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_2.Base
{
    /// <summary>
    ///     명령 시 맥락을 제공합니다.
    /// </summary>
    [Serializable]
    internal class MessageToMethodArguments
    {
        public DateTime Time => m_now;

        DateTime m_now;
        bool m_isSealed = false;
        string discordMessageArguments = "";

        public MessageToMethodArguments()
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
