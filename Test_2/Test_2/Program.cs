using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using Test_2.Base;
using Test_2.Code.Object;
using static System.Collections.Specialized.BitVector32;

namespace Test_2;

// 어떤 키 -> 어떤 객체 : 뭐더라, 메시지를 주고받을때, 맥락같은걸 요구하기도 합니다.
public class Program
{
    static internal Apps.Works.Automation.WorkAutomationRoutineManager RoutineManager;

    private DiscordSocketClient _client;
    // userID => UserContext
    private Base.UserContext _userContext;
    private Apps.Works.Automation.WorkAutomationRoutineManager _routineManager;
    //private Singleton _singleton;
    /// <summary>
    ///     무한 루프를 종료시킵니다.
    /// </summary>
    public static bool NeedShutdown = false;

    public static Task Main(string[] args)
    {
        return new Program().MainAsync();
    }
    
    public async Task MainAsync()
    {
        Init();

        // 이 녀석이 메시지를 읽을 특권을 부여합니다.
        //DiscordSocketConfig config = new()
        //{
        //    GatewayIntents = GatewayIntents.All
        //};
        _client = new DiscordSocketClient(new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.All,
            LogLevel = LogSeverity.Debug
        });
        _userContext = new Base.UserContext();

        Hack.Say(this, "클라이언트 / 컨피그 설정됨");

        // 이벤트에 메서드를 걸어놓습니다.
        _client.Log += Log;
        _client.Ready += Ready;
        _client.MessageReceived += ReadMessage;
        _client.ReactionAdded += AddReaction;

        await _client.LoginAsync(TokenType.Bot, TokenReceiver.GetApiKey(EKeyType.DiscordBot));
        await _client.StartAsync();

        while(NeedShutdown == false)
        {
            Hack.Say(this, "루프를 돕니다.");

            await Task.Delay(6000);
        }
        Hack.Say(this, "루프를 빠져나갑니다");
        await Task.Delay(Timeout.Infinite);
    }

    private void Init()
    {
        Singleton.instance = new Singleton();
        Singleton.instance.Init();

        _routineManager = new Apps.Works.Automation.WorkAutomationRoutineManager();
        RoutineManager = _routineManager;
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private Task Ready()
    {
        Hack.Say(this, $"{_client.CurrentUser}가 연결되었습니다.");

        return Task.CompletedTask;
    }

    private async Task ReadMessage(SocketMessage args)
    {
        // 이 함수 내부에서 전부를 처리해야 합니다!
        SaySocketMessageInfo(args);
        SocketUserMessage message = args as SocketUserMessage; // 소수의 예외를 제외하고는 대부분 SocketUserMessage의 형식이에요


        if (message == null)
        {
            Hack.Say(this, "args를 SocketUserMessage로 바꿀 수 없음.");
            return; // Task.CompletedTask;
        }
        if (message.Author.Id.Equals(_client.CurrentUser.Id))
        {
            Hack.Say(this, "내가 보낸 메시지는 무시합니다");
            return; // Task.CompletedTask;
        }
        int pos = 0;
        if(!(message.HasCharPrefix('.', ref pos) ||
            message.HasStringPrefix("여기", ref pos) ||
            message.HasStringPrefix("여기야", ref pos)))
        {
            Hack.Say(this, "접두어가 옳지 않음.");
            return;
        }

        // 여기에 메시지 넣기
        
        Base.MessageToMethod receiver = new(message);

        // 접두어 필터링
        string content = null;
        if (message.HasCharPrefix('.', ref pos))
        {
            content = message.Content.Remove(0, 1);
        }
        else if (message.HasStringPrefix("여기야 ", ref pos))
        {
            content = message.Content.Remove(0, 4);
        }
        else if (message.HasStringPrefix("여기 ", ref pos))
        {
            content = message.Content.Remove(0, 3);
        }
        _userContext[message.Author.Id].recentMessage = message;

        MessageToMethodResult m_messageResult;

        if (message.Author.Id == Data.MasterInfo.MasterID)
            m_messageResult = await receiver.RecvAdmin(content);
        else
            m_messageResult = await receiver.Recv(content);

        if (m_messageResult.isPrintMessage)
        {
            message.Channel.SendMessageAsync($"{m_messageResult.message}");
        }

        return; // Task.CompletedTask;
    }


    
    private async Task AddReaction(Cacheable<IUserMessage, ulong> userMessage, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
    {
        Emote emoji = reaction.Emote as Emote;
        if(emoji == null)
        {
            await reaction.Channel.SendMessageAsync($"반응이 추가되었습니다! 알 수 없는 이모지!");
            return;
        }
        
        await reaction.Channel.SendMessageAsync($"반응이 추가되었습니다! 반응 이미지 : {emoji.Url}");
    }

    private void SaySocketMessageInfo(SocketMessage args)
    {
        Hack.Say(this, $"받은 메시지\t:{args.Content}");
        Hack.Say(this, $"작성자 아이디\t:{args.Author.Id}");
        Hack.Say(this, $"작성자 이름\t:{args.Author.Username}");
    }
}