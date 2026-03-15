using Servr.Domain.Interface;

namespace Servr.Domain.Model;

public class DrinkExtra : DrinkDecorator
{
    private readonly string _extraName;
    private readonly decimal _extraPrice;

    public DrinkExtra(IDrink drink, string extraName, decimal extraPrice)
        : base(drink)
    {
        _extraName = extraName;
        _extraPrice = extraPrice;
    }

    public override string Name => $"{_drink.Name} with {_extraName}";
    public override decimal Price => _drink.Price + _extraPrice;
}
