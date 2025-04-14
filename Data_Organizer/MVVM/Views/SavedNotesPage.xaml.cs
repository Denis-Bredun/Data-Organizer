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

        base.OnAppearing();
    }
}