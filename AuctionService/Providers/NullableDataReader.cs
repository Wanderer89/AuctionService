using System;
using System.Data;
using System.Globalization;

namespace AuctionService.Providers
{
    public class NullableDataReader
    {
        private IDataReader m_reader;
        private delegate T Converter<T>(int ordinal);

        public NullableDataReader(IDataReader reader)
        {
            m_reader = reader;
        }

        #region INullableDataReader Members

        public bool GetBoolean(string name)
        {
            return GetNonNullable<bool>(name, m_reader.GetBoolean);
        }

        public bool GetBooleanString(string name)
        {
            string value = GetString(name);

            if (value.Equals("T") || value.Equals("1") || value.Equals("Y"))
                return true;
            else
                return false;
        }

        public bool? GetNullableBoolean(string name)
        {
            return GetNullable<bool>(name, m_reader.GetBoolean);
        }

        public bool? GetNullableBooleanString(string name)
        {
            string nullableValue = GetNullableString(name);
            if (string.IsNullOrEmpty(nullableValue))
                return null;
            else if (nullableValue.Equals("T") || nullableValue.Equals("1") || nullableValue.Equals("Y"))
                return true;
            else
                return false;
        }

        public byte GetByte(string name)
        {
            return GetNonNullable<byte>(name, m_reader.GetByte);
        }

        public byte? GetNullableByte(string name)
        {
            return GetNullable<byte>(name, m_reader.GetByte);
        }

        public char GetChar(string name)
        {
            return GetNonNullable<char>(name, m_reader.GetChar);
        }

        public char? GetNullableChar(string name)
        {
            return GetNullable<char>(name, m_reader.GetChar);
        }

        public DateTime GetDateTime(string name)
        {
            return GetNonNullable<DateTime>(name, m_reader.GetDateTime);
        }

        public DateTimeOffset GetDateTimeWithPST(string name)
        {
            var d = GetNonNullable<DateTime>(name, m_reader.GetDateTime);
            return DateTimeOffset.Parse(d.ToString("G") + "-0800", CultureInfo.InvariantCulture);
        }

        public DateTimeOffset? GetNullableDateTimeWithPST(string name)
        {
            var d =  GetNullable<DateTime>(name, m_reader.GetDateTime);
            if (d == null)
                return null;
            else
                return DateTimeOffset.Parse(d?.ToString("G") + "-0800", CultureInfo.InvariantCulture);
        }

        public DateTime? GetNullableDateTime(string name)
        {
            return GetNullable<DateTime>(name, m_reader.GetDateTime);
        }


        public DateTimeOffset GetDateTimeOffset(string name)
        {
            return DateTimeOffset.Parse(GetNonNullable(name).ToString("G") + "-0800", CultureInfo.InvariantCulture);
        }

        public DateTimeOffset? GetNullableDateTimeOffset(string name)
        {
            return GetNullable(name);
        }

        public decimal GetDecimal(string name)
        {
            return GetNonNullable<decimal>(name, m_reader.GetDecimal);
        }

        public decimal? GetNullableDecimal(string name)
        {
            return GetNullable<decimal>(name, m_reader.GetDecimal);
        }

        public double GetDouble(string name)
        {
            return GetNonNullable<double>(name, m_reader.GetDouble);
        }

        public double? GetNullableDouble(string name)
        {
            return GetNullable<double>(name, m_reader.GetDouble);
        }

        public float GetFloat(string name)
        {
            return GetNonNullable<float>(name, m_reader.GetFloat);
        }

        public float? GetNullableFloat(string name)
        {
            return GetNullable<float>(name, m_reader.GetFloat);
        }

        public Guid GetGuid(string name)
        {
            return GetNonNullable<Guid>(name, m_reader.GetGuid);
        }

        public Guid? GetNullableGuid(string name)
        {
            return GetNullable<Guid>(name, m_reader.GetGuid);
        }

        public short GetInt16(string name)
        {
            return GetNonNullable<short>(name, m_reader.GetInt16);
        }

        public short? GetNullableInt16(string name)
        {
            return GetNullable<short>(name, m_reader.GetInt16);
        }

