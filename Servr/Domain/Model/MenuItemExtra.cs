using Servr.Domain.Interface;

namespace Servr.Domain.Model;

public class MenuItemExtra : MenuItemDecorator
{
    private readonly string _extraName;
    private readonly decimal _extraPrice;

    public MenuItemExtra(IMenuItem item, string extraName, decimal extraPrice)
        : base(item)
    {
        _extraName = extraName;
        _extraPrice = extraPrice;
    }

    public override string Name => $"{_menuItem.Name} + {_extraName}";
    public override decimal Price => _menuItem.Price + _extraPrice;
}
