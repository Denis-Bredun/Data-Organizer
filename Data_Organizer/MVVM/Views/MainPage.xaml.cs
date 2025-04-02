using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.ViewModels;
using Data_Organizer.MVVM.ViewModels.MainPageViewModel;

namespace Data_Organizer.MVVM.Views;
public partial class MainPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    private HelpPageViewModel _helpPageViewModel;
    private MainPageViewModel _mainPageViewModel;

    public MainPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_mainPageViewModel == null)
            _mainPageViewModel = (MainPageViewModel)BindingContext;
        _mainPageViewModel.IsLoading = false;
        _mainPageViewModel.HasVisitedMainPage = true;

        await CheckAndShowHelpIfNeeded();

        ModifySubscriptionMarkIfNeeded();
    }

    private void ModifySubscriptionMarkIfNeeded()
    {
        var featureService = _serviceProvider.GetRequiredService<IFeatureService>();
        var firebaseAuthService = _serviceProvider.GetRequiredService<IFirebaseAuthService>();

        if (firebaseAuthService.IsUserAuthorized())
            featureService.RemoveSubscriptionMarkFromFeatures();
        else
            featureService.AddSubscriptionMarkToFeatures();
    }

    private async Task CheckAndShowHelpIfNeeded()
    {
        if (_helpPageViewModel == null)
            _helpPageViewModel = _serviceProvider.GetRequiredService<HelpPageViewModel>();

        if (!_helpPageViewModel.HasBeenClosedOnce)
            await _mainPageViewModel.ShowHelpInformation();
    }

    private async void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.OldTextValue != e.NewTextValue)
            await MyScrollView.ScrollToAsync(MyEditor, ScrollToPosition.End, true);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ((MainPageViewModel)BindingContext).Dispose();
    }
}