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

  public static List<IItem> GetStarters()
  {
    return new List<IItem>
        {
            new MenuItem("Bruschetta", 45m, "Toasted bread with tomato and basil", "Starters"),
            new MenuItem("Garlic Bread", 35m, "With melted mozzarella", "Starters"),
            new MenuItem("Soup of the Day", 55m, "Ask your server", "Starters"),
        };
  }

  public static List<IItem> GetMainCourses()
  {
    return new List<IItem>
        {
            new MenuItem("Burger", 89m, "Classic beef burger with fries", "MainCourses"),
            new MenuItem("Steak", 159m, "200g ribeye with pepper sauce", "MainCourses"),
            new MenuItem("Pasta Carbonara", 95m, "Pancetta, egg, parmesan", "MainCourses"),
            new MenuItem("Grilled Salmon", 129m, "With dill sauce and potatoes", "MainCourses"),
        };
  }

  public static List<IItem> GetSides()
  {
    return new List<IItem>
        {
            new MenuItem("Fries", 30m, "Crispy french fries", "Sides"),
            new MenuItem("Salad", 35m, "Mixed green salad", "Sides"),
            new MenuItem("Onion Rings", 35m, "Beer battered", "Sides"),
        };
  }

  public static List<IItem> GetDesserts()
  {
    return new List<IItem>
        {
            new MenuItem("Brownie", 55m, "Warm chocolate brownie with ice cream", "Desserts"),
            new MenuItem("Cheesecake", 60m, "New York style", "Desserts"),
            new MenuItem("Ice Cream", 40m, "Three scoops", "Desserts"),
        };
  }

  public static List<IItem> GetSoftDrinks()
  {
    return new List<IItem>
        {
            new Drink("Cola", 25m, "Coca Cola", 33),
            new Drink("Fanta", 25m, "Orange Fanta", 33),
            new Drink("Juice", 30m, "Fresh orange juice", 25),
            new Drink("Water", 15m, "Still water", 50),
        };
  }

  public static List<IItem> GetAlcoholicDrinks()
  {
    return new List<IItem>
        {
            new Drink("Vodka", 45m, "Classic vodka", 4),
            new Drink("Beer", 55m, "Draft pilsner", 50),
            new Drink("White Wine", 65m, "Chardonnay", 18),
            new Drink("Red Wine", 65m, "Merlot", 18),
            new Drink("Gin & Tonic", 75m, "With cucumber", 25),
        };
  }

  public static List<(string Name, decimal Price)> GetExtrasForItem(string itemName)
  {
    return ItemExtras.GetValueOrDefault(itemName, []);
  }

  private static readonly Dictionary<string, List<(string Name, decimal Price)>> ItemExtras = new()
  {
    // Starters
    ["Bruschetta"] = [("Olives", 5m), ("Extra Cheese", 10m), ("Basil", 0m)],
    ["Garlic Bread"] = [("Extra Cheese", 10m), ("Olives", 5m), ("Herbs", 0m)],
    ["Soup of the Day"] = [("Bread", 5m), ("Croutons", 5m), ("Cream", 3m)],

    // Main Courses
    ["Burger"] = [("Extra Cheese", 10m), ("Bacon", 12m), ("Extra Patty", 15m), ("Jalapeños", 5m)],
    ["Steak"] = [("Pepper Sauce", 10m), ("Garlic Butter", 8m), ("Mushrooms", 10m)],
    ["Pasta Carbonara"] = [("Extra Parmesan", 8m), ("Truffle Oil", 15m), ("Pancetta", 12m)],
    ["Grilled Salmon"] = [("Extra Dill Sauce", 8m), ("Capers", 5m), ("Lemon", 3m)],

    // Sides
    ["Fries"] = [("Cheese", 10m), ("Truffle Oil", 12m), ("Garlic", 5m)],
    ["Salad"] = [("Dressing", 5m), ("Croutons", 5m), ("Feta Cheese", 10m)],
    ["Onion Rings"] = [("Dipping Sauce", 5m), ("Cheese", 8m)],

    // Desserts
    ["Brownie"] = [("Whipped Cream", 5m), ("Extra Scoop", 10m), ("Chocolate Sauce", 5m)],
    ["Cheesecake"] = [("Whipped Cream", 5m), ("Berry Sauce", 8m), ("Chocolate Sauce", 5m)],
    ["Ice Cream"] = [("Extra Scoop", 10m), ("Chocolate Sauce", 5m), ("Sprinkles", 3m)],

    // Soft Drinks
    ["Cola"] = [("Ice", 0m), ("Lemon", 3m)],
    ["Fanta"] = [("Ice", 0m), ("Lemon", 3m)],
    ["Juice"] = [("Ice", 0m), ("Sugar", 0m)],
    ["Water"] = [("Ice", 0m), ("Lemon", 3m)],

    // Alcoholic Drinks
    ["Vodka"] = [("Ice", 0m), ("Lemon", 3m), ("Syrup", 5m)],
    ["Beer"] = [("Lemon", 3m)],
    ["White Wine"] = [("Ice", 0m)],
    ["Red Wine"] = [],
    ["Gin & Tonic"] = [("Ice", 0m), ("Lemon", 3m), ("Cucumber", 3m)],
  };
}
