using Windows.System;
using System;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Email;

namespace MatrixJar
{
    public sealed partial class SettingsDiag : ContentDialog
    {
        public SettingsDiag()
        {
            this.InitializeComponent();
            Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
            FormatCombo.SelectedIndex = (int)localSettings.Values["Format"];
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["Format"] = FormatCombo.SelectedIndex;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            return;
        }

        private async void WebSiteBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri("http://worldbeater.github.io"));
            }
            catch
            {

            }
        }

        private async void MailMeBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            EmailMessage emailMessage = new EmailMessage();
            emailMessage.To.Add(new EmailRecipient("worldbeater-dev@yandex.ru"));
            emailMessage.Body = "MatrixJar Feedback";
            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }

        private async void FeedbackBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?ProductId=9nblggh530bd"));
            }
            catch
            {

            }
        }
    }
}
