using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Online.SQL.DataManagement
{
    public class SQLDataStructure
    {
        //TODO
        public List<SQLTable> Tables { get; private set; } = new List<SQLTable>();
        public List<SQLForeignKey> ForeignKeys { get; private set; } = new List<SQLForeignKey>();


        public void AddTable(SQLTable table)
        {
            this.Tables.Add(table);
        }

        public void AddForeignKey(SQLForeignKey foreignKey)
        {
            this.ForeignKeys.Add(foreignKey);
        }
    }
}
