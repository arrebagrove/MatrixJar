using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace myMatrix
{
    public sealed partial class ResultBox : UserControl
    {
        public ResultBox()
        {
            this.InitializeComponent();
        }

        public Matrix InnerMatrix
        {
            get
            {
                int columnCount = MatrixOne.ColumnDefinitions.Count;
                int rowCount = MatrixOne.RowDefinitions.Count;
                Matrix matrix = new Matrix(rowCount, columnCount);
                for (int x = 0; x < rowCount; x++)
                    for (int y = 0; y < columnCount; y++)
                        foreach (UIElement child in MatrixOne.Children)
                            if (((int)child.GetValue(Grid.ColumnProperty) == y)
                                && ((int)child.GetValue(Grid.RowProperty) == x))
                                matrix[x, y] = double.Parse(((TextBlock)child).Text);
                return matrix;
            }
            set
            {
                int columnCount = value.GetWidth();
                int rowCount = value.GetHeight();

                MatrixOne.Children.Clear();
                MatrixOne.RowDefinitions.Clear();
                MatrixOne.ColumnDefinitions.Clear();

                for (int x = 0; x < rowCount; x++) MatrixOne.ColumnDefinitions.Add(new ColumnDefinition());
                for (int x = 0; x < columnCount; x++) MatrixOne.RowDefinitions.Add(new RowDefinition());

                for (int x = 0; x < columnCount; x++)
                {
                    for (int y = 0; y < rowCount; y++)
                    {
                        TextBlock textBlock = createTextBlock(x, y);
                        textBlock.Text = ((Math.Floor(value[x, y] * 100) / 100)).ToString();
                        MatrixOne.Children.Add(textBlock);
                    }
                }
            }
        }

        private TextBlock createTextBlock(int x, int y)
        {
            TextBlock textBlock = new TextBlock() { Margin = new Thickness(0, 9, 0, 9), MinWidth = 0 };
            textBlock.SetValue(Grid.RowProperty, x);
            textBlock.SetValue(Grid.ColumnProperty, y);
            textBlock.Transitions = new TransitionCollection();
            textBlock.Transitions.Add(new AddDeleteThemeTransition());
            textBlock.TextAlignment = TextAlignment.Center;
            return textBlock;
        }

        public Grid ErrorInput
        {
            get
            {
                return this.ErrorInputP;
            }
        }

        public Grid ErrorSize
        {
            get
            {
                return this.ErrorSizeP;
            }
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton apb = (AppBarButton)sender;
            try
            {
                App._matrix = InnerMatrix;
                PopFlyout("Матрица скопирована!", apb);
            }
            catch
            {
                PopFlyout("Не удалось скопировать матрицу.", apb);
            }
        }

        private async void PopFlyout(string text, FrameworkElement sender)
        {
            Flyout fly = new Flyout() { Content = new TextBlock() { Text = text } };
            fly.ShowAt(sender);
            await Task.Delay(1000);
            fly.Hide();
        }

        public TextBlock SizeException
        {
            get
            {
                return this.SizeExceptionText;
            }
        }
    }
}
