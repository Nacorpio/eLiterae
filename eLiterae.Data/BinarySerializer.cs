using System.IO;
using eLiterae.Data.Extensions;

namespace eLiterae.Data
{
    public static class BinarySerializer
    {
        /// <summary>
        /// Serializes a specific object into a byte array.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static byte[] Serialize<T>(T obj) where T : new()
        {
            return obj.ToByteArray();
        }

        /// <summary>
        /// Deserializes an object from a specific <see cref="Stream"/>.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static T Deserialize<T>(BinaryReader stream) where T : new()
        {
            return stream.ToObject<T>();
        }

        /// <summary>
        /// Deserializes an object from a specific byte array.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] bytes) where T : new()
        {
            return Deserialize<T>(new BinaryReader(new MemoryStream(bytes)));
        }
    }
}
