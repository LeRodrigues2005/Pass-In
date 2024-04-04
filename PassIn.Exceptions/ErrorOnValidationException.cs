using PassIn.Exception;

namespace PassIn.Exceptions
{
    public class ErrorOnValidationException : PassInException
    {
        public ErrorOnValidationException(string message) : base(message)
        {
        }
    }
}