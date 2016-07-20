using System;
using System.Collections;
using System.Collections.Generic;
using eLiterae.Data.Attributes;

namespace eLiterae.Data.Extensions
{
    public static partial class DataExtensions
    {
        /// <summary>
        /// Converts a specific <see cref="IDictionary{TKey,TValue}"/> to a <see cref="byte"/> array.
        /// </summary>
        /// <typeparam name="T">The key type.</typeparam>
        /// <typeparam name="T1">The value type.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToByteArray<T, T1>(this IDictionary<T, T1> value)
        {
            var bytes = new List<byte>
            {
                (byte) Type.GetTypeCode(typeof(T)),
                (byte) Type.GetTypeCode(typeof(T1))
            };

            bytes.AddRange(value.Count.ToByteArray());

            foreach (var pair in value)
                bytes.AddRange(pair.ToByteArray());

            return bytes.ToArray();
        }

        public static byte[] ToByteArray(this IDictionary value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var enumerator = value.GetEnumerator();
            enumerator.MoveNext();

            var keyType = enumerator.Key.GetType();
            var valueType = enumerator.Value.GetType();

            var bytes = new List<byte>
            {
                (byte) Type.GetTypeCode(keyType),
                (byte) Type.GetTypeCode(valueType)
            };

            bytes.AddRange(value.Count.ToByteArray());

            foreach (var entry in value)
            {
                bytes.AddRange(ToByteArray((DictionaryEntry) entry));
            }

            return bytes.ToArray();
        }

        /// <summary>
        /// Converts a specific <see cref="DictionaryEntry"/> to a byte array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToByteArray(this DictionaryEntry value)
        {
            var bytes = new List<byte>();

            bytes.AddRange(value.Key.ToByteArray());
            bytes.AddRange(value.Value.ToByteArray());

            return bytes.ToArray();
        }

        /// <summary>
        /// Converts a specific <see cref="KeyValuePair{TKey,TValue}"/> to a <see cref="byte"/> array.
        /// </summary>
        /// <typeparam name="T">The key type.</typeparam>
        /// <typeparam name="T1">The value type.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToByteArray<T, T1>(this KeyValuePair<T, T1> value)
        {
            var bytes = new List<byte>();

            bytes.AddRange(value.Key.ToByteArray());
            bytes.AddRange(value.Value.ToByteArray());

            return bytes.ToArray();
        }

        /// <summary>
        /// Converts a specific <see cref="ICollection"/> to a byte array.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToByteArray(this ICollection value)
        {
            var bytes = new List<byte>();
            bytes.AddRange(value.Count.ToByteArray());

            var enumerator = value.GetEnumerator();

            while (enumerator.MoveNext()) 
                bytes.AddRange(enumerator.Current.ToByteArray());

            return bytes.ToArray();
        }

        /// <summary>
        /// Converts a specific <see cref="IEnumerable{T}"/> to a <see cref="byte"/> array.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToByteArray<T>(this IEnumerable<T> value)
        {
            return ToByteArray((ICollection) value);
;        }

        /// <summary>
        /// Converts a specific array of <see cref="string"/> to a <see cref="byte"/> array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string[] value)
        {
            return ToByteArray((IEnumerable<string>)value);
            ;
        }

        /// <summary>
        /// Converts a specific type to a <see cref="byte"/> array.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToByteArray<T>(this T value)
        {
            return ToByteArray((object)value);
            ;
        }

        /// <summary>
        /// Converts a specific object to a <see cref="byte"/> array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] ToByteArray(this object value)
        {
            var type = value.GetType();

            if (!Constants.ByteArrayConverters.ContainsKey(type))
            {
                if (!type.CanBeSerialized())
                {
                    if (type.IsArray)
                        return ToByteArray((ICollection)value);

                    if (type.IsGenericType)
                    {
                        if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                            return ToByteArray((IDictionary) value);
                    }
;                }

                var bytes = new List<byte>();

                var properties = type.GetPropertiesWithAttribute<BinaryProperty>();
                foreach (var property in properties)
                {
                    var byteField = property.GetAttributes<BinaryProperty>()[0];
                    var id = byteField.Id;
                    var data = property.GetValue(value).ToByteArray();

                    bytes.AddRange(id.ToByteArray());
                    bytes.AddRange(data);
                }

                return bytes.ToArray();
            }

            return Constants.ByteArrayConverters[type].Invoke(value);
        }

        /// <summary>
        /// Converts a specific string to a <see cref="byte"/> array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="encodingType">The encoding type.</param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string value, EncodingType encodingType = EncodingType.Unicode)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            var bytes = new List<byte>
            {
                (byte) encodingType
            };

            bytes.AddRange(value.Length.ToByteArray());

            var encoding = encodingType.ToEncoding();
            bytes.AddRange(encoding.GetBytes(value));

            return bytes.ToArray();
        }
    }
}
