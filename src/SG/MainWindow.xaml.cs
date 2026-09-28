using System.Windows;
using SG.Services;

namespace SG
{
    public partial class MainWindow : Window
    {
        private readonly ProcessService processes = new ProcessService();
        private readonly SettingsService settings = new SettingsService();
        public MainWindow() { InitializeComponent(); settings.Load(); FilterBox.Text=settings.SavedId; Refresh(); }
        private void Refresh_Click(object sender, RoutedEventArgs e) { Refresh(); }
        private void RefreshSelected_Click(object sender, RoutedEventArgs e) { Refresh(); }
        private void Refresh() {
            var rows=processes.GetProcesses(FilterBox.Text.Trim());
            ProcessGrid.ItemsSource=rows; CountText.Text=rows.Count+"개"; StatusText.Text="프로세스 목록을 갱신했습니다.";
        }
        private void Close_Click(object sender, RoutedEventArgs e) {
            var row=ProcessGrid.SelectedItem as SG.Models.ProcessRow;
            if(row==null){StatusText.Text="프로세스를 선택하세요.";return;}
            StatusText.Text=processes.RequestClose(row.Id) ? row.Name+"에 정상 종료 요청을 보냈습니다." : "정상 종료 요청을 처리하지 못했습니다.";
            Refresh();
        }
    }
}