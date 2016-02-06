using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace JapprendACompter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
            Stats.StatFile.LoadFileAsync();
        }

        private void AdditionButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ExercicePage), "Addition");
        }

        private void MultiplicationButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ExercicePage), "Multiplication");
        }

        private void StatLink_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StatPage));
        }
    }
}
