namespace KD.CQRS.Core
{
    /// <summary>
    /// Command is responsible for executing some action / logic.
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    public interface ICommand<in TIn>
    {
        object Execute(TIn source);
    }
}
