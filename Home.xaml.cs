
using System.Windows;
using System.Windows.Controls;

namespace FileComparer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
            LoadSessions();
        }
        public void LoadSessions()
        {
            sessionListBox.ItemsSource = SessionManager.SessionNames;
        }

        private void sessionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Load selected session when user clicks on it
            string? selectedSessionName = sessionListBox.SelectedItem as string;
            if (selectedSessionName!=null)
            {
                try
                {
                    string sessionData = SessionManager.GetSessionData(selectedSessionName);
                   
                    // Pass session data to the file comparison window
                    CompareWindow fileComparisonWindow = new CompareWindow(selectedSessionName,sessionData);
                    this.Hide();
                    fileComparisonWindow.Show();
                   
                }
                catch(ArgumentException ex) {
                    MessageBox.Show(ex.Message);

                }
            }

        }
        private void window(object o, RoutedEventArgs e)
        {
            this.Hide();
            CompareWindow m = new CompareWindow();
            m.Show();
        }
        private void syncwindow(object o,RoutedEventArgs e)
        {
            this.Hide();
            Syncfolder w = new Syncfolder();
            w.Show();
        }
    }
}
