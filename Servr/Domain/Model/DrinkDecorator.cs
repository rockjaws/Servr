using Servr.Domain.Interface;

namespace Servr.Domain.Model;

public abstract class DrinkDecorator : IDrink
{
    protected readonly IDrink _drink;
    public IDrink WrappedDrink => _drink;

    protected DrinkDecorator(IDrink drink)
    {
        _drink = drink;
    }

    public virtual string Name => _drink.Name;
    public virtual decimal Price => _drink.Price;
    public virtual string Description => _drink.Description;
    public virtual int Volume => _drink.Volume;
}
