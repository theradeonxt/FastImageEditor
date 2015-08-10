namespace VectorImageEdit.Forms
{
    abstract class AbstractListener<TController>
    {
        protected readonly TController Controller;

        protected AbstractListener(TController controller)
        {
            Controller = controller;
        }
    }
}
