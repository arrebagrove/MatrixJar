using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
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

namespace MatrixJar
{
    public sealed partial class Page_Determinant : Page
    {
        public Page_Determinant()
        {
            this.InitializeComponent();
            Result.commandBar.Visibility = Visibility.Collapsed;
            App.ChosenIndex = 8;
        }


        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (App._matrix != null)
            {
                MatrixA.PasteButton.Visibility = 
                    MatrixA.PasteButtonSeparator.Visibility = Visibility.Visible;
            }

            if (((Pivot)sender).SelectedIndex == 1)
            {
                try
                {
                    Result.ErrorInput.Visibility = 
                        Windows.UI.Xaml.Visibility.Collapsed;
                    Result.ErrorSize.Visibility = 
                        Windows.UI.Xaml.Visibility.Collapsed;
                    Matrix matrix = new Matrix(1, 1);
                    matrix[0, 0] = MatrixA.InnerMatrix.GetDeterminant();
                    Result.InnerMatrix = matrix;
                }
                catch (MatrixInputInvalidException ex)
                {
                    Result.commandBar.Visibility = Visibility.Collapsed;
                    Result.ErrorInput.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                catch (MatrixSizeException ex)
                {
                    Result.commandBar.Visibility = Visibility.Collapsed;
                    ResourceLoader rl = new ResourceLoader();
                    Result.SizeException.Text = rl.GetString("DetNotExists");
                    Result.ErrorSize.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                catch (Exception ex)
                {
                    Result.commandBar.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
