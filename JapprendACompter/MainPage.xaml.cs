using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace JapprendACompter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int _leftOperand;
        private int _rightOperand;

        private int _wrongAnswerCount;

        public MainPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateOperation();
        }

        private async void AnswerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (CheckAnswer())
            {
                case true:
                    await ManageRightAnswer();
                    break;
                case false:
                    await ManageWrongAnswer();
                    break;
            }
        }

        private async Task ManageRightAnswer()
        {
            await ShowMessage("Bravo !", TimeSpan.FromSeconds(2));
            GenerateOperation();
        }

        private async Task ManageWrongAnswer()
        {
            _wrongAnswerCount++;

            if (_wrongAnswerCount >= 3)
            {
                var expectedAnswer = (_leftOperand + _rightOperand).ToString();
                await ShowMessage("Perdu :-(\nLa bonne réponse était\n" + expectedAnswer, TimeSpan.FromSeconds(5));
                GenerateOperation();
            }
        }

        private async Task ShowMessage(string message, TimeSpan duration)
        {
            MessageTextBlock.Text = message;
            FullScreenMessagePanel.Visibility = Visibility.Visible;
            this.ContentPanel.Visibility = Visibility.Collapsed;

            await Task.Delay(duration);

            FullScreenMessagePanel.Visibility = Visibility.Collapsed;
            this.ContentPanel.Visibility = Visibility.Visible;
        }

        private bool? CheckAnswer()
        {
            var expectedAnswer = (_leftOperand + _rightOperand).ToString();
            var actualAnswer = AnswerTextBox.Text;

            if (actualAnswer == expectedAnswer)
            {
                return true;
            }
            else if (actualAnswer.Length < expectedAnswer.Length && expectedAnswer.Contains(actualAnswer))
            {
                return null;
            }
            else
            {
                return false;
            }
        }

        private void GenerateOperation()
        {
            _wrongAnswerCount = 0;
            var rnd = new Random();
            _leftOperand = rnd.Next(9) + 1;
            _rightOperand = rnd.Next(9) + 1;

            OperationTextblock.Text = $"{_leftOperand} + {_rightOperand} = ?";
            AnswerTextBox.Text = "";
            AnswerTextBox.Focus(FocusState.Programmatic);
        }
    }
}
