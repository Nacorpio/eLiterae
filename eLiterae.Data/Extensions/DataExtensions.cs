using System;
using System.IO;
using System.Text;

namespace eLiterae.Data.Extensions
{
    /// <summary>
    /// Represents a collection of extensions for serializing and deserializing objects.
    /// </summary>
    public static partial class DataExtensions
    {
        /// <summary>
        /// Reads a specific amount of bytes from a specific <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="count">The amount of bytes to read.</param>
        /// <returns></returns>
        public static byte[] ReadBytes(this BinaryReader stream, int count)
        {
            var buffer = new byte[count];
            stream.Read(buffer, 0, count);

            return buffer;
        }

        /// <summary>
        /// Converts a specific type code to a <see cref="Type"/>.
        /// </summary>
        /// <param name="typeCode">The type code.</param>
        /// <returns></returns>
        public static Type ToType(this TypeCode typeCode)
        {
            return Type.GetType("System." + typeCode);
        }

        /// <summary>
        /// Reads an object with a specific <see cref="TypeCode"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="typeCode">The type code.</param>
        /// <returns></returns>
        public static object ReadObject(this BinaryReader stream, TypeCode typeCode)
        {
            var result = (object) null;
            switch (typeCode)
            {
                case TypeCode.Int16:
                    result = ToObject<short>(stream);
                    break;

                case TypeCode.Int32:
                    result = ToObject<int>(stream);
                    break;

                case TypeCode.Int64:
                    result = ToObject<long>(stream);
                    break;

                case TypeCode.Char:
                    result = ToObject<char>(stream);
                    break;

                case TypeCode.Boolean:
                    result = ToObject<bool>(stream);
                    break;

                case TypeCode.Single:
                    result = ToObject<float>(stream);
                    break;

                case TypeCode.Double:
                    result = ToObject<double>(stream);
                    break;

                case TypeCode.UInt16:
                    result = ToObject<ushort>(stream);
                    break;

                case TypeCode.UInt32:
                    result = ToObject<uint>(stream);
                    break;

                case TypeCode.UInt64:
                    result = ToObject<ulong>(stream);
                    break;
            }
            return result;
        }

        /// <summary>
        /// Converts a specific <see cref="EncodingType"/> to an <see cref="Encoding"/>.
        /// </summary>
        /// <param name="type">The encoding type.</param>
        /// <returns></returns>
        public static Encoding ToEncoding(this EncodingType type)
        {
            Encoding result = null;
            switch (type)
            {
                case EncodingType.Ascii:
                    result = Encoding.ASCII;
                    break;

                case EncodingType.Unicode:
                    result = Encoding.Unicode;
                    break;

                case EncodingType.Default:
                    result = Encoding.Unicode;
                    break;
            }
            return result;
        }

        /// <summary>
        /// Converts the bytes from a specific stream into a <see cref="string"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static string ToString(this BinaryReader stream)
        {
            var result = string.Empty;

            var encoding = ((EncodingType) stream.ReadBytes(1)[0]).ToEncoding();
            var length = stream.ReadBytes(4).ToObject<int>();

            var byteCount = encoding.GetByteCount(new[] {'a'});

            for (var i = 0; i < length; i++)
            {
                result += stream.ReadBytes(byteCount).ToObject<char>();
            }

            return result;
        }
    }
}
