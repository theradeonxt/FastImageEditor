namespace VectorImageEdit.WindowsFormsBridge
{
    interface IFormDialogFactory<out T>
    {
        T CreateDialog(params object[] dialogParameters);
    }
}
