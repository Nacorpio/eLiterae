using System;
using System.Linq;
using System.Reflection;
using eLiterae.Data.Attributes;

namespace eLiterae.Data.Extensions
{
    internal static class ReflectionExtensions
    {
        /// <summary>
        /// Determines whether a specific type can be serialized.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool CanBeSerialized(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.GetPropertiesWithAttribute<BinaryProperty>().Any();
        }

        /// <summary>
        /// Creates an instance of the specific type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="genericTypes">The generic types.</param>
        /// <returns></returns>
        public static T CreateGenericInstance<T>(params Type[] genericTypes)
        {
            if (genericTypes == null)
                throw new ArgumentNullException(nameof(genericTypes));

            if (!typeof(T).IsGenericType)
                return default(T);

            return (T) CreateGenericInstance(typeof(T), genericTypes);
        }

        /// <summary>
        /// Creates an instance of the specific type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="genericTypes">The generic types.</param>
        /// <returns></returns>
        public static object CreateGenericInstance(this Type type, params Type[] genericTypes)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (genericTypes == null)
                throw new ArgumentNullException(nameof(genericTypes));

            if (!type.IsGenericType)
                throw new ArgumentException("The type isn't generic.", nameof(type));

            return Activator.CreateInstance(type.MakeGenericType(genericTypes));
        }

        /// <summary>
        /// Gets an array of a specific <see cref="Attribute"/> located in a specific <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static T[] GetAttributes<T>(this Type type) where T : Attribute
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.GetCustomAttributes(typeof(T), false)
                .Select(x => (T)x)
                .ToArray();
        }

        /// <summary>
        /// Gets an array of a specific <see cref="Attribute"/> located in a specific <see cref="MemberInfo"/>.
        /// </summary>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <param name="memberInfo">The member info.</param>
        /// <returns></returns>
        public static T[] GetAttributes<T>(this MemberInfo memberInfo) where T : Attribute
        {
            if (memberInfo == null)
                throw new ArgumentNullException(nameof(memberInfo));

            return memberInfo.GetCustomAttributes(typeof(T), false)
                .Select(x => (T)x)
                .ToArray();
        }

        /// <summary>
        /// Returns the members with an attribute of a specific type.
        /// </summary>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static MemberInfo[] GetMembersWithAttribute<T>(this Type type) where T : Attribute
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return type.GetMembers()
                .Where(x => x.GetCustomAttributes(typeof(T), false).Any())
                .ToArray();
        }

        /// <summary>
        /// Returns the properties with an attribute of a specific type.
        /// </summary>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static PropertyInfo[] GetPropertiesWithAttribute<T>(this Type type) where T : Attribute
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return GetMembersWithAttribute<T>(type)
                .OfType<PropertyInfo>()
                .ToArray();
        }
    }
}
