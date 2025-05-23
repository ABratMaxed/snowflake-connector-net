using System.Data.Common;
using System;
using System.Data;
using Snowflake.Data.Core;

namespace Snowflake.Data.Client
{
    public class SnowflakeDbParameter : DbParameter
    {
        public SFDataType SFDataType { get; set; }

        private SFDataType OriginType;

        private DbType _dbType;

        public SnowflakeDbParameter()
        {
            SFDataType = SFDataType.None;
            OriginType = SFDataType.None;
        }

        public SnowflakeDbParameter(string ParameterName, SFDataType SFDataType)
        {
            this.ParameterName = ParameterName;
            this.SFDataType = SFDataType;
            OriginType = SFDataType;
        }

        public SnowflakeDbParameter(int ParameterIndex, SFDataType SFDataType)
        {
            this.ParameterName = ParameterIndex.ToString();
            this.SFDataType = SFDataType;
        }

        public override DbType DbType
        {
            get
            {
                if (_dbType != default(DbType) || Value == null || Value is DBNull)
                {
                    return _dbType;
                }

                var type = Value.GetType();
                if (type.IsArray && type != typeof(byte[]))
                {
                    return SFDataConverter.TypeToDbTypeMap[type.GetElementType()];
                }
                else
                {
                    return SFDataConverter.TypeToDbTypeMap[type];
                }
            }

            set => _dbType = value;
        }

        public override ParameterDirection Direction
        {
            get
            {
                return ParameterDirection.Input;
            }

            set
            {
                if (value != ParameterDirection.Input)
                {
                    throw new SnowflakeDbException(SFError.UNSUPPORTED_FEATURE);
                }
            }
        }

        public override bool IsNullable { get; set; }

        public override string ParameterName { get; set; }

        public override int Size { get; set; }

        public override string SourceColumn { get; set; }

        public override bool SourceColumnNullMapping { get; set; }

        public override object Value { get; set; }

        public override void ResetDbType()
        {
            SFDataType = OriginType;
        }
    }
}
