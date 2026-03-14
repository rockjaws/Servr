using Servr.Domain.Enum;

namespace Servr.Infrastructure.Data;

public static class Data
{
    public static List<MenuCategory> GetCategories()
    {
        return Enum.GetValues<MenuCategory>().ToList();
    }
}
