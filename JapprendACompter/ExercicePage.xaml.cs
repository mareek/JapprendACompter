using System;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace JapprendACompter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExercicePage : Page
    {
        private ExerciceBase _exercice;
        private DispatcherTimer _getupppa = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };

        public ExercicePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            switch (e.Parameter.ToString())
            {
                case "Addition":
                    _exercice = new Addition();
                    break;
                case "Multiplication":
                    _exercice = new Multiplication();
                    break;
                default:
                    throw new NotSupportedException("Opération inconnue : " + e.Parameter);
            }

            ShowOperation();

            _getupppa.Tick += getupppa_Tick;
            _getupppa.Start();
        }

        private void ShowOperation()
        {
            QuestionTextblock.Text = _exercice.Question;
            AnswerTextBox.Text = "";
            AnswerTextBox.Focus(FocusState.Programmatic);
        }

        private async void getupppa_Tick(object sender, object e)
        {
            await CheckAnswer();
        }

        private async void AnswerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            await CheckAnswer(AnswerTextBox.Text);
        }

        private async Task CheckAnswer(string answer = "")
        {
            var message = _exercice.CheckAnswer(answer);

            if (message != null)
            {
                await ShowMessage(message);
                if (_exercice.TryShowNextOperation())
                {
                    ShowOperation();
                }
                else
                {
                    await ShowMessage(_exercice.GetEndMessage());
                }
            }
        }

        private async Task ShowMessage(ExerciceBase.Message message)
        {
            _getupppa.Stop();
            MessageTextBlock.Text = message.Text;
            FullScreenMessagePanel.Visibility = Visibility.Visible;
            ContentPanel.Visibility = Visibility.Collapsed;
            InputPane.GetForCurrentView().TryHide();

            await Task.Delay(message.Duration);

            FullScreenMessagePanel.Visibility = Visibility.Collapsed;
            ContentPanel.Visibility = Visibility.Visible;
            _getupppa.Start();
        }
    }
}
