namespace VectorImageEdit.Interfaces
{
    /// <summary>
    /// Interface for every kind of exporter that outputs to a
    /// local resource stored on a physical storage medium.
    /// </summary>
    /// <typeparam name="TParam"> Type of adjustable parameter </typeparam>
    /// <typeparam name="TData"> Type of data source </typeparam>
    interface IDiskResourceExporter<in TParam, in TData>
    {
        bool ExportData();

        TParam ExportParameter { set; }
        TData DataSource { set; }
        string FileName { get; }
    }
}
