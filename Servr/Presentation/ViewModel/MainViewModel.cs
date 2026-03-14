using System.Collections.ObjectModel;
using Servr.Domain.Enum;
using Servr.Infrastructure.Data;

namespace Servr.Presentation.ViewModel
{
    public class MainViewModel
    {
        private readonly List<MenuCategory> _categories;
        public ObservableCollection<MenuCategory> CategoryOptions { get; }

        public MainViewModel()
        {
            _categories = Data.GetCategories();
            CategoryOptions = new ObservableCollection<MenuCategory>(_categories);
        }
    }
}
