using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_2;
#region MyRegion
internal class TokenReceiver
{
    public static string GetToken()
    {
        string result;
        StreamReader sr =
            new StreamReader(
                new FileStream("../../../../../DiCoBot/key.txt", FileMode.Open));
        result = sr.ReadLine();
        sr.Close();
        return result;
    }
}

#endregion
