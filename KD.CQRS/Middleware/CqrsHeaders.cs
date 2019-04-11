namespace KD.CQRS.Middleware
{
    /// <summary>
    /// Contains headers used by middleware.
    /// </summary>
    public static class CqrsHeaders
    {
        public const string CQRS_COMMAND = "cqrs-command";
        public const string CQRS_QUERY = "cqrs-query";
    }
}
