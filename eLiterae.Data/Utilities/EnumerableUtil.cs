using System;
using System.Collections;
using System.Collections.Generic;
using eLiterae.Data.Extensions;

namespace eLiterae.Data.Utilities
{
    /// <summary>
    /// Represents a collection of <see cref="IEnumerable{T}"/> utilities.
    /// </summary>
    public static class EnumerableUtil
    {
        /// <summary>
        /// Creates a <see cref="ICollection{T}"/> with a specific element type.
        /// </summary>
        /// <param name="type">The element type.</param>
        /// <returns></returns>
        public static ICollection CreateCollection(Type type)
        {
            return (ICollection) typeof(ICollection<>).CreateGenericInstance(type);
        }

        /// <summary>
        /// Creates a <see cref="IList{T}"/> with a specific element type.
        /// </summary>
        /// <param name="type">The element type.</param>
        /// <returns></returns>
        public static IList CreateList(Type type)
        {
            return (IList) typeof(IList<>).CreateGenericInstance(type);
        }

        /// <summary>
        /// Creates a <see cref="IEnumerable{T}"/> with a specific element type.
        /// </summary>
        /// <param name="type">The element type.</param>
        /// <returns></returns>
        public static IEnumerable CreateEnumerable(Type type)
        {
            return (IEnumerable) typeof(IEnumerable<>).CreateGenericInstance(type);
        }
    }
}
