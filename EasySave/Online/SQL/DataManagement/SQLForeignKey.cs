using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Online.SQL.DataManagement
{
    //TODO

    public class SQLForeignKey
    {
        public SQLTable Table1 { get; private set; }
        public SQLTable Table2 { get;private set; }

        public List<SQLAttribute> Keys1 { get; private set; }
        public List<SQLAttribute> Keys2 { get; private set; }

        public SQLForeignKey(SQLTable table1, SQLTable table2, List<SQLAttribute> keys1, List<SQLAttribute> keys2)
        {
            this.Table1 = table1;
            this.Table2 = table2;
            this.Keys1 = keys1;
            this.Keys2 = keys2;
        }
    }
}
