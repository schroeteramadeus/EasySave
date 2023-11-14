using System;
using System.Collections.Generic;
using System.Text;
using EasySave.Offline.JSON;

namespace TestProject
{
    public class MySQLServerConnection : JSONSerializable
    {
        public string Servername, Username, Database, Password;
    }
}
