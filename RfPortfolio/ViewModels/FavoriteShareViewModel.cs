using System.Collections.ObjectModel;
using ReactiveUI;

namespace RfPortfolio.ViewModels
{
    public class FavoriteShareViewModel : ViewModelBase
    {
        protected FavoriteShareItemViewModel? _selectedShare;

        public ObservableCollection<FavoriteShareItemViewModel> Shares { get; } = new();

        public FavoriteShareItemViewModel? SelectedShare
        {
            get => _selectedShare;
            set => this.RaiseAndSetIfChanged(ref _selectedShare, value);
        }

        public FavoriteShareViewModel()
        {
            Shares.Add(new FavoriteShareItemViewModel { Name = "Test1" });
            Shares.Add(new FavoriteShareItemViewModel { Name = "Test2" });
        }
    }
}