using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_2.Apps.Trpg;

internal class Dice
{
    public static async void SayRoll(SocketUserMessage arg)
    {
        await arg.Channel.SendMessageAsync(
            $"주사위를 굴렸습니다 :{Roll(6)}");
    }

    public static async void SayRoll(int diceSide, SocketUserMessage arg)
    {
        await arg.Channel.SendMessageAsync(
            $"D{diceSide}를 굴렸습니다 :{Roll(diceSide)}");
    }

    public static int Roll(int diceSide)
    {
        if (diceSide < 1) return 1;
        return new Random().Next(1, diceSide);
    }

}
