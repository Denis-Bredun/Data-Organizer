﻿using CommunityToolkit.Mvvm.ComponentModel;
using Data_Organizer.Interfaces;

namespace Data_Organizer.MVVM.ViewModels.SignInViewModel
{
    public partial class SignInViewModel : ObservableObject
    {
        private readonly INotificationService _notificationService;
        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFirestoreDbService _firestoreDbService;

        [ObservableProperty] private string _email;
        [ObservableProperty] private string _password;
        [ObservableProperty] private bool _isLoading;

        public SignInViewModel(
            INotificationService notificationService,
            IFirebaseAuthService firebaseAuthService,
            IServiceProvider serviceProvider,
            IFirestoreDbService firestoreDbService)
        {
            _notificationService = notificationService;
            _firebaseAuthService = firebaseAuthService;
            _serviceProvider = serviceProvider;
            _firestoreDbService = firestoreDbService;
        }

        private async Task SetMetadataFlagIfAuthorized()
        {
            if (_firebaseAuthService.IsUserAuthorized())
            {
                var settingsPageViewModel = _serviceProvider.GetRequiredService<SettingsPageViewModel.SettingsPageViewModel>();
                var isMetadataStored = await _firestoreDbService.GetMetadataStoredAsync();
                settingsPageViewModel.ChangeMetadataFlagWithoutAsking(isMetadataStored);
            }
        }

        private void CleanFields()
        {
            Email = "";
            Password = "";
        }
    }
}