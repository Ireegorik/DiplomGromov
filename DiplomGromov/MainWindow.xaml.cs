using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RiakClient;
using RiakClient.Models;
using static DiplomGromov.Settings;
namespace DiplomGromov
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentFrame.Content = new Pages.Auth();
            client = RiakCluster.FromConfig("riakConfig","Riak.Config").CreateClient();
            if (client.Ping().IsSuccess)
            {
                MessageBox.Show("База данных успешно подключена!");
            }
            else
            {
                MessageBox.Show("База данных не подключена!");
            }
            MainContent = ContentFrame;
            mw = this;
            MenuAuth.Visibility = Visibility.Visible;
            MenuProgram.Visibility = Visibility.Hidden;
        }
        public void changeMenu()
        {
            MenuAuth.Visibility = Visibility.Hidden;
            MenuProgram.Visibility = Visibility.Visible;
        }
        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Pages.Auth();
        }

        private void RegisButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Pages.Regis();
        }

        private void TrainerButton_Click(object sender, RoutedEventArgs e)
        {
            if (client.Get("Tasks", (lastSolvetTaskID + 1).ToString()).ResultCode != RiakClient.ResultCode.NotFound)
            {
                MainContent.Content = new Pages.AnswerPractis(lastSolvetTaskID + 1);
            }
            else
            {
                MessageBox.Show("На данный момент нет новых задач");
            }
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Pages.Profile();
        }

        private void ProgressButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Pages.Progress();
        }

        private void TaskCreate_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Pages.CreateTask();
        }
    }
}
