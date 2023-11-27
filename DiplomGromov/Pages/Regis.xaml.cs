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
    /// Логика взаимодействия для Regis.xaml
    /// </summary>
    public partial class Regis : Page
    {
        public Regis()
        {
            InitializeComponent();
        }
        public void Register(string login,string password,string role)
        {
            if(client.Get("Users", login).ResultCode == RiakClient.ResultCode.NotFound)
            {
                Users newUser = new Users
                {
                    login = login,
                    password = password,
                    role = role,
                    dateOfBirth = "Дата рождения не установленна",
                    famalyName = "Фамилия не установлена",
                    fatherName = "Отчество не установлено",
                    name = "Имя не установлено"
                };
                var o = new RiakObject("Users", newUser.login, newUser);
                if (client.Put(o).ResultCode == RiakClient.ResultCode.Success)
                {
                    user = newUser;
                    mw.changeMenu();
                    MainContent.Content = new Pages.Profile();
                    
                }
                else
                {
                    MessageBox.Show("Что-то пошло не так");
                }

            }
            else
            {
                MessageBox.Show("Такой пользователь уже существует");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Register(Login.Text, Password.Text, "пользователь");
        }
    }
}
