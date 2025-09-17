﻿using Dapper;
using System.Data;

namespace BuoySensorManager.Core.Handlers
{
    public class GuidHandler : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            parameter.Value = value.ToString();
        }

        public override Guid Parse(object value)
        {
            return Guid.Parse(value.ToString()!);
        }
    }

}
