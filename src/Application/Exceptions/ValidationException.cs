namespace Application.Exceptions
{
    public class ValidationException : Exception
    {
        public Dictionary<string, Dictionary<string, string[]>> Errors { get; } = new();

        public ValidationException(Dictionary<string, string[]> values)
        {
            Errors.Add("errors", values);
        }
    }
}
