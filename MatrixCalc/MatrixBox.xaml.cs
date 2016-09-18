using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class MatrixBox : UserControl
    {
        public MatrixBox()
        {
            this.InitializeComponent();
        }

        private void AddRow_Click(object sender, RoutedEventArgs e)
        {
            int count = MatrixOne.RowDefinitions.Count;
            if (count < 6)
            {
                RowDefinition rowDefinition = new RowDefinition();
                MatrixOne.RowDefinitions.Add(rowDefinition);
                for (int x = 0; x < MatrixOne.ColumnDefinitions.Count; x++)
                {
                    TextBox textBox = new TextBox() { Margin = new Thickness(3), MinWidth = 0 };
                    textBox.SetValue(Grid.RowProperty, count);
                    textBox.SetValue(Grid.ColumnProperty, x);
                    textBox.Transitions = new TransitionCollection();
                    textBox.Transitions.Add(new AddDeleteThemeTransition());
                    textBox.InputScope = new InputScope()
                    {
                        Names = { new InputScopeName(InputScopeNameValue.Digits) }
                    };
                    textBox.BorderBrush = Resources["AppBarItemDisabledForegroundThemeBrush"] as Brush;
                    MatrixOne.Children.Add(textBox);
                }
            }

            UpdateCounter();
        }

        private async void RemRow_Click(object sender, RoutedEventArgs e)
        {
            int index = MatrixOne.RowDefinitions.Count - 1;
            if (index > 1)
            {
                for (int x = 0; x < MatrixOne.ColumnDefinitions.Count; x++)
                    foreach (UIElement child in MatrixOne.Children)
                        if (((int)child.GetValue(Grid.RowProperty) == index)
                            && ((int)child.GetValue(Grid.ColumnProperty) == x))
                            MatrixOne.Children.Remove(child);
                await Task.Delay(600);
                MatrixOne.RowDefinitions.RemoveAt(index);
            }

            UpdateCounter();
        }

        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
            int count = MatrixOne.ColumnDefinitions.Count;
            if (count < 6)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                MatrixOne.ColumnDefinitions.Add(columnDefinition);
                for (int x = 0; x < MatrixOne.RowDefinitions.Count; x++)
                {
                    TextBox textBox = new TextBox() { Margin = new Thickness(3), MinWidth = 0 };
                    textBox.SetValue(Grid.RowProperty, x);
                    textBox.SetValue(Grid.ColumnProperty, count);
                    textBox.Transitions = new TransitionCollection();
                    textBox.Transitions.Add(new AddDeleteThemeTransition());
                    textBox.InputScope = new InputScope()
                    {
                        Names = { new InputScopeName(InputScopeNameValue.Digits) }
                    };
                    textBox.BorderBrush = Resources["AppBarItemDisabledForegroundThemeBrush"] as Brush;
                    MatrixOne.Children.Add(textBox);
                }
            }

            UpdateCounter();
        }

        private async void RemColumn_Click(object sender, RoutedEventArgs e)
        {
            int index = MatrixOne.ColumnDefinitions.Count - 1;
            if (index > 1)
            {
                for (int x = 0; x < MatrixOne.RowDefinitions.Count; x++)
                    foreach (UIElement child in MatrixOne.Children)
                        if (((int)child.GetValue(Grid.ColumnProperty) == index)
                            && ((int)child.GetValue(Grid.RowProperty) == x))
                            MatrixOne.Children.Remove(child);
                await Task.Delay(500);
                MatrixOne.ColumnDefinitions.RemoveAt(index);
            }

            UpdateCounter();
        }

        private void UpdateCounter()
        {
            int x_num = MatrixOne.ColumnDefinitions.Count;
            int y_num = MatrixOne.RowDefinitions.Count;
            xC.Text = y_num.ToString();
            yC.Text = x_num.ToString();
        }
    }
}
