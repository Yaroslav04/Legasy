using Legasy.Core.ViewModel;

namespace Legasy;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
        BindingContext = new MainPageViewModel();
    }
}

