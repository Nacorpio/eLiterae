using System;

namespace eLiterae.Data.Utilities
{
    /// <summary>
    /// Represents a collection of array utilities.
    /// </summary>
    public static class ArrayUtil
    {
        /// <summary>
        /// Creates an array from the base type of a specific object array.
        /// </summary>
        /// <param name="value">The object array.</param>
        /// <returns></returns>
        public static Array RefineArray(object[] value)
        {
            var array = CreateArray(value[0].GetType(), value.Length);
            Array.Copy(value, array, value.Length);

            return array;
        }

        /// <summary>
        /// Creates an array of a specific type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static Array CreateArray(Type type, int length)
        {
            return Array.CreateInstance(type, length);
        }

        /// <summary>
        /// Creates an array of a specific type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static T[] CreateArray<T>(int length)
        {
            return (T[])CreateArray(typeof(T), length);
        }
    }
}
