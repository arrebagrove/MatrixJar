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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace MatrixCalc
{
    public sealed partial class Page_Plus : Page
    {
        public Page_Plus()
        {
            this.InitializeComponent();
            App.ChosenIndex = 1;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double[,] matrix = MatrixA.Matrix;
                await (new MessageDialog(matrix.ToString())).ShowAsync();
                matrix[0, 0] = 9999D;
                MatrixB.Matrix = matrix;
            }
            catch
            {

            }
        }
    }
}
