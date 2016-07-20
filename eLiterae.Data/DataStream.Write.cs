using System;
using System.Collections.Generic;
using eLiterae.Data.Extensions;

namespace eLiterae.Data
{
    public partial class DataStream
    {
        /// <summary>
        /// Writes a specific type to the <see cref="DataStream"/>.
        /// </summary>
        /// <typeparam name="T">The data type.</typeparam>
        /// <param name="input">The input.</param>
        public void Write<T>(T input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var data = input.ToByteArray();
            _stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Writes the properties of a specific type, using a specific instance of the type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="obj">The object.</param>
        public void WriteType<T>(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var type = typeof(T);

            foreach (var p in type.GetProperties())
                Write(p.GetValue(obj));
        }

        /// <summary>
        /// Writes a specific <see cref="IEnumerable{T}"/> to the <see cref="DataStream"/>.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        public void WriteEnumerable<T>(IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            var data = enumerable.ToByteArray();
            _stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Writes a specific pair to the <see cref="DataStream"/>.
        /// </summary>
        /// <typeparam name="T">The key type.</typeparam>
        /// <typeparam name="T1">The value type.</typeparam>
        /// <param name="pair">The pair.</param>
        public void WriteKeyValuePair<T, T1>(KeyValuePair<T, T1> pair)
        {
            var data = pair.ToByteArray();
            _stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Writes a specific dictionary to the <see cref="DataStream"/>.
        /// </summary>
        /// <typeparam name="T">The key type.</typeparam>
        /// <typeparam name="T1">The value type.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        public void WriteDictionary<T, T1>(IDictionary<T, T1> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            var data = dictionary.ToByteArray();
            _stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Writes a specific <see cref="string"/> value to the <see cref="DataStream"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="encoding">The encoding.</param>
        public void WriteString(string value, EncodingType encoding = EncodingType.Unicode)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var data = value.ToByteArray(encoding);
            _stream.Write(data, 0, data.Length);
        }
    }
}
