using System.Windows;

namespace FloorMaster_Partners
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new ListPartnersPage());
        }
    }
}
