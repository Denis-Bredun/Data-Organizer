using Data_Organizer.MVVM.ViewModels;

namespace Data_Organizer.MVVM.Views;

public partial class SignUpPage : ContentPage
{
    public SignUpPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        var signUpViewModel = (SignUpViewModel)BindingContext;

        signUpViewModel.IsLoading = false;

        base.OnAppearing();
    }
}