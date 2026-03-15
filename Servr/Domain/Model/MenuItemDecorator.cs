using Servr.Domain.Interface;

namespace Servr.Domain.Model;

public abstract class MenuItemDecorator : IMenuItem
{
    protected readonly IMenuItem _menuItem;

    protected MenuItemDecorator(IMenuItem menuItem)
    {
        _menuItem = menuItem;
    }

    public virtual string Name => _menuItem.Name;
    public virtual decimal Price => _menuItem.Price;
    public virtual string Description => _menuItem.Description;
    public virtual string Category => _menuItem.Category;
}
