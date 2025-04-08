using Data_Organizer.MVVM.ViewModels.SignInViewModel;

namespace Data_Organizer.MVVM.Views;

public partial class SignInPage : ContentPage
{
    private SignInViewModel _signInViewModel;

    public SignInPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        if (_signInViewModel == null)
            _signInViewModel = (SignInViewModel)BindingContext;

        _signInViewModel.IsLoading = false;

        base.OnAppearing();
    }
}