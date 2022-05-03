namespace Core.Contracts
{
    public interface IRequestAccepted
    {
        Guid Guid { get; }
        DateTime DateTime { get; }
    }
}
