using Windows.UI.Xaml.Controls;

namespace MatrixJar
{
    public sealed partial class SettingsDiag : ContentDialog
    {
        public SettingsDiag()
        {
            this.InitializeComponent();
            Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
            ThemeCombo.SelectedIndex = (int)localSettings.Values["Theme"];
            FormatCombo.SelectedIndex = (int)localSettings.Values["Format"];
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["Theme"] = ThemeCombo.SelectedIndex;
            localSettings.Values["Format"] = FormatCombo.SelectedIndex;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            return;
        }
    }
}
