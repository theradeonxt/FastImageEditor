using System;
using System.IO;
using NLog;
using VectorImageEdit.Modules.Interfaces;

namespace VectorImageEdit.Modules.ImportExports
{
    /// <summary>
    /// Base class for every kind of exporter
    /// </summary>
    /// <typeparam name="TParam"> Type of adjustable parameter </typeparam>
    /// <typeparam name="TData"> Type of data source </typeparam>
    internal abstract class AbstractExporter<TParam, TData> : IDiskResourceExporter<TParam, TData>
    {
        //  ReSharper disable once StaticMemberInGenericType
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private TParam _exportParameter;
        private TData _dataSource;

        /// <summary>
        /// Initialize the filename and internal validation states for this exporter
        /// </summary>
        /// <param name="fileName"> Output File </param>
        /// <param name="dummyParameterState"> TParam ignored if this is true </param>
        protected AbstractExporter(string fileName, bool dummyParameterState = false)
        {
            FileName = fileName;
            IsValidFileName = CheckValidFileName(fileName);
            IsValidParameter = dummyParameterState;
            IsValidDataSource = false;
        }

        public TParam ExportParameter
        {
            set
            {
                _exportParameter = value;
                IsValidParameter = value != null;
            }
            protected get { return _exportParameter; }
        }
        public TData DataSource
        {
            set
            {
                _dataSource = value;
                IsValidDataSource = value != null;
            }
            protected get { return _dataSource; }
        }
        public string FileName { get; private set; }
        public abstract bool ExportData();

        protected bool ExportValidator(Action concreteExporterCallback)
        {
            if (!IsValidFileName || !IsValidParameter || !IsValidDataSource)
            {
                Logger.Warn("Export validation failed. Validation states were: " +
                            "IsValidFileName={0}, IsValidParameter={1}, IsValidDataSource={2}", IsValidFileName, IsValidParameter, IsValidDataSource);
                return false;
            }
            try
            {
                concreteExporterCallback();
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

        private bool IsValidParameter { get; set; }
        private bool IsValidDataSource { get; set; }
        private bool IsValidFileName { get; set; }
        private bool CheckValidFileName(string fileName)
        {
            return !string.IsNullOrEmpty(fileName) &&
                   !string.IsNullOrWhiteSpace(fileName) &&
                   fileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1;
        }
    }
}