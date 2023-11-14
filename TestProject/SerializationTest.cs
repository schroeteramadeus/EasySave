using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject
{
    public class JSONSerializationTest : EasySave.Offline.JSON.JSONSerializable
    {
        public string hello = "there";
        public string general = "kenobi";
    }
    public class SQLSerializationTest : EasySave.Online.SQL.SQLSerializable
    {
        public string hello = "there";
        public string general = "kenobi";
    }
}
