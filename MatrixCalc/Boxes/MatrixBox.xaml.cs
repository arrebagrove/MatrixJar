using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

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
            textBox.TextChanged += (sender, args) => TextBox_TextChanged(sender, args);
            return textBox;
        }

        private void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            try
            {
                double.Parse(textBox.Text.Replace('.', ','));
                textBox.BorderBrush = Resources["SystemControlHighlightAccentBrush"] as Brush;
            }
            catch
            {
                textBox.BorderBrush = Resources["AppBarItemDisabledForegroundThemeBrush"] as Brush;
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
                    ShowError(string.Format(@"Ошибка ввода данных в матрицу {0}! Ячейки, в которые был осуществлен некорректный ввод, подсвечиваются серым цветом.", Title));
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
                for (int x = 0; x < rowCount; x++) MatrixOne.ColumnDefinitions.Add(new ColumnDefinition());
                for (int x = 0; x < columnCount; x++) MatrixOne.RowDefinitions.Add(new RowDefinition());

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

        private async void ShowError(string error)
        {
            await (new MessageDialog(error)).ShowAsync();
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
                SetValue(TitleProperty, value);
                FillMatrix.Text = string.Format("Заполните матрицу {0} и\nпроведите по экрану!", value);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in MatrixOne.Children)
                ((TextBox)child).Text = string.Empty;
        }
    }
}
