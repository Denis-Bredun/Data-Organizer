using Data_Organizer.MVVM.ViewModels;

namespace Data_Organizer.MVVM.Views;

public partial class ResetPasswordPage : ContentPage
{
    private ResetPasswordPageViewModel _resetPasswordPageViewModel;

    public ResetPasswordPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        if (_resetPasswordPageViewModel == null)
            _resetPasswordPageViewModel = (ResetPasswordPageViewModel)BindingContext;

        _resetPasswordPageViewModel.IsLoading = false;

        base.OnAppearing();
    }
}