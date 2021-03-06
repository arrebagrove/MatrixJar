﻿using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace MatrixJar
{
    public sealed partial class Page_Minus : Page
    {
        public Page_Minus()
        {
            this.InitializeComponent();
            MatrixA._pivotToNavigate = MainPivot;
            MatrixB._pivotToNavigate = MainPivot;
            Result._pivotToNavigate = MainPivot;
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
                    Result.ErrorInput.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Result.ErrorSize.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Result.InnerMatrix = MatrixA.InnerMatrix - MatrixB.InnerMatrix;
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
                    Result.SizeException.Text =rl.GetString("PlusMinusErr");
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
