using MonopolyMAUI.ViewModel;

namespace MonopolyMAUI.View;

public partial class RulesPage : ContentPage
{
	public RulesPage(RulesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}