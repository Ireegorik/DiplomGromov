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
using static DiplomGromov.Settings;
using RiakClient;
using RiakClient.Models;
using DiplomGromov.Models;
namespace DiplomGromov.Pages
{
    /// <summary>
    /// Логика взаимодействия для AnswerPractis.xaml
    /// </summary>
    public partial class AnswerPractis : Page
    {
        Tasks tasks;
        public AnswerPractis(int id)
        {
            InitializeComponent();
            tasks = client.Get("Tasks", id.ToString()).Value.GetObject<Tasks>();
            TextTasks.Text = tasks.text;
        }
        void checkAnswer(string answer)
        {
            lastSolvetTaskID = tasks.ID;
            string res = "неправильно";
            if (tasks.query.ToLower().Trim(' ').Contains(answer.ToLower()))
            {
                MessageBox.Show("Правильно!");
                res = "правильно";
            }
            else
            {
                MessageBox.Show("Неправильно!");
                res = "неправильно";
            }
            int id = 0;
            while (client.Get("TasksResults", id.ToString()).ResultCode != RiakClient.ResultCode.NotFound)
            {
                id++;
            }
            RiakObject o = new RiakObject("TasksResults", id.ToString(), new TasksResults { ID = id, AnswerStudent = AnswerTasks.Text,result = res,studentLogin = user.login });
            client.Put(o);
            if (client.Get("Tasks", (lastSolvetTaskID + 1).ToString()).ResultCode != RiakClient.ResultCode.NotFound)
            {
                MainContent.Content = new Pages.AnswerPractis(lastSolvetTaskID + 1);
            }
            else
            {
                MessageBox.Show("На данный момент нет новых задач");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            checkAnswer(AnswerTasks.Text);
        }
    }
}
