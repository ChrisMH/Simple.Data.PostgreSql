using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace Simple.Data.PostgreSql
{
  public class TypeResolver
  {
    public static NpgsqlDbType GetDbType(string typeName)
    {
      if (!DbTypeMap.ContainsKey(typeName))
      {
        return NpgsqlDbType.Bytea; // Typically a user-defined type
      }
      return DbTypeMap[typeName];
    }

    public static Type GetClrType(string dbTypeName)
    {
      if (!ClrTypeMap.ContainsKey(dbTypeName))
      {
        return typeof(byte[]); // Typically a user-defined type
      }
      return ClrTypeMap[dbTypeName];
    }

    private static readonly Dictionary<string, NpgsqlDbType> DbTypeMap = new Dictionary<string, NpgsqlDbType>
                                                                           {
                                                                             {"ARRAY", NpgsqlDbType.Array},
                                                                             {"bigint", NpgsqlDbType.Bigint},
                                                                             {"bit", NpgsqlDbType.Bit},
                                                                             {"bit varying", NpgsqlDbType.Bit},
                                                                             {"boolean", NpgsqlDbType.Boolean},
                                                                             {"box", NpgsqlDbType.Box},
                                                                             {"bytea", NpgsqlDbType.Bytea},
                                                                             {"char", NpgsqlDbType.Char},
                                                                             {"character", NpgsqlDbType.Char},
                                                                             {"character varying", NpgsqlDbType.Varchar},
                                                                             {"cidr", NpgsqlDbType.Inet},
                                                                             {"circle", NpgsqlDbType.Circle},
                                                                             {"decimal", NpgsqlDbType.Numeric},
                                                                             {"double precision", NpgsqlDbType.Double},
                                                                             {"inet", NpgsqlDbType.Inet},
                                                                             {"integer", NpgsqlDbType.Integer},
                                                                             {"interval", NpgsqlDbType.Interval},
                                                                             {"line", NpgsqlDbType.Line},
                                                                             {"lseg", NpgsqlDbType.LSeg},
                                                                             {"macaddr", NpgsqlDbType.Bytea}, // TODO: May be a better type
                                                                             {"money", NpgsqlDbType.Money},
                                                                             {"numeric", NpgsqlDbType.Numeric},
                                                                             {"oid", NpgsqlDbType.Oidvector}, // TODO: not sure about this one
                                                                             {"point", NpgsqlDbType.Point},
                                                                             {"polygon", NpgsqlDbType.Polygon},
                                                                             {"path", NpgsqlDbType.Path},
                                                                             {"real", NpgsqlDbType.Real},
                                                                             {"smallint", NpgsqlDbType.Smallint},
                                                                             {"text", NpgsqlDbType.Text},
                                                                             {"time", NpgsqlDbType.Time},
                                                                             {"time with time zone", NpgsqlDbType.TimeTZ},
                                                                             {"time without time zone", NpgsqlDbType.Time},
                                                                             {"timestamp", NpgsqlDbType.Timestamp},
                                                                             {"timestamp with time zone", NpgsqlDbType.TimestampTZ},
                                                                             {"timestamp without time zone", NpgsqlDbType.Timestamp},
                                                                             {"uuid", NpgsqlDbType.Uuid},
                                                                             {"varchar", NpgsqlDbType.Varchar}
                                                                           };


    private static readonly Dictionary<string, Type> ClrTypeMap = new Dictionary<string, Type>
                                                                          {
                                                                            {NpgsqlDbType.Abstime.ToString(), typeof (DateTime)},
                                                                            //TODO {NpgsqlDbType.Array.ToString(), typeof ()},
                                                                            {NpgsqlDbType.Bigint.ToString(), typeof (long)},
                                                                            //TODO {NpgsqlDbType.Bit.ToString(), typeof ()},
                                                                            {NpgsqlDbType.Boolean.ToString(), typeof (bool)},
                                                                            {NpgsqlDbType.Box.ToString(), typeof (NpgsqlBox)}, // TODO: Not sure how these Npgsql types will work.  Write some tests.
                                                                            {NpgsqlDbType.Bytea.ToString(), typeof (byte[])},
                                                                            {NpgsqlDbType.Char.ToString(), typeof (char)},
                                                                            {NpgsqlDbType.Circle.ToString(), typeof (NpgsqlCircle)},
                                                                            {NpgsqlDbType.Date.ToString(), typeof (DateTime)},
                                                                            {NpgsqlDbType.Double.ToString(), typeof (double)},
                                                                            {NpgsqlDbType.Inet.ToString(), typeof (NpgsqlInet)},
                                                                            {NpgsqlDbType.Integer.ToString(), typeof (int)},
                                                                            {NpgsqlDbType.Interval.ToString(), typeof (TimeSpan)},
                                                                            {NpgsqlDbType.Line.ToString(), typeof (NpgsqlPath)},
                                                                            {NpgsqlDbType.LSeg.ToString(), typeof (NpgsqlLSeg)},
                                                                            {NpgsqlDbType.Money.ToString(), typeof (double)},
                                                                            {NpgsqlDbType.Name.ToString(), typeof (string)},
                                                                            {NpgsqlDbType.Numeric.ToString(), typeof (double)},
                                                                            {NpgsqlDbType.Oidvector.ToString(), typeof (int)},
                                                                            {NpgsqlDbType.Path.ToString(), typeof (NpgsqlPath)},
                                                                            {NpgsqlDbType.Point.ToString(), typeof (NpgsqlPoint)},
                                                                            {NpgsqlDbType.Polygon.ToString(), typeof (NpgsqlPolygon)},
                                                                            {NpgsqlDbType.Real.ToString(), typeof (float)},
                                                                            // TODO {NpgsqlDbType.Refcursor.ToString(), typeof ()},
                                                                            {NpgsqlDbType.Smallint.ToString(), typeof (short)},
                                                                            {NpgsqlDbType.Text.ToString(), typeof (string)},
                                                                            {NpgsqlDbType.Time.ToString(), typeof (DateTime)},
                                                                            {NpgsqlDbType.Timestamp.ToString(), typeof (DateTime)},
                                                                            {NpgsqlDbType.TimestampTZ.ToString(), typeof (DateTime)},
                                                                            {NpgsqlDbType.TimeTZ.ToString(), typeof (DateTime)},
                                                                            {NpgsqlDbType.Uuid.ToString(), typeof (Guid)},
                                                                            {NpgsqlDbType.Varchar.ToString(), typeof (string)},
                                                                            {NpgsqlDbType.Xml.ToString(), typeof (string)},
                                                                          };
  }
}