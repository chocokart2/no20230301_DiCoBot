using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_2;

internal class Hack
{
    public static void Say(object callerClass, object message,
        bool isPrint = true,
        [CallerMemberName] string memberName = "")
    {
        if (isPrint)
        {
            Console.WriteLine($"DEBUG_{callerClass.GetType().Name}.{memberName}() : {message}");
        }
    }
}