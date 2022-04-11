namespace Restaurant.Kitchen.Exceptions
{
    [Serializable]
    public class LasagnaException : Exception
    {
        public LasagnaException()
        {
        }

        public LasagnaException(string message) : base(message)
        {
        }

        public LasagnaException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
