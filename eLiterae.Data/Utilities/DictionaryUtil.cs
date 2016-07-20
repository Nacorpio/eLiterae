using System;
using System.Collections;
using System.Collections.Generic;
using eLiterae.Data.Extensions;

namespace eLiterae.Data.Utilities
{
    /// <summary>
    /// Represents a collection of <see cref="IDictionary{TKey,TValue}"/> utilities.
    /// </summary>
    public static class DictionaryUtil
    {
        /// <summary>
        /// Creates a <see cref="IDictionary{TKey,TValue}"/> with a specific key- and value type.
        /// </summary>
        /// <param name="tKey">The key type.</param>
        /// <param name="tValue">The value type.</param>
        /// <returns></returns>
        public static IDictionary CreateDictionary(Type tKey, Type tValue)
        {
            return (IDictionary) typeof(Dictionary<,>).CreateGenericInstance(tKey, tValue);
        }
    }
}
