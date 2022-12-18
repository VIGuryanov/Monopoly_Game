namespace Monopoly_class_library
{
    public class ActionResult
    {
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }

        public ActionResult()
        {
            IsSuccess = true;
        }

        public ActionResult(string error)
        {
            IsSuccess = false;
            ErrorMessage = error;
        }
    }
}
