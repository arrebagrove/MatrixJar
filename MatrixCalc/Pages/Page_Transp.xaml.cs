using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace myMatrix
{
    public sealed partial class Page_Transp : Page
    {
        public Page_Transp()
        {
            this.InitializeComponent();
            App.ChosenIndex = 4;
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((Pivot)sender).SelectedIndex == 1)
            {
                try
                {
                    Result.ErrorInput.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Result.ErrorSize.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Result.InnerMatrix = MatrixA.InnerMatrix.Transpose();
                }
                catch (MatrixInputInvalidException ex)
                {
                    Result.ErrorInput.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                catch (MatrixSizeException ex)
                {
                    Result.ErrorSize.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
            }
        }
    }
}
