using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace Data_Organizer.MVVM.Views.Controls
{
    public partial class SettingsPopup : ContentView
    {
        public SettingsPopup()
        {
            InitializeComponent();

            InitializeScrollView();
        }

        public void InitializeScrollView()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                SettingsScrollView.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
                    .SetIsLegacyColorModeEnabled(true);
            }
        }
    }
}