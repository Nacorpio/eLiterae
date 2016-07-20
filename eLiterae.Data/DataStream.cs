using System.IO;

namespace eLiterae.Data
{
    /// <summary>
    /// Represents a stream which allows writing and reading of dynamic types.
    /// </summary>
    public partial class DataStream
    {
        private readonly Stream _stream;

        /// <summary>
        /// Initializes an instance of the <see cref="DataStream"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public DataStream(Stream stream)
        {
            _stream = stream;
        }
    }
}
