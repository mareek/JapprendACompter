using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

        private Stopwatch _chrono;

        public MainPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateOperation();

            var getupppa = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            getupppa.Tick += getupppa_Tick;
            getupppa.Start();

        }

        private async void getupppa_Tick(object sender, object e)
        {
            if (_chrono != null && _chrono.Elapsed > TimeSpan.FromSeconds(15))
            {
                await ShowMessage(ResponseLevel.TooSlow);
            }
        }

        private async void AnswerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var expectedAnswer = (_leftOperand + _rightOperand).ToString();
            var actualAnswer = AnswerTextBox.Text;

            if (actualAnswer == expectedAnswer)
            {
                await ShowMessage(GetResponseLevel());
            }
            else if (actualAnswer.Length >= expectedAnswer.Length || !expectedAnswer.Contains(actualAnswer))
            {
                _wrongAnswerCount++;
            }

            if (_wrongAnswerCount >= 3)
            {
                await ShowMessage(ResponseLevel.Wrong);
            }
        }

        private ResponseLevel GetResponseLevel()
        {
            if (_chrono == null || _chrono.Elapsed < TimeSpan.FromSeconds(5))
            {
                return ResponseLevel.Fast;
            }
            else if (_chrono.Elapsed < TimeSpan.FromSeconds(10))
            {
                return ResponseLevel.Normal;
            }
            else if (_chrono.Elapsed < TimeSpan.FromSeconds(15))
            {
                return ResponseLevel.Slow;
            }
            else
            {
                return ResponseLevel.TooSlow;
            }
        }

        private async Task ShowMessage(ResponseLevel responseLevel)
        {
            _chrono = null;
            var expectedAnswer = (_leftOperand + _rightOperand).ToString();
            switch (responseLevel)
            {
                case ResponseLevel.Fast:
                    await ShowMessage("Bravo !", TimeSpan.FromSeconds(2));
                    break;
                case ResponseLevel.Normal:
                    await ShowMessage("C'est bien.", TimeSpan.FromSeconds(2));
                    break;
                case ResponseLevel.Slow:
                    await ShowMessage("Juste mais lent...", TimeSpan.FromSeconds(3));
                    break;
                case ResponseLevel.TooSlow:
                    await ShowMessage("Trop lent !\nLa bonne réponse était\n" + expectedAnswer, TimeSpan.FromSeconds(5));
                    break;
                case ResponseLevel.Wrong:
                    await ShowMessage("Perdu :-(\nLa bonne réponse était\n" + expectedAnswer, TimeSpan.FromSeconds(5));
                    break;
            }

            _chrono = Stopwatch.StartNew();
            GenerateOperation();
        }

        private async Task ShowMessage(string message, TimeSpan duration)
        {
            MessageTextBlock.Text = message;
            FullScreenMessagePanel.Visibility = Visibility.Visible;
            ContentPanel.Visibility = Visibility.Collapsed;
            InputPane.GetForCurrentView().TryHide();

            await Task.Delay(duration);

            FullScreenMessagePanel.Visibility = Visibility.Collapsed;
            ContentPanel.Visibility = Visibility.Visible;
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
