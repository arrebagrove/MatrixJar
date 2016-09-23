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
    public sealed partial class Page_Determinant : Page
    {
        public Page_Determinant()
        {
            this.InitializeComponent();
            App.ChosenIndex = 6;
        }


        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
                    Result.ErrorInput.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                catch (MatrixSizeException ex)
                {
                    Result.SizeException.Text = "Размеры матриц не совпадают!Операции сложения и вычитания можно совершать только над матрицами одинаковой размерности.";
                    Result.ErrorSize.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                catch (Exception ex)
                {
                    (new MessageDialog(ex.ToString())).ShowAsync();
                }
            }
        }
    }
}
