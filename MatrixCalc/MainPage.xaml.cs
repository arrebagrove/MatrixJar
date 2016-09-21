using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MatrixCalc
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MainFrame.Navigate(typeof(Page_Plus));
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(300, 300));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                SolidColorBrush statuscolor = NavBackground.Background as SolidColorBrush;
                statusBar.BackgroundColor = statuscolor.Color;
                statusBar.BackgroundOpacity = 1;
            }
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                if (MainFrame.CanGoBack)
                {
                    MainFrame.GoBack();
                    a.Handled = true;
                }
            };
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void HamburgerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                    MainFrame.Navigate(typeof(Page_Transp));
                    break;
                case 5:
                    MainFrame.Navigate(typeof(Page_Reverse));
                    break;
                case 6:
                    MainFrame.Navigate(typeof(Page_Determinant));
                    break;
            }

            MySplitView.IsPaneOpen = false;
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            HamburgerListBox.SelectedIndex = App.ChosenIndex;
        }

        private void SplitView_Open(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = true;
        }

        private void SplitView_Close(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = false;
        }
    }
}
