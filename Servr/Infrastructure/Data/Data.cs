using Servr.Domain.Enum;
using Servr.Domain.Interface;
using Servr.Domain.Model;

namespace Servr.Infrastructure.Data;

public static class Data
{
    public static List<MenuCategory> GetCategories()
    {
        return Enum.GetValues<MenuCategory>().ToList();
    }

    public static List<IMenuItem> GetStarters()
    {
        return new List<IMenuItem>
        {
            new MenuItem("Bruschetta", 45m, "Toasted bread with tomato and basil", "Starters"),
            new MenuItem("Garlic Bread", 35m, "With melted mozzarella", "Starters"),
            new MenuItem("Soup of the Day", 55m, "Ask your server", "Starters"),
        };
    }

    public static List<IMenuItem> GetMainCourses()
    {
        return new List<IMenuItem>
        {
            new MenuItem("Burger", 89m, "Classic beef burger with fries", "MainCourses"),
            new MenuItem("Steak", 159m, "200g ribeye with pepper sauce", "MainCourses"),
            new MenuItem("Pasta Carbonara", 95m, "Pancetta, egg, parmesan", "MainCourses"),
            new MenuItem("Grilled Salmon", 129m, "With dill sauce and potatoes", "MainCourses"),
        };
    }

    public static List<IMenuItem> GetSides()
    {
        return new List<IMenuItem>
        {
            new MenuItem("Fries", 30m, "Crispy french fries", "Sides"),
            new MenuItem("Salad", 35m, "Mixed green salad", "Sides"),
            new MenuItem("Onion Rings", 35m, "Beer battered", "Sides"),
        };
    }

    public static List<IMenuItem> GetDesserts()
    {
        return new List<IMenuItem>
        {
            new MenuItem("Brownie", 55m, "Warm chocolate brownie with ice cream", "Desserts"),
            new MenuItem("Cheesecake", 60m, "New York style", "Desserts"),
            new MenuItem("Ice Cream", 40m, "Three scoops", "Desserts"),
        };
    }

    public static List<IDrink> GetSoftDrinks()
    {
        return new List<IDrink>
        {
            new Drink("Cola", 25m, "Coca Cola", 33),
            new Drink("Fanta", 25m, "Orange Fanta", 33),
            new Drink("Juice", 30m, "Fresh orange juice", 25),
            new Drink("Water", 15m, "Still water", 50),
        };
    }

    public static List<IDrink> GetAlcoholicDrinks()
    {
        return new List<IDrink>
        {
            new Drink("Vodka", 45m, "Classic vodka", 4),
            new Drink("Beer", 55m, "Draft pilsner", 50),
            new Drink("White Wine", 65m, "Chardonnay", 18),
            new Drink("Red Wine", 65m, "Merlot", 18),
            new Drink("Gin & Tonic", 75m, "With cucumber", 25),
        };
    }
}
