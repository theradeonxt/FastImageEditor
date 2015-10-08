using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VectorImageEdit.Modules.ImportExports
{
    class VectorExporter : AbstractExporter<int, IList>
    {
        public VectorExporter(string fileName)
            : base(fileName, dummyParameterState: true)
        {
        }

        public override bool ExportData()
        {
            bool status = ExportValidator(() =>
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(FileName, FileMode.Create))
                {
                    formatter.Serialize(stream, DataSource);
                }
            });
            return status;
        }
    }
}
