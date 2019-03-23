#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace AngelBoard.Controls
{
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
            if (String.IsNullOrEmpty(str) || str == "-")
            {
                return;
            }

            args.Cancel = !Decimal.TryParse(str, out decimal m);
        }
    }
}
