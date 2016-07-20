using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using eLiterae.Data.Extensions;

namespace eLiterae.Data
{
    public partial class DataStream
    {
        /// <summary>
        /// Reads a pair with a specific key- and value type.
        /// </summary>
        /// <typeparam name="T">The key type.</typeparam>
        /// <typeparam name="T1">The value type.</typeparam>
        /// <returns></returns>
        public KeyValuePair<T, T1> ReadKeyValuePair<T, T1>()
        {
            var result = Read<KeyValuePair<object, object>>();
            return new KeyValuePair<T, T1>((T)result.Key, (T1)result.Key);
        }

        /// <summary>
        /// Reads a dictionary with a specific key- and value type.
        /// </summary>
        /// <typeparam name="T">The key type.</typeparam>
        /// <typeparam name="T1">The value type.</typeparam>
        /// <returns></returns>
        public Dictionary<T, T1> ReadDictionary<T, T1>()
        {
            return Read<Dictionary<object, object>>()
                .ToDictionary(
                    x => (T)x.Key,
                    y => (T1)y.Value);
        }

        /// <summary>
        /// Reads an object of a specific <see cref="TypeCode"/>.
        /// </summary>
        /// <param name="typeCode">The type code.</param>
        /// <returns></returns>
        public object Read(TypeCode typeCode)
        {
            object result = null;
            switch (typeCode)
            {
                case TypeCode.Int16:
                    result = Read<short>();
                    break;

                case TypeCode.Int32:
                    result = Read<int>();
                    break;

                case TypeCode.Int64:
                    result = Read<long>();
                    break;

                case TypeCode.Double:
                    result = Read<double>();
                    break;

                case TypeCode.Single:
                    result = Read<float>();
                    break;

                case TypeCode.Char:
                    result = Read<char>();
                    break;

                case TypeCode.String:
                    result = Read<string>();
                    break;
            }
            return result;
        }

        /// <summary>
        /// Reads an object of a specific type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns></returns>
        public T Read<T>()
        {
            return (T)ReadObject<T>();
        }

        /// <summary>
        /// Reads a list of a specific type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns></returns>
        public List<T> ReadList<T>()
        {
            return Read<List<object>>().Cast<T>().ToList();
        }

        /// <summary>
        /// Reads an object of a specific type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns></returns>
        public object ReadObject<T>()
        {
            var type = typeof(T);

            if (Constants.TypeConverters.ContainsKey(type) &&
                Constants.ByteCounts.ContainsKey(type))
            {
                return Constants.TypeConverters[type].Invoke(new BinaryReader(_stream));
            }

            if (type == typeof(string))
            {
                var encoding = ((EncodingType)Read()).ToEncoding();
                var length = Read<int>();
                var byteCount = encoding.GetByteCount(new[] { 'a' });

                var chars = new List<char>();

                for (var i = 0; i < byteCount * length; i += byteCount)
                {
                    chars.Add(Read<char>());
                }

                return new string(chars.ToArray());
            }

            if (type.IsArray)
            {

            }

            if (type.IsGenericType &&
                (type.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                var length = Read<int>();
                var byteCount = Read<int>();
                var typeCode = (TypeCode) Read();

                var elements = new List<object>();

                for (var i = 0; i < byteCount * length; i += byteCount)
                {
                    elements.Add(Read(typeCode));
                }

                return elements;
            }

            if (type.IsGenericType &&
                (type.GetGenericTypeDefinition() == typeof(Dictionary<,>)))
            {
                var typeCode0 = (TypeCode) Read();
                var typeCode1 = (TypeCode) Read();

                var length = Read<int>();

                var pairs = new Dictionary<object, object>();

                for (var i = 0; i < length; i++)
                {
                    pairs.Add(Read(typeCode0), Read(typeCode1));
                }

                return pairs;
            }

            return null;
        }

        /// <summary>
        /// Reads one byte from the <see cref="DataStream"/>.
        /// </summary>
        /// <returns></returns>
        public byte Read()
        {
            return Read(1)[0];
        }

        /// <summary>
        /// Reads a specific amount of bytes from the <see cref="DataStream"/>.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public byte[] Read(int count)
        {
            var buffer = new byte[count];
            _stream.Read(buffer, 0, count);

            return buffer;
        }
    }
}
