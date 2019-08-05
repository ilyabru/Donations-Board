using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DonationBoard.Controls
{
    /// <summary>
    /// This textbox override allows only decimals numbers to be entered
    /// Does not allow a value less than zero to be entered to be entered
    /// </summary>
    public class DecimalTextBox : TextBox
    {
        public DecimalTextBox()
        {
            RegisterPropertyChangedCallback(TextProperty, OnTextChanged);
            BeforeTextChanging += OnBeforeTextChanging;
        }

        #region Format
        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(nameof(Format), typeof(string), typeof(DecimalTextBox), new PropertyMetadata(null, OnPropertyChanged));
        #endregion

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DecimalTextBox;
            control.ApplyTextFormat();
        }

        private void OnTextChanged(DependencyObject sender, DependencyProperty dp)
        {
            ApplyTextFormat();
        }

        private void ApplyTextFormat()
        {
            if (FocusState == FocusState.Unfocused && !string.IsNullOrEmpty(Text))
            {
                decimal.TryParse(Text, out decimal m);
                Text = m == 0 ? "" : m.ToString(Format);
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            decimal.TryParse(Text, out decimal m);
            Text = m == 0 ? "" : m.ToString();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                if (!decimal.TryParse(Text, out decimal m))
                {
                    Text = "";
                }
                else
                {
                    Text = m.ToString(Format);
                }
            }
            base.OnLostFocus(e);
        }

        private void OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            string str = args.NewText;
            if (string.IsNullOrEmpty(str))
            {
                return;
            }

            args.Cancel = !decimal.TryParse(str, out decimal m);
        }
    }
}
