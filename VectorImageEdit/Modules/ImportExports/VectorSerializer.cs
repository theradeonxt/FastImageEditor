using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VectorImageEdit.Modules.ImportExports
{
    class VectorSerializer
    {
         /// <summary>
         /// Deserializes the workspace layers from a given file
         /// </summary>
         ///// <param name="fileName"> Input file </param>
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
    }
}
