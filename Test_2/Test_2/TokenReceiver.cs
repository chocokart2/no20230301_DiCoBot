using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_2;
#region MyRegion
enum EKeyType
{
    DiscordBot,
    OpenAiApi,
    NotionChocokartApi,
    Max
}

internal class TokenReceiver
{
    public const string DISCORD_BOT_KEY = "key";
    public const string OPENAI_API_KEY = "openai_api_key";
    public const string NOTION_CHOCOKART_API_KEY = "notion_chocokart_api_key";

    public static string GetApiKey(EKeyType type)
    {
        string filePath = type switch
        {
            EKeyType.DiscordBot => DISCORD_BOT_KEY,
            EKeyType.OpenAiApi => OPENAI_API_KEY,
            EKeyType.NotionChocokartApi => NOTION_CHOCOKART_API_KEY,
            _ => ""
        };

        string result = null;
        StreamReader sr =
            new StreamReader(
                new FileStream($"../../../../../DiCoBot/{filePath}.txt", FileMode.Open));
        result = sr.ReadLine();
        sr.Close();

        Debug.Assert(result != null, "!! TokenReceiver.GetApiKey(EKeyType type) : result가 널이예요");

        return result;
    }
}

#endregion
