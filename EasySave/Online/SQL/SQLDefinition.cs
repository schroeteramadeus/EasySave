using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Online.SQL
{
    //TODO

    /*[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]*/
    [AttributeUsage(AttributeTargets.Field)]
    public class SQLDefinition : Attribute
    {
        public bool IsPartOfKey = false;
        public SQLConversionType ConversionType { get; private set; } = SQLConversionType.Default;
        public ISQLTypeConverter TypeConverter { get; private set; }

        public SQLDefinition(bool isPartOfKey) : this(SQLConversionType.Default, isPartOfKey) { }

        public SQLDefinition(SQLConversionType type) : this(type, false) { }

        public SQLDefinition(ISQLTypeConverter typeConverter) : this(typeConverter, false) { }

        public SQLDefinition(SQLConversionType type, bool isPartOfKey)
        {
            this.ConversionType = type;
            this.IsPartOfKey = isPartOfKey;
        }
        public SQLDefinition(ISQLTypeConverter typeConverter, bool isPartOfKey)
        {
            this.TypeConverter = typeConverter;
            this.IsPartOfKey = isPartOfKey;
        }
    }
    /// <summary>
    /// Contains supported SQL Types to convert to
    /// </summary>
    public enum SQLConversionType
    {
        Int,
        Float,
        String,
        Bool,
        /// <summary>
        /// Handle the conversion automatically (will ignore complex objects if no explicit converter is given)
        /// </summary>
        Default,
        /// <summary>
        /// Does not save the structure
        /// </summary>
        Ignore
    }
}
