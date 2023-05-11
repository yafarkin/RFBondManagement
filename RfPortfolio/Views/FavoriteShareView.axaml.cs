using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RfFondPortfolio.Common.Interfaces;
using RfPortfolio.ViewModels;
using Unity;

namespace RfPortfolio.Views
{
    public partial class FavoriteShareView : UserControl
    {
        [Dependency]
        public IPaperRepository PaperRepository { get; set; }

        public FavoriteShareView()
        {
            InitializeComponent();
        }

        public FavoriteShareView(IPaperRepository paperRepository)
        {
            if (null == paperRepository)
            {
                return;
            }

            PaperRepository = paperRepository;

            var vm = new FavoriteShareViewModel();

            var favoritePapers = PaperRepository.Get().Where(p => p.IsFavorite).ToList();

            foreach (var favoritePaper in favoritePapers)
            {
                vm.Shares.Add(new FavoriteShareItemViewModel
                {
                    Name = favoritePaper.Name
                });
            }

            DataContext = vm;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
