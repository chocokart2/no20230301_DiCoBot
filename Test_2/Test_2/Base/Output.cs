using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_2.Base;

/// <summary>
///     출력을 담당하는 함수가 있습니다
/// </summary>
internal class Output
{
    
    // 아마 여기서 문자를 디스코드로 출력하는 함수가 있을 것입니다.

    /// <summary>
    ///     정해진 채널에 메시지를 전송합니다.
    /// </summary>
    public static async void Print(string message, SocketUserMessage context)
    {
        await context.Channel.SendMessageAsync(text: message);
    }
}
