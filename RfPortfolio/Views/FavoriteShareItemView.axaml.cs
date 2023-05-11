using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace RfPortfolio.Views
{
    public partial class FavoriteShareItemView : UserControl
    {
        public FavoriteShareItemView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
