
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NSoup;

namespace TODTool.TODSchedulers
{
    public class NSoupExample
    {
        private const string signInURL = "https://dellcms.sdlproducts.com/ISHSTS/account/signin";

        static void Main(string[] args)
        {
            IConnection loginformConnection = (IConnection)NSoupClient.Connect(signInURL).Method(Method.Get).Execute();
            IResponse response = loginformConnection.Response();
            System.Console.WriteLine(response.Cookies());
        }
    }
}