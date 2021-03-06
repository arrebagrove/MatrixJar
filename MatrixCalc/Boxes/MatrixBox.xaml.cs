﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace MatrixJar
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
                    TextBox textBox = createTextBox(count, x);
                    MatrixOne.Children.Add(textBox);
                }
                UpdateCounter();
            }
        }

        private bool _canRemRow = true;
        private void RemRow_Click(object sender, RoutedEventArgs e)
        {
            if (!_canRemRow) return;
            _canRemRow = false;
            int index = MatrixOne.RowDefinitions.Count - 1;
            if (index > 1)
            {
                for (int x = 0; x < MatrixOne.ColumnDefinitions.Count; x++)
                    foreach (UIElement child in MatrixOne.Children)
                        if (((int)child.GetValue(Grid.RowProperty) == index)
                            && ((int)child.GetValue(Grid.ColumnProperty) == x))
                            MatrixOne.Children.Remove(child);
                MatrixOne.RowDefinitions.RemoveAt(index);
                UpdateCounter();
            }
            _canRemRow = true;
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
                    TextBox textBox = createTextBox(x, count);
                    MatrixOne.Children.Add(textBox);
                }
                UpdateCounter();
            }
        }

        private bool _canRemColumn = true;
        private async void RemColumn_Click(object sender, RoutedEventArgs e)
        {
            if (!_canRemColumn) return;
            _canRemColumn = false;
            int index = MatrixOne.ColumnDefinitions.Count - 1;
            if (index > 1)
            {
                for (int x = 0; x < MatrixOne.RowDefinitions.Count; x++)
                    foreach (UIElement child in MatrixOne.Children)
                        if (((int)child.GetValue(Grid.ColumnProperty) == index)
                            && ((int)child.GetValue(Grid.RowProperty) == x))
                            MatrixOne.Children.Remove(child);
                await Task.Delay(700);
                MatrixOne.ColumnDefinitions.RemoveAt(index);
                UpdateCounter();
            }
            _canRemColumn = true;
        }

        private TextBox createTextBox(int a, int b)
        {
            TextBox textBox = new TextBox() { Margin = new Thickness(3), MinWidth = 0 };
            textBox.SetValue(Grid.RowProperty, a);
            textBox.SetValue(Grid.ColumnProperty, b);
            textBox.Transitions = new TransitionCollection();
            textBox.Transitions.Add(new AddDeleteThemeTransition());
            textBox.InputScope = new InputScope()
            {
                Names = { new InputScopeName(InputScopeNameValue.Digits) }
            };
            textBox.BorderBrush = Resources["AppBarItemDisabledForegroundThemeBrush"] as Brush;
            textBox.TextChanged += TextBox_TextChanged;
            textBox.KeyDown += TextBox_KeyDown;
            return textBox;
        }

        private bool _canJump = true;
        private async void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (!_canJump) return;
                _canJump = false;

                int width = MatrixOne.ColumnDefinitions.Count;
                int height = MatrixOne.RowDefinitions.Count;

                int x = (int)((TextBox)sender).GetValue(Grid.ColumnProperty);
                int y = (int)((TextBox)sender).GetValue(Grid.RowProperty);

                if (x < width)
                {
                    for (int i = 0; i < MatrixOne.Children.Count; i++)
                    {
                        TextBox textBox = (TextBox)MatrixOne.Children[i];
                        if ((int)textBox.GetValue(Grid.RowProperty) == y &&
                            (int)textBox.GetValue(Grid.ColumnProperty) == x + 1)
                        {
                            textBox.Focus(FocusState.Programmatic);
                            await Task.Delay(100);
                            _canJump = true;
                            return;
                        }
                    }
                }

                if (y < width)
                {
                    for (int i = 0; i < MatrixOne.Children.Count; i++)
                    {
                        TextBox textBox = (TextBox)MatrixOne.Children[i];
                        if ((int)textBox.GetValue(Grid.RowProperty) == y + 1 &&
                            (int)textBox.GetValue(Grid.ColumnProperty) == 0)
                        {
                            textBox.Focus(FocusState.Programmatic);
                            await Task.Delay(100);
                            _canJump = true;
                            return;
                        }
                    }
                }

                await Task.Delay(100);
                _canJump = true;
            }
        }
        
        private void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.Equals("-")) return; //minus is okay, don't cancel
            try
            {
                double.Parse(textBox.Text.Replace('.', ','));
                textBox.BorderBrush = Resources["SystemControlHighlightAccentBrush"] as Brush;
                try
                {
                    Matrix temp = this.InnerMatrix;
                    GoForward.IsEnabled = true;
                }
                catch
                {
                    GoForward.IsEnabled = false;
                }
            }
            catch
            {
                GoForward.IsEnabled = false;
                textBox.BorderBrush = Resources["AppBarItemDisabledForegroundThemeBrush"] as Brush;
                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1);
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }

        private void UpdateCounter()
        {
            int x_num = MatrixOne.ColumnDefinitions.Count;
            int y_num = MatrixOne.RowDefinitions.Count;
            xC.Text = y_num.ToString();
            yC.Text = x_num.ToString();
        }

        public Matrix InnerMatrix
        {
            get
            {
                int columnCount = MatrixOne.ColumnDefinitions.Count;
                int rowCount = MatrixOne.RowDefinitions.Count;
                Matrix matrix = new Matrix(rowCount, columnCount);
                try
                {
                    for (int x = 0; x < rowCount; x++)
                        for (int y = 0; y < columnCount; y++)
                            foreach (UIElement child in MatrixOne.Children)
                                if (((int)child.GetValue(Grid.ColumnProperty) == y)
                                    && ((int)child.GetValue(Grid.RowProperty) == x))
                                    matrix[x, y] = double.Parse(((TextBox)child).Text.Replace('.', ','));
                    return matrix;
                }
                catch
                {
                    throw new MatrixInputInvalidException(string.Format("Неверный ввод в матрицу {0}!", Title));
                }
            }
            set
            {
                // Get matrix dimensions
                int columnCount = value.GetWidth();
                int rowCount = value.GetHeight();

                // Clear initially generated xaml
                MatrixOne.Children.Clear();
                MatrixOne.RowDefinitions.Clear();
                MatrixOne.ColumnDefinitions.Clear();

                // Set column and row count of a new matrix
                for (int x = 0; x < rowCount; x++)
                    MatrixOne.ColumnDefinitions.Add(new ColumnDefinition());
                for (int x = 0; x < columnCount; x++)
                    MatrixOne.RowDefinitions.Add(new RowDefinition());

                // Fill the matrix with numbers
                for (int x = 0; x < columnCount; x++)
                {
                    for (int y = 0; y < rowCount; y++)
                    {
                        TextBox textBox = createTextBox(x, y);
                        textBox.Text = value[x, y].ToString();

                        // Data in the matrix is 100% correct, so let's mark it as correct
                        textBox.BorderBrush = Resources["SystemControlHighlightAccentBrush"] as Brush;
                        MatrixOne.Children.Add(textBox);
                    }
                }               
        
                UpdateCounter();
            }
        }

        public static readonly DependencyProperty TitleProperty = 
            DependencyProperty.Register("Title", typeof(string), typeof(string), 
                new PropertyMetadata(null));

        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                ResourceLoader rl = new ResourceLoader();
                SetValue(TitleProperty, value);
                FillMatrix.Text = string.Format(rl.GetString("FillMatrix"), value);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton apb = (AppBarButton)sender;
            foreach (UIElement child in MatrixOne.Children)
                ((TextBox)child).Text = string.Empty;
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            ResourceLoader rl = new ResourceLoader();
            AppBarButton apb = (AppBarButton)sender;
            try
            {
                App._matrix = InnerMatrix;
                PopFlyout(rl.GetString("CopySuccess"), apb);
                PasteEnabling.Begin();
            }
            catch
            {
                PopFlyout(rl.GetString("CopyFail"), apb);
            }
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            ResourceLoader rl = new ResourceLoader();
            AppBarButton apb = (AppBarButton)sender;
            if (App._matrix != null)
            {
                GoForward.IsEnabled = true;
                InnerMatrix = App._matrix;
            }
            else
            {
                PopFlyout(rl.GetString("CopyFirst"), apb);
            }
        }

        private async void PopFlyout(string text, FrameworkElement sender)
        {
            Flyout fly = new Flyout() { Content = new TextBlock() { Text = text } };
            fly.ShowAt(sender);
            await Task.Delay(1000);
            fly.Hide();
        }

        public AppBarButton PasteButton
        {
            get
            {
                return Paste;
            }
        }

        public AppBarSeparator PasteButtonSeparator
        {
            get
            {
                return PasteSeparator;
            }
        }

        public Pivot _pivotToNavigate = null;
        private void GoForward_Click(object sender, RoutedEventArgs e)
        {
            _pivotToNavigate.SelectedIndex += 1;
        }
    }
}
