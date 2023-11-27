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
using DiplomGromov.Models;
using RiakClient.Models;
using static DiplomGromov.Settings;

namespace DiplomGromov.Pages
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Page
    {
        public Auth()
        {
            InitializeComponent();
        }
        public void CheckUser(string login, string password)
        {
            if (client.Get("Users", login).ResultCode != RiakClient.ResultCode.NotFound)
            {
                Users mUser = client.Get("Users", login).Value.GetObject<Users>();
                if (mUser.password == password)
                {
                    int id = 0;
                    while (client.Get("TasksResults", id.ToString()).ResultCode != RiakClient.ResultCode.NotFound)
                    {
                        TasksResults tasksResults = client.Get("TasksResults", id.ToString()).Value.GetObject<TasksResults>();
                        if (tasksResults.studentLogin == mUser.login)
                        {
                            lastSolvetTaskID = id;
                        }

                        id++;
                    }
                    user = mUser;
                    mw.changeMenu();
                    MainContent.Content = new Pages.Profile();
                    
                }
                else
                {
                    MessageBox.Show("Неправильный пароль!");
                }

            }
            else
            {
                MessageBox.Show("Такой пользователь не существует!");
            }
        }
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            CheckUser(Login.Text,Password.Text);
        }
    }
}
