using System;
using System.Collections.Generic;
using System.Text;
using EasySave.Online.SQL.DataManagement;
using System.Reflection;

namespace EasySave.Online.SQL
{
    //TODO

    public class SQLSerializable
    {
        private static Dictionary<Type, SQLDataStructure> _cache = new Dictionary<Type, SQLDataStructure>();

        private List<SQLAttribute> _key; //get from database
        private SQLDataStructure _dataStructure;

        public SQLSerializable()
        {
            /*
            //TODO
            //get and cache datastructure if not present
            if (!SQLSerializable._cache.ContainsKey(this.GetType()))
            {
                List<SQLAttribute> key = new List<SQLAttribute>();
                List<SQLAttribute> attributes = new List<SQLAttribute>();

                foreach(MemberInfo m in this.GetType().GetMembers())
                {
                    Attribute attr = m.GetCustomAttribute(typeof(SQLDefinition));
                    if(attr != null)
                    {
                        SQLDefinition sqlDefinition = (SQLDefinition)attr;
                        //TODO split into more tables for converters
                        if (sqlDefinition.IsPartOfKey)
                        {
                            key.Add(new SQLAttribute(m.Name, m.GetType()));
                        }
                        else
                        {
                            attributes.Add(new SQLAttribute(m.Name, m.GetType()));
                        }
                    }
                    else
                    {
                        attributes.Add(new SQLAttribute(m.Name, m.GetType()));
                    }
                }

                //SQLTable mainTable = new SQLTable(this.GetType(),);
                //create new Datastructure
                //save in _cache
            }
            if (SQLSerializable._cache.TryGetValue(this.GetType(), out this._dataStructure))
            {

            }
            else
                throw new KeyNotFoundException("The data structure could not be created.");
        */
        }

        public static T Deserialize<T>(int id) where T : SQLSerializable
        {
            //daten mit id aus datenbank laden
            throw new NotImplementedException();
        }
        public void Serialize()
        {
            //mehrere Tabellen erstellen (falls nötig)
            //in dataobject serialisieren
            //speichern (mit Parameter: Client)
        }
    }
}
