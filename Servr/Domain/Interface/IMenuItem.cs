namespace Servr.Domain.Interface
{
    public interface IMenuItem : IItem
    {
        string Category { get; }
    }
}
