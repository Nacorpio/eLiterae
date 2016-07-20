using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using eLiterae.Data.Attributes;
using eLiterae.Data.Utilities;

namespace eLiterae.Data.Extensions
{
    public static partial class DataExtensions
    {
        /// <summary>
        /// Reads an object of a specific type from the specific <see cref="Stream"/>.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static T ToObject<T>(this BinaryReader stream) where T : new()
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var type = typeof(T);
            if (Constants.TypeConverters.ContainsKey(type))
                return default(T);

            if (!type.CanBeSerialized())
                throw new Exception("The specified type can't be serialized.");
            
            var properties = type.GetPropertiesWithAttribute<BinaryProperty>();
            var result = new T();

            var i = 0;
            while (i < properties.Length)
            {
                // Convert the buffer to a 16-bit integer.
                var id = stream.ReadBytes(2)
                    .ToObject<short>();

                // Retrieve the property with the id.
                var property = properties
                    .FirstOrDefault(x => x.GetAttributes<BinaryProperty>()
                    .Any(y => y.Id == id));

                if (property == null)
                    throw new NullReferenceException("Can't find a property with that identifier.");

                dynamic obj;
                try
                {
                    obj = stream.ToObject(property.PropertyType);
                }
                catch (ArgumentException)
                {
                    obj = ToObject(stream, property.PropertyType);
                }

                var boxed = (object)result;

                if (obj.GetType().IsArray)
                    obj = ArrayUtil.RefineArray((object[])obj);

                type.GetProperty(property.Name).SetValue(boxed, obj);
                result = (T)boxed;

                i++;
            }

            return result;
        }

        /// <summary>
        /// Reads a <see cref="Dictionary{TKey,TValue}"/> from the specific <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream to read the dictionary from.</param>
        /// <returns></returns>
        public static IDictionary ToDictionary(this BinaryReader stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var type0 = ((TypeCode) stream.ReadByte()).ToType();
            var type1 = ((TypeCode) stream.ReadByte()).ToType();
            
            var count = stream.ReadBytes(sizeof(int)).ToObject<int>();

            var result = new List<KeyValuePair<object, object>>();
            for (var i = 0; i < count; i++)
            {
                result.Add(ToKeyValuePair(stream, type0, type1));
            }

            var dictionary = DictionaryUtil.CreateDictionary(type0, type1);

            foreach (var pair in result)
                dictionary.Add(pair.Key, pair.Value);

            return dictionary;
        }

        /// <summary>
        /// Reads a <see cref="DictionaryEntry"/> from the specific <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream to read the entry from.</param>
        /// <param name="keyType">The key type.</param>
        /// <param name="valueType">The value type.</param>
        /// <returns></returns>
        public static DictionaryEntry ToDictionaryEntry(this BinaryReader stream, Type keyType, Type valueType)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (keyType == null)
                throw new ArgumentNullException(nameof(keyType));

            if (valueType == null)
                throw new ArgumentNullException(nameof(valueType));

            var key = stream.ToObject(keyType);
            var value = stream.ToObject(valueType);

            return new DictionaryEntry(key, value);
        }
        
        /// <summary>
        /// Reads a <see cref="KeyValuePair{TKey,TValue}"/> from the specific <see cref="Stream"/>.
        /// </summary>
        /// <typeparam name="T">The key type.</typeparam>
        /// <typeparam name="T1">The value type.</typeparam>
        /// <param name="stream">The stream to read the pair from.</param>
        /// <returns></returns>
        public static KeyValuePair<T, T1> ToKeyValuePair<T, T1>(this BinaryReader stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var result = ToKeyValuePair(stream, typeof(T), typeof(T1));
            return new KeyValuePair<T, T1>((T) result.Key, (T1) result.Value);
        }

        /// <summary>
        /// Reads a <see cref="KeyValuePair{TKey,TValue}"/> from the specific <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream to read the pair from.</param>
        /// <param name="type1">The key type.</param>
        /// <param name="type2">The value type.</param>
        /// <returns></returns>
        public static KeyValuePair<dynamic, dynamic> ToKeyValuePair(this BinaryReader stream, Type type1, Type type2)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (type1 == null)
                throw new ArgumentNullException(nameof(type1));

            if (type2 == null)
                throw new ArgumentNullException(nameof(type2));

            var key = Convert.ChangeType(stream.ToObject(type1), type1);
            var value = Convert.ChangeType(stream.ToObject(type2), type2);

            return new KeyValuePair<dynamic, dynamic>(key, value);
        }

        /// <summary>
        /// Reads an array of a specific type from a specific <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream to read the array from.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static object[] ToArray(this BinaryReader stream, Type type)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var result = new List<object>();
            var length = stream.ReadBytes(sizeof(int))
                .ToObject<int>();

            for (var i = 0; i < length; i++)
            {
                result.Add(ToObject(stream, type.GetElementType()));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Converts the specific bytes to an object of a specific type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static T ToObject<T>(this byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            if (!Constants.TypeConverters.ContainsKey(typeof(T)))
            {}

            var test = (T)Constants.TypeConverters[typeof(T)]
                .Invoke(new BinaryReader(new MemoryStream(bytes)));

            return test;
        }

        private static dynamic ReadGeneric(BinaryReader stream, Type type)
        {
            if (type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
                return ToKeyValuePair(stream, typeof(object), typeof(object));

            if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                return ToDictionary(stream);

            return null;
        }

        /// <summary>
        /// Reads an object of a specific type from a specific <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream to read the object from.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static dynamic ToObject(this BinaryReader stream, Type type)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.IsArray)
                return ToArray(stream, type);

            if (type.IsGenericType)
                return ReadGeneric(stream, type);

            if (type.CanBeSerialized())
            {
                var method = typeof(DataExtensions).GetMethods()
                    .Where(x => x.Name == "ToObject")
                    .FirstOrDefault(x => x.IsGenericMethod);

                var newType = method?.MakeGenericMethod(type);
                return newType?.Invoke(null, new object[] {stream});
            }

            return Constants.TypeConverters[type].Invoke(stream);
        }
    }
}
