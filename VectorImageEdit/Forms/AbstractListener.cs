namespace VectorImageEdit.Forms
{
    /// <summary>
    /// Common Base Listener class
    /// 
    /// - contains a reference to parent controller that manages this listener
    /// </summary>
    /// <typeparam name="TController"> Controller typename </typeparam>
    abstract class AbstractListener<TController>
    {
        protected readonly TController Controller;

        protected AbstractListener(TController controller)
        {
            Controller = controller;
        }
    }
}
