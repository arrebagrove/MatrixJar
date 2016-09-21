using System;
using Windows.UI.Popups;
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

        private async void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((Pivot)sender).SelectedIndex == 2)
            {
                try
                {
                    Result.InnerMatrix = MatrixA.InnerMatrix + MatrixB.InnerMatrix;
                }
                catch (MatrixSizeException ex)
                {
                    await (new MessageDialog("Размеры матриц не совпадают!", "Ошибка!")).ShowAsync();
                }
                catch (MatrixInputInvalidException ex)
                {

                }
                catch (Exception ex)
                {

                }
            }
        }

    }
}
