using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_2.Base;

/// <summary>
///     유저의 아이디를 기반으로 유저의 정보와 상태를 나타냅니다. 어떤 명령어가 실행되었었는지도 저장합니다.
/// </summary>
internal class UserContext
{
    #region member

    /// <summary>
    ///     Nullable
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public SingleUserContext this[ulong id]
    {
        get
        {
            if (UserId2Context.ContainsKey(id) == false)
                UserId2Context.Add(id, new SingleUserContext());
            return UserId2Context[id];
        }
        set
        {
            if(UserId2Context.ContainsKey(id))
                UserId2Context[id] = value;
            else
                UserId2Context.Add(id, value);
        }
    }
    private Dictionary<ulong, SingleUserContext> UserId2Context;

    public UserContext()
    {
        UserId2Context = new Dictionary<ulong, SingleUserContext>();
    }
    #endregion
    #region nestedClass
    public class SingleUserContext
    {
        /// <summary>
        ///     최근 입력한 반응입니다.
        /// </summary>
        public Emoji recentEmojiReaction;
        /// <summary>
        ///     최근 입력한 반응입니다.
        /// </summary>
        public Emote recentEmoteReaction;
        /// <summary>
        ///     최근 입력한 메시지입니다.
        /// </summary>
        public SocketUserMessage recentMessage;
    }


    #endregion
}
