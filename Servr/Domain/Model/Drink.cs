using Servr.Domain.Interface;

namespace Servr.Domain.Model
{
    public class Drink : IDrink
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public int Volume { get; private set; }

        public Drink(string name, decimal price, string description, int dlVolume)
        {
            Name = name;
            Price = price;
            Description = description;
            Volume = dlVolume;
        }
    }
}
