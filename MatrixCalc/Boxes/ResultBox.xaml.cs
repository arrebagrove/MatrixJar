using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace MatrixJar
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

                Windows.Storage.ApplicationDataContainer localSettings =
                    Windows.Storage.ApplicationData.Current.LocalSettings;
                bool isLong = (int)localSettings.Values["Format"] == 0 ? false : true;

                // Fill the matrix with numbers
                for (int x = 0; x < columnCount; x++)
                {
                    for (int y = 0; y < rowCount; y++)
                    {
                        TextBlock textBlock = createTextBlock(x, y);
                        textBlock.Text = ((Math.Floor(value[x, y] * (isLong ? 10000 : 100)) / 
                            (isLong ? 10000 : 100))).ToString();
                        MatrixOne.Children.Add(textBlock);
                    }
                }
            }
        }

        private TextBlock createTextBlock(int x, int y)
        {
            TextBlock textBlock = new TextBlock() { Margin = new Thickness(0, 9, 0, 9), MinWidth = 0 };
            textBlock.SetValue(Grid.RowProperty, x);
            textBlock.FontSize = textBlock.FontSize * 1.1;
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
            ResourceLoader rl = new ResourceLoader();
            AppBarButton apb = (AppBarButton)sender;
            try
            {
                App._matrix = InnerMatrix;
                PopFlyout(rl.GetString("CopySuccess"), apb);
            }
            catch
            {
                PopFlyout(rl.GetString("CopyFail"), apb);
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
                ResultType.Text = Title;
            }
        }
        
        public CommandBar commandBar
        {
            get
            {
                return AppBar;
            }
        }

        public Pivot _pivotToNavigate = null;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _pivotToNavigate.SelectedIndex = 0;
        }
    }
}
