using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace MatrixJar
{
    public sealed partial class MainPage : Page
    {
        private bool _manipulationsEnabled = true;

        public MainPage()
        {
            Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values["Format"] == null) localSettings.Values["Format"] = 0;

            this.InitializeComponent();
            MainFrame.Navigate(typeof(Page_Plus));
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(300, 300));
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                statusBar.BackgroundColor = (Color)Application.Current.Resources["SystemAccentColor"];
                statusBar.ForegroundColor = Colors.White;
                statusBar.BackgroundOpacity = 0.85;
            }
            //PC customization
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {
                    titleBar.BackgroundColor = titleBar.ButtonBackgroundColor = 
                        (Color)Application.Current.Resources["SystemAccentColor"];
                    titleBar.ForegroundColor = Colors.White;
                }
            }

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                if (SplitviewLayer.Width > 0)
                {
                    ClosePane();
                    a.Handled = true;
                }
                else if (MainFrame.CanGoBack)
                {
                    MainFrame.GoBack();
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = 
                        MainFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;    
                    a.Handled = true;
                }
            };

            await Task.Delay(250);
            OpenPane();
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            if (SplitviewLayer.Width > 0) ClosePane(); else OpenPane();
        }

        private async void HamburgerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (HamburgerListBox.SelectedIndex)
            {
                case 1:
                    MainFrame.Navigate(typeof(Page_Plus));
                    break;
                case 2:
                    MainFrame.Navigate(typeof(Page_Minus));
                    break;
                case 3:
                    MainFrame.Navigate(typeof(Page_Multi));
                    break;
                case 4:
                    MainFrame.Navigate(typeof(Page_MultiNum));
                    break;
                case 5:
                    MainFrame.Navigate(typeof(Page_Expo));
                    break;
                case 6:
                    MainFrame.Navigate(typeof(Page_Transp));
                    break;
                case 7:
                    MainFrame.Navigate(typeof(Page_Reverse));
                    break;
                case 8:
                    MainFrame.Navigate(typeof(Page_Determinant));
                    break;
            }

            await Task.Delay(150);
            ClosePane();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            HamburgerListBox.SelectedIndex = App.ChosenIndex;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                MainFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private void SplitviewLayer_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            double dx = e.Delta.Translation.X;
            if (SplitviewLayer.Width + dx < 270 && SplitviewLayer.Width + dx > 0)
            {
                SplitviewLayer.Width += dx;
            }
        }

        private async void SplitviewLayer_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            HamburgerListBox.IsEnabled = false;
            double v = e.Velocities.Linear.X;
            if (v < 0)
            {
                ClosePane();
                await Task.Delay(200);
            }
            else if (v >= 0)
            {
                OpenPane();
                await Task.Delay(200);
            }
            HamburgerListBox.IsEnabled = true;
        }

        private void LayoutController_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            if (!_manipulationsEnabled) return;
            SplitviewLayer.Width = LayoutController.Width;
        }

        private async void LayoutController_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (!_manipulationsEnabled) return;
            double v = e.Velocities.Linear.X;
            if (v < 0)
            {
                ClosePane();
                await Task.Delay(200);
            }
            else if (v >= 0)
            {
                OpenPane();
                await Task.Delay(200);
            }
        }

        private void LayoutController_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (!_manipulationsEnabled) return;
            double dx = e.Delta.Translation.X;
            if (SplitviewLayer.Width + dx <= 270 &&
                SplitviewLayer.Width + dx >= 0 &&
                LayoutController.Width + dx < 270)
            {
                LayoutController.Width += dx;
                SplitviewLayer.Width += dx;
            }
        }

        private void LayoutController_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ClosePane();
        }

        private void OpenPane()
        {
            _manipulationsEnabled = false;
            LayoutController.Width = double.NaN;
            LayoutController.HorizontalAlignment = HorizontalAlignment.Stretch;
            DoubleAnimation line = new DoubleAnimation()
            {
                From = SplitviewLayer.Width,
                To = 270,
                Duration = TimeSpan.FromMilliseconds(200),
                EnableDependentAnimation = true
            };
            Storyboard.SetTarget(line, SplitviewLayer);
            Storyboard.SetTargetProperty(line, "Width");
            Storyboard openpane = new Storyboard();
            openpane.Children.Add(line);
            openpane.Begin();
        }

        private void ClosePane()
        {
            _manipulationsEnabled = true;
            LayoutController.HorizontalAlignment = HorizontalAlignment.Left;
            LayoutController.Width = 12;
            DoubleAnimation line = new DoubleAnimation()
            {
                From = SplitviewLayer.Width,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(200),
                EnableDependentAnimation = true
            };
            Storyboard.SetTarget(line, SplitviewLayer);
            Storyboard.SetTargetProperty(line, "Width");
            Storyboard openpane = new Storyboard();
            openpane.Children.Add(line);
            openpane.Begin();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await (new SettingsDiag()).ShowAsync();
        }
    }
}
