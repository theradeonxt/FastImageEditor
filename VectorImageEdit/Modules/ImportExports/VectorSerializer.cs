using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VectorImageEdit.Modules.ImportExports
{
    class VectorSerializer
    {
        static VectorSerializer()
        {
            SupportedFileExt = ".vdata";
        }

        /// <summary>
        /// Serializes the workspace layers into a vector format to a given file
        /// </summary>
        /// <param name="fileName"> Input file </param>
        public void Serialize(string fileName)
        {
            if (Source == null) throw new InvalidOperationException("Source data was not specified");
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                formatter.Serialize(stream, Source);
            }
        }

        public IList Deserialize(string fileName)
        {
            IList layers;
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                layers = (IList)formatter.Deserialize(stream);
            }
            if (layers == null) throw new InvalidDataException("Deserialization into " + layers.GetType() + " failed.");
            return layers;
        }

        public IList Source { get; set; }

        public static string SupportedFileExt { get; private set; }
    }
}
