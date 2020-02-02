namespace WeLoveTaxes.Domain.Exception
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class WeLoveTaxesDomainException : System.Exception
    {
        public WeLoveTaxesDomainException()
        { }

        public WeLoveTaxesDomainException(string message)
            : base(message)
        { }

        public WeLoveTaxesDomainException(string message, System.Exception innerException)
            : base(message, innerException)
        { }
    }
}