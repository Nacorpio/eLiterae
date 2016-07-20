using System;
using System.Collections.Generic;
using System.IO;
using eLiterae.Data.Extensions;

namespace eLiterae.Data
{
    internal static class Constants
    {
        /// <summary>
        /// Represents a collection of byte array converters.
        /// </summary>
        public static Dictionary<Type, Func<object, byte[]>> ByteArrayConverters = new Dictionary<Type, Func<object, byte[]>>
        {
            {typeof(short), o => BitConverter.GetBytes((short) o)},
            {typeof(int), o => BitConverter.GetBytes((int) o)},
            {typeof(long), o => BitConverter.GetBytes((long) o)},
            {typeof(char), o => BitConverter.GetBytes((char) o)},
            {typeof(double), o => BitConverter.GetBytes((double) o)},
            {typeof(float), o => BitConverter.GetBytes((float) o)},
            {typeof(bool), o => BitConverter.GetBytes((bool) o)},
            {typeof(ushort), o => BitConverter.GetBytes((ushort) o)},
            {typeof(uint), o => BitConverter.GetBytes((uint) o)},
            {typeof(ulong), o => BitConverter.GetBytes((ulong) o)},
            {typeof(string), o => ((string) o).ToByteArray(EncodingType.Default)},
            {typeof(string[]), o => ((string[]) o).ToByteArray()}
        };

        /// <summary>
        /// Represents a collection of type converters.
        /// </summary>
        public static Dictionary<Type, Func<BinaryReader, object>> TypeConverters = new Dictionary<Type, Func<BinaryReader, object>>
        {
            {typeof(short), s => BitConverter.ToInt16(s.ReadBytes(sizeof(short)), 0)},
            {typeof(int), s => BitConverter.ToInt32(s.ReadBytes(sizeof(int)), 0)},
            {typeof(long), s => BitConverter.ToInt64(s.ReadBytes(sizeof(long)), 0)},
            {typeof(char), s => BitConverter.ToChar(s.ReadBytes(sizeof(char)), 0)},
            {typeof(double), s => BitConverter.ToDouble(s.ReadBytes(sizeof(double)), 0)},
            {typeof(float), s => BitConverter.ToSingle(s.ReadBytes(sizeof(float)), 0)},
            {typeof(bool), s => BitConverter.ToBoolean(s.ReadBytes(sizeof(bool)), 0)},
            {typeof(ushort), s => BitConverter.ToUInt16(s.ReadBytes(sizeof(ushort)), 0)},
            {typeof(uint), s => BitConverter.ToUInt32(s.ReadBytes(sizeof(uint)), 0)},
            {typeof(ulong), s => BitConverter.ToUInt64(s.ReadBytes(sizeof(ulong)), 0)},
            {typeof(Dictionary<,>), s => null},
            {typeof(KeyValuePair<,>), s => s.ToKeyValuePair(typeof(object), typeof(object))},
            {typeof(string), DataExtensions.ToString},
        };

        /// <summary>
        /// Represents a collection of byte counts paired by type.
        /// </summary>
        public static Dictionary<Type, int> ByteCounts = new Dictionary<Type, int>
        {
            {typeof(short), sizeof(short)},
            {typeof(int), sizeof(int)},
            {typeof(long), sizeof(long)},
            {typeof(char), sizeof(char)},
            {typeof(double), sizeof(double)},
            {typeof(float), sizeof(float)},
            {typeof(bool), sizeof(bool)},
            {typeof(ushort), sizeof(ushort)},
            {typeof(uint), sizeof(uint)},
            {typeof(ulong), sizeof(ulong)}
        };
    }
}
