namespace Servr.Domain.Interface;

public interface IItem
{
    decimal Price { get; }
    string Description { get; }
    string Name { get; }
}
