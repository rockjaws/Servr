namespace Servr.Domain.Model
{
    public class MenuItem
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }

        public MenuItem(string name, decimal price, string description, string category)
        {
            Name = name;
            Price = price;
            Description = description;
            Category = category;
        }
    }
}
