using System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace myMatrix
{
    public sealed partial class Page_Minus : Page
    {
        public Page_Minus()
        {
            this.InitializeComponent();
            App.ChosenIndex = 2;
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (App._matrix != null)
            {
                MatrixA.PasteButton.Visibility =
                    MatrixA.PasteButtonSeparator.Visibility = Windows.UI.Xaml.Visibility.Visible;
                MatrixB.PasteButton.Visibility =
                    MatrixB.PasteButtonSeparator.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

            if (((Pivot)sender).SelectedIndex == 2)
            {
                try
                {
                    Result.InnerMatrix = MatrixA.InnerMatrix - MatrixB.InnerMatrix;
                }
                catch (MatrixSizeException ex)
                {
                    Result.SizeException.Text = "Размеры матриц не совпадают! Операции сложения и вычитания можно совершать только над матрицами одинаковой размерности.";
                    Result.ErrorSize.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                catch (MatrixInputInvalidException ex)
                {
                    Result.ErrorInput.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
