using Data_Organizer.MVVM.ViewModels.MainPageViewModel;
namespace Data_Organizer.MVVM.Views;

public partial class MainPage : ContentPage
{
    private static bool _isHelpShown = false;

    public MainPage()
    {
        try
        {
            InitializeComponent();
            
            this.Loaded += (s, e) => {
                try
                {
                    foreach (var child in this.GetVisualTreeDescendants())
                    {
                        if (child is ContentView contentView)
                        {
                            contentView.BindingContext = this.BindingContext;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error setting binding context: {ex}");
                }
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in MainPage constructor: {ex}");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (VersionTracking.IsFirstLaunchEver && !_isHelpShown)
        {
            ((MainPageViewModel)BindingContext).IsHelpOpen = true;
            _isHelpShown = true;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ((MainPageViewModel)BindingContext).Dispose();
    }
}