        public int GetInt32(string name)
        {
            return GetNonNullable<int>(name, m_reader.GetInt32);
        }

        public int? GetNullableInt32(string name)
        {
            return GetNullable<int>(name, m_reader.GetInt32);
        }

        public long GetInt64(string name)
        {
            return GetNonNullable<long>(name, m_reader.GetInt64);
        }

        public long? GetNullableInt64(string name)
        {
            return GetNullable<long>(name, m_reader.GetInt64);
        }

        public double GetSqlDecimal(string name)
        {
            return GetNonNullable<double>(name, m_reader.GetDouble);
        }

        public double? GetNullableSqlDecimal(string name)
        {
            return GetNullable<double>(name, m_reader.GetDouble);
        }

        public string GetString(string name)
        {
            return GetNonNullable<string>(name, m_reader.GetString).Trim();
        }

        public string GetNullableString(string name)
        {
            int ordinal = -1;

            try
            {
                ordinal = m_reader.GetOrdinal(name);
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }

            if (m_reader.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                if (ordinal.Equals(3))
                {
                    var abc = m_reader.GetString(3);
                }
                return SafeConvert<string>(name, ordinal, m_reader.GetString).Trim();
            }
        }

        public object GetValue(string name)
        {
            int ordinal = GetOrdinal(name);
            if (m_reader.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return m_reader.GetValue(ordinal);
            }
        }

        public bool IsDBNull(string name)
        {
            return m_reader.IsDBNull(GetOrdinal(name));
        }

        #endregion

        #region Private Methods

        private int GetOrdinal(string name)
        {
            try
            {
                return m_reader.GetOrdinal(name);
            }
            catch (IndexOutOfRangeException)
            {
                throw;
            }
        }

        private T SafeConvert<T>(string name, int ordinal, Converter<T> convert)
        {
            try
            {
                return convert(ordinal);
            }
            catch (InvalidCastException e)
            {
                throw;
            }
        }

        private Nullable<T> GetNullable<T>(string name, Converter<T> convert)
            where T : struct
        {
            Nullable<T> nullable;

            int ordinal = -1;

            try
            {
                ordinal = m_reader.GetOrdinal(name);
            }
            catch (IndexOutOfRangeException)
            {
                nullable = null;
                return nullable;
            }

            if (m_reader.IsDBNull(ordinal))
            {
                nullable = null;
            }
            else
            {
                nullable = SafeConvert<T>(name, ordinal, convert);
            }
            return nullable;
        }

        private DateTimeOffset GetNonNullable(string name)
        {
            int ordinal = GetOrdinal(name);
            if (m_reader.IsDBNull(ordinal))
            {
                throw new Exception("Exception from GetNonNullable");
            }
            else
            {
                DateTimeOffset result;
                Object t = m_reader.GetValue(ordinal);

                try
                {
                    //result = (DateTimeOffset)t;
                    result = new DateTimeOffset((DateTime) t);
                }
                catch (InvalidCastException e)
                {
                    String str = t.ToString();
                    //  UnifiedLogging.Log("Windows.Store.Licensing.Error", "Error parsing DateTimeOffset, String is " + str);
                    throw;
                }

                return result;
            }
        }

        private Nullable<DateTimeOffset> GetNullable(string name)
        {
            Nullable<DateTimeOffset> nullable;

            int ordinal = -1;

            try
            {
                ordinal = m_reader.GetOrdinal(name);
            }
            catch (IndexOutOfRangeException)
            {
                nullable = null;
                return nullable;
            }

            if (m_reader.IsDBNull(ordinal))
            {
                nullable = null;
            }
            else
            {
                nullable = GetNonNullable(name);
            }
            return nullable;
        }



        private T GetNonNullable<T>(string name, Converter<T> convert)
        {
            int ordinal = GetOrdinal(name);
            if (m_reader.IsDBNull(ordinal))
            {
                throw new Exception("Invalid operation");
            }
            else
            {
                return SafeConvert<T>(name, ordinal, convert);
            }
        }

        #endregion
    }
}
