using Data_Organizer.MVVM.ViewModels;

namespace Data_Organizer.MVVM.Views;

public partial class SavedNotesPage : ContentPage
{
    private SavedNotesPageViewModel _savedNotesPageViewModel;

    public SavedNotesPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        if (_savedNotesPageViewModel == null)
            _savedNotesPageViewModel = (SavedNotesPageViewModel)BindingContext;

        _savedNotesPageViewModel.IsLoading = false;
        _savedNotesPageViewModel.UpdateStatusMessage();

        base.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _savedNotesPageViewModel.UpdateStatusMessage();
    }
}