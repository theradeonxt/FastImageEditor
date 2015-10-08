using System;
using NLog;
using VectorImageEdit.Interfaces;

namespace VectorImageEdit.Modules.ImportExports
{
    /// <summary>
    /// Base class for every kind of importer
    /// </summary>
    /// <typeparam name="TParams"> Type of adjustable parameter(s) </typeparam>
    /// <typeparam name="TOutData"> Type of output data </typeparam>
    internal abstract class GenericResourceImporter<TParams, TOutData>
        : IGenericResourceImporter<TParams, TOutData>
    {
        // ReSharper disable once StaticMemberInGenericType
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected bool InternalErrorValidation(Action concreteImporterCallback)
        {
            try
            {
                concreteImporterCallback();
                return true;
            }
            catch (Exception ex)
            {
                // There are too many types of failure: no point in handling them individually.
                // Any error will be caught here and logged.
                Logger.Error(ex.ToString());
                return false;
            }
        }

        public abstract TOutData Acquire(string resourcePath);
        public abstract TOutData Acquire(string[] resourcePath);
        public abstract bool Validate(string resourcePath);
        public abstract bool Validate(string[] resourcePath);

        public TParams ImportParameters { protected get; set; }
    }
}
