namespace Monopoly_class_library
{
    public class ActionResult
    {
        public bool IsSuccess { get; }
        public ErrorMessages ErrorMessage { get; }

        public ActionResult()
        {
            IsSuccess = true;
        }

        public ActionResult(ErrorMessages error)
        {
            IsSuccess = false;
            ErrorMessage = error;
        }
    }

    public enum ErrorMessages
    {
        Ok,
        MoneyNotEnough,
        AlreadyMortgaged,
        NoMortgaged,
        SetOwnershipError,
        HouseCountError,
        LandOwnershipError,
        Bankruptcy
    }
}
