using Data_Organizer.MVVM.ViewModels;

namespace Data_Organizer.MVVM.Views;

public partial class SupportPage : ContentPage
{
    private SupportPageViewModel _supportPageViewModel;

    public SupportPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        if (_supportPageViewModel == null)
            _supportPageViewModel = (SupportPageViewModel)BindingContext;

        _supportPageViewModel.IsLoading = false;

        base.OnAppearing();
    }
}