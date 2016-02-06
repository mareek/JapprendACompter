using JapprendACompter.Stats;
using System;
using System.Threading;
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

        private CancellationTokenSource _messagePanelCancellationTokenSource = null;
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
                    await StatFile.Instance.Save();
                    await ShowMessage(_exercice.GetEndMessage());
                    Frame.Navigate(typeof(HomePage));
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

            using (_messagePanelCancellationTokenSource = new CancellationTokenSource())
            {
                try
                {
                    await Task.Delay(message.Duration, _messagePanelCancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    /* avoid ghost event that prevent the input field from keeping focus */
                    await Task.Delay(TimeSpan.FromSeconds(0.1));
                }
            }

            _messagePanelCancellationTokenSource = null;
            FullScreenMessagePanel.Visibility = Visibility.Collapsed;
            ContentPanel.Visibility = Visibility.Visible;
            _getupppa.Start();
        }

        private void FullScreenMessagePanel_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_messagePanelCancellationTokenSource != null && !_messagePanelCancellationTokenSource.IsCancellationRequested)
            {
                _messagePanelCancellationTokenSource.Cancel();
            }
        }
    }
}
