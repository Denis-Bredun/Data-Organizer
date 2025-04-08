using Data_Organizer.MVVM.ViewModels.SignUpViewModel;

namespace Data_Organizer.MVVM.Views;

public partial class SignUpPage : ContentPage
{
    private SignUpViewModel _signUpViewModel;

    public SignUpPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        if (_signUpViewModel == null)
            _signUpViewModel = (SignUpViewModel)BindingContext;

        _signUpViewModel.IsLoading = false;

        base.OnAppearing();
    }
}