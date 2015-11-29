namespace VectorImageEdit.Modules.Interfaces
{
    /// <summary>
    // Interface for every kind of importer
    /// </summary>
    /// <typeparam name="TParams"> Type of adjustable parameter(s) </typeparam>
    /// <typeparam name="TOutData"> Type of output data </typeparam>
    interface IGenericResourceImporter<in TParams, out TOutData>
    {
        TOutData Acquire(string resourcePath);
        TOutData Acquire(string[] resourcePath);

        bool Validate(string resourcePath);
        bool Validate(string[] resourcePath);

        TParams ImportParameters { set; }
    }
}