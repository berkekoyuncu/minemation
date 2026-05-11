using System.Windows;
using System.Windows.Controls;

namespace Minemation.Desktop
{
    public partial class CreateReportView : UserControl
    {
        public CreateReportView(string reportType)
        {
            InitializeComponent();
            TxtReportType.Text = reportType;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            mainWindow.MainContent.Content = new ReportsView();
        }
    }
}
