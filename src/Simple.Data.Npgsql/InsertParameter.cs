using System;

namespace Simple.Data.Npgsql
{
    public static class InsertParameter
    {
        public static object Transform(object value)
        {
            if (value != null)
            {
                Type valueType = value.GetType();
                if (valueType.IsEnum)
                {
                    value = Convert.ChangeType(value, Enum.GetUnderlyingType(valueType));
                }
            }

            return value;
        }
    }
}