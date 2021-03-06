﻿using System;
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
    public sealed partial class Page_Reverse : Page
    {
        public Page_Reverse()
        {
            this.InitializeComponent();
            MatrixA._pivotToNavigate = MainPivot;
            Result._pivotToNavigate = MainPivot;
            App.ChosenIndex = 7;
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (App._matrix != null)
            {
                MatrixA.PasteButton.Visibility =
                    MatrixA.PasteButtonSeparator.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

            if (((Pivot)sender).SelectedIndex == 1)
            {
                try
                {
                    Result.commandBar.Visibility = Visibility.Visible;
                    Result.ErrorInput.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Result.ErrorSize.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Result.InnerMatrix = MatrixA.InnerMatrix.Inverse();
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
