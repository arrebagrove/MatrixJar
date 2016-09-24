using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

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
                    Result.commandBar.Visibility = Visibility.Visible;
                    Result.InnerMatrix = MatrixA.InnerMatrix - MatrixB.InnerMatrix;
                }
                catch (MatrixSizeException ex)
                {
                    Result.commandBar.Visibility = Visibility.Collapsed;
                    ResourceLoader rl = new ResourceLoader();
                    Result.SizeException.Text =rl.GetString("PlusMinusErr");
                    Result.ErrorSize.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                catch (MatrixInputInvalidException ex)
                {
                    Result.commandBar.Visibility = Visibility.Collapsed;
                    Result.ErrorInput.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                catch (Exception ex)
                {
                    Result.commandBar.Visibility = Visibility.Collapsed;

                }
            }
        }
    }
}
