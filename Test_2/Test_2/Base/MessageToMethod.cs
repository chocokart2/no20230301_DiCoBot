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
///     메시지를 받아서 다른 클래스의 함수 호출을 하는 클래스입니다.
/// </summary>
internal class MessageToMethod
{
    private SocketUserMessage mSocketUserMessage;

    public MessageToMethod(SocketUserMessage arg) { this.mSocketUserMessage = arg; }

    public (bool, string) RecvAdmin(string message)
    {
        (bool, string) result;
        switch (message)
        {
            // 특별히 어드민만 사용 가능한 명령어만 기술
            case "저장": result = DataIO.Save(); break;
            case "종료": result = (false, ""); Program.NeedShutdown = true; break;
                // 그 외 경우는 일반인 명령어만
            default: result = Recv(message); Hack.Say(this, "주인님 반갑습니다."); break;
        }

        return result;
    }

    public (bool, string) Recv(string message)
    {   
        string[] commands = message.Split(' ');
        (bool, string) result = (false, "");

        if(commands.Length == 0)
        {
            return result;
        }

        // if문의 도움이 필요한 구현
        // 다이스 구현
        if (commands[0].StartsWith('d') || commands[0].StartsWith('D'))
        {
            if (commands[0].Length == 1)
            {
                Apps.Trpg.Dice.SayRoll(mSocketUserMessage);
                return result;
            }
            if (int.TryParse(commands[0].Remove(0,1), out int diceSide))
            {
                Apps.Trpg.Dice.SayRoll(diceSide, mSocketUserMessage);
                return result;
            }
        }

        // 그 외 나머지 구현
        switch (commands[0])
        {
            case "에코":
            case "echo":
                result = (true, message); mEcho(message); break;
            default:
                mEcho($"메시지 받았습니다!\n메시지 내용 :{message}");
                break;
        }

        return result;
    }

    private async void mEcho(string message)
    {
        await mSocketUserMessage.Channel.SendMessageAsync(message);
    }

}
