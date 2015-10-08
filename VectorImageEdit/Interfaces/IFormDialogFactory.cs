using System;

namespace VectorImageEdit.Interfaces
{
    interface IFormDialogFactory<T>
    {
        Tuple<T> CreateDialog(params object[] dialogParameters);
    }
}
