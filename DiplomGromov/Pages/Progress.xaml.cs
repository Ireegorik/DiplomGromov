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
    /// Логика взаимодействия для Progress.xaml
    /// </summary>
    public partial class Progress : Page
    {
        int MaxTasks = 0;
        public Progress()
        {
            InitializeComponent();
            
            while (client.Get("Tasks", MaxTasks.ToString()).ResultCode != RiakClient.ResultCode.NotFound)
            {
                MaxTasks++;
            }
            Bar.Maximum = MaxTasks * 10;
            Bar.Value = lastSolvetTaskID * 10;
            List<TasksResults> results = new List<TasksResults>();
            int id = 0;
            while (client.Get("TasksResults", id.ToString()).ResultCode != RiakClient.ResultCode.NotFound)
            {
                TasksResults result = client.Get("TasksResults", id.ToString()).Value.GetObject<TasksResults>();
                if (result.studentLogin == user.login) results.Add(result);
                id++;
            }
            ListAnswers.ItemsSource = results;
        }
    }
}
