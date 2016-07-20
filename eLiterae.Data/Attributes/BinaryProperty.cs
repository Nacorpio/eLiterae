using System;

namespace eLiterae.Data.Attributes
{
    /// <summary>
    /// Applies functionality for serializing a <see cref="System.Reflection.PropertyInfo"/> into binary format.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class BinaryProperty : Attribute
    {
        /// <summary>
        /// Initializes an instance of the <see cref="BinaryProperty"/> attribute.
        /// </summary>
        /// <param name="id">A unique identifier.</param>
        public BinaryProperty(short id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets a unique identifier for the <see cref="BinaryProperty"/>.
        /// </summary>
        public short Id { get; }
    }
}
