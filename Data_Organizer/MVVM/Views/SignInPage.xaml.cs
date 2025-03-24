
using Data_Organizer.MVVM.ViewModels;
using Data_Organizer.MVVM.ViewModels.SignInViewModel;

namespace Data_Organizer.MVVM.Views;

public partial class SignInPage : ContentPage
{
    public SignInPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        var signInViewModel = (SignInViewModel)BindingContext;

        signInViewModel.IsLoading = false;

        base.OnAppearing();
    }
}