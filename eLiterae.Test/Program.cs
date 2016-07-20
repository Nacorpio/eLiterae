using System.Collections.Generic;
using System.IO;
using eLiterae.Data;
using eLiterae.Data.Attributes;

namespace eLiterae.Test
{
    public static class Program
    {
        public struct Structure
        {
            [BinaryProperty(0)]
            public Dictionary<int, int> Dictionary { get; set; }

            [BinaryProperty(1)]
            public Child Child { get; set; }
        }

        public struct Child
        {
            [BinaryProperty(0)]
            public int Integer { get; set; }

            [BinaryProperty(1)]
            public int[] Array { get; set; }
        }

        static void Main(string[] args)
        {
            var structure = new Structure
            {
                Dictionary = new Dictionary<int, int>
                {
                    {0, 1337},
                    {1, 1674}
                },
                Child = new Child
                {
                    Integer = 1337,
                    Array = new [] {0, 1, 3}
                }
            };

            using (var stream = new BinaryWriter(File.OpenWrite("test.dat")))
            {
                var data = BinarySerializer.Serialize(structure);
                stream.Write(data);
            }

            using (var stream = new BinaryReader(File.OpenRead("test.dat")))
            {
                var result = BinarySerializer.Deserialize<Structure>(stream);
            }
        }
    }
}
