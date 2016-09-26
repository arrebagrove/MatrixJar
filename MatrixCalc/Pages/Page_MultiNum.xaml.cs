﻿using myMatrix;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MatrixCalc.Pages
{
    public sealed partial class Page_MultiNum : Page
    {
        public Page_MultiNum()
        {
            this.InitializeComponent();
            App.ChosenIndex = 4;
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (App._matrix != null)
            {
                MatrixA.PasteButton.Visibility =
                    MatrixA.PasteButtonSeparator.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

            if (((Pivot)sender).SelectedIndex == 2)
            {
                try
                {
                    Result.commandBar.Visibility = Visibility.Visible;
                    Result.ErrorInput.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Result.ErrorSize.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    int number = int.Parse(NumberMulti.Text);
                    Result.InnerMatrix = MatrixA.InnerMatrix.MultiplyByNumber(number);
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