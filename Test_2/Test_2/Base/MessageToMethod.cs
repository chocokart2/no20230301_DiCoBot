using Discord;
using Discord.Commands;
using Discord.WebSocket;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_2.Apps.LlmChat;
using Test_2.Apps.Works.Automation;
using Test_2.Code.Object;

namespace Test_2.Base;

/// <summary>
///     메시지를 받아서 다른 클래스의 함수 호출을 하는 클래스입니다.
/// </summary>
internal class MessageToMethod
{
    private SocketUserMessage mSocketUserMessage;

    public MessageToMethod(SocketUserMessage arg) { this.mSocketUserMessage = arg; }

    public async Task<MessageToMethodResult> RecvAdmin(string message)
    {
        MessageToMethodResult result;
        switch (message)
        {
            // 특별히 어드민만 사용 가능한 명령어만 기술
            case "저장": result = DataIO.Save(); break;
            case "종료": result = new MessageToMethodResult()
                {
                    isSuccess = false,
                    message = "종료합니다."
                };
                Program.NeedShutdown = true; break;
            case "루틴":
                //await mSocketUserMessage.Channel.SendMessageAsync("루틴을 시작하겠습니다.");
                WorkAutomationRoutineArguments m_context = new WorkAutomationRoutineArguments();
                m_context.Seal();

                //await mSocketUserMessage.Channel.SendMessageAsync($"루틴의 현재 컨텍스트 : {m_context.Time}");
                StringBuilder m_resultPrintMessage = new StringBuilder();
                m_resultPrintMessage.AppendLine("루틴이 호출되었습니다. 다음은 결과입니다.");
                List<WorkAutomationRoutineResult> m_routineResultList = await Program.RoutineManager.DoRoutine(m_context);
                foreach (WorkAutomationRoutineResult m_one in m_routineResultList)
                {
                    m_resultPrintMessage.AppendLine(m_one.DiscordMessage);

                }
                m_resultPrintMessage.AppendLine("루틴 결과 끝.");

                result = new MessageToMethodResult()
                {
                    isSuccess = false,
                    isPrintMessage = true,
                    message = m_resultPrintMessage.ToString()
                };
                // await mSocketUserMessage.Channel.SendMessageAsync("루틴이 종료되었습니다.");
                break;
                // 그 외 경우는 일반인 명령어만
            default: result = await Recv(message); Hack.Say(this, "주인님 반갑습니다."); break;
        }

        return result;
    }

    public async Task<MessageToMethodResult> Recv(string message)
    {   
        string[] commands = message.Split(' ');
        MessageToMethodResult result = new MessageToMethodResult()
        {
            isSuccess = false,
            message = "메시지 없음"
        };

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
                result = new MessageToMethodResult()
                {
                    isSuccess = true,
                    message = message
                };
                mEcho(message); break;
            case "근데":
                result = await Singleton.instance.llmOpenAi.AssignWorkAsync(message);
                break;
            case "문서읽어":


                break;
            case "노션":
                result = new MessageToMethodResult()
                {
                    isSuccess = true,
                    message = await Singleton.instance.notionFileGetter.GetBlocksAsync()
                };
                break;
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
