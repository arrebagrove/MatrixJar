using System;
using Windows.UI.Xaml.Controls;

namespace MatrixCalc
{
    public sealed partial class Page_Plus : Page
    {
        public Page_Plus()
        {
            this.InitializeComponent();
            App.ChosenIndex = 1;
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((Pivot)sender).SelectedIndex == 2) {
                try
                {
                    Result.ErrorInput.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Result.ErrorSize.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Result.InnerMatrix = MatrixA.InnerMatrix + MatrixB.InnerMatrix;
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

                }
            }
        }

    }
}
