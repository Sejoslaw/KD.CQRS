namespace KD.CQRS.Core
{
    /// <summary>
    /// Query is responsible for retrieving specified data.
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public interface IQuery<in TIn, out TOut>
    {
        TOut Execute(TIn input);
    }
}
