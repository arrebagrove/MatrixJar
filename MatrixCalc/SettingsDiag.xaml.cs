using Windows.UI.Xaml.Controls;

namespace MatrixCalc
{
    public sealed partial class SettingsDiag : ContentDialog
    {
        public SettingsDiag()
        {
            Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;

            switch ((string)localSettings.Values["Theme"])
            {
                case "0":
                    ThemeCombo.SelectedIndex = 0;
                    break;
                case "1":
                    ThemeCombo.SelectedIndex = 1;
                    break;
                case "2":
                    ThemeCombo.SelectedIndex = 1;
                    break;
            }

            switch ((string)localSettings.Values["Format"])
            {
                case "0":
                    FormatCombo.SelectedIndex = 0;
                    break;
                case "1":
                    FormatCombo.SelectedIndex = 1;
                    break;
            }

            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["Theme"] = ThemeCombo.SelectedIndex.ToString();
            localSettings.Values["Format"] = ThemeCombo.SelectedIndex.ToString();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            return;
        }
    }
}
