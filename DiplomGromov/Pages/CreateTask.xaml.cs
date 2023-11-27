using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для CreateTask.xaml
    /// </summary>
    public partial class CreateTask : Page
    {
        DataTable dataTable;
        string columnHeader = "";
        public CreateTask()
        {
            InitializeComponent();
            List<string> Actions = new List<string> { "Put", "Get" };
            ComboActions.ItemsSource = Actions;
            Datas.CanUserAddRows = true;
            dataTable = new DataTable();
            Datas.ItemsSource = dataTable.DefaultView;
            if (Datas.ColumnHeaderStyle == null)
            {
                Datas.ColumnHeaderStyle = new Style(typeof(DataGridColumnHeader));
            }
            Datas.ColumnHeaderStyle.Setters.Add(new EventSetter(ButtonBase.ClickEvent,
                 new RoutedEventHandler(columnHeaderClick)));
        }

        private void ComboActions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((string)ComboActions.SelectedItem)
            {
                case "Put":

                    break;
                case "Get":

                    break;
            }
        }

        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {



            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = NameColumn.Text;
            textColumn.Binding = new Binding(NameColumn.Text);
            Datas.Columns.Add(textColumn);
            dataTable.Columns.Add(new DataColumn(NameColumn.Text));
            Datas.ItemsSource = dataTable.DefaultView;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            getAnswer();

        }
        private void columnHeaderClick(object sender, RoutedEventArgs e)
        {
            columnHeader = (string)(sender as DataGridColumnHeader).Content;
        }
        void getAnswer()
        {
            string param = "";
            switch ((string)ComboActions.SelectedItem){
                case "Put":
                    TextAnswer.Text = "";
                    for (int i = 0; i < Datas.Items.Count-1; i++)
                    {
                        param = "";
                        if (TextAnswer.Text != "") TextAnswer.Text += "\n";
                        param += "{ ";
                        object[] d = dataTable.Rows[i].ItemArray;
                        int j = 0;
                        foreach(object c in d)
                        {
                            if(j>0) param += ",";
                            param += $"{dataTable.Columns[j].ColumnName} = {dataTable.Rows[i][j]}";
                            j++;
                        }
                        param += "}";
                        TextAnswer.Text += $"client.Put('{NameTable.Text}',{columnHeader} , new RiakObject{param});";
                    }
                    
                    break;
                case "Get":
                    TextAnswer.Text = "";
                    for (int i = 0; i < Datas.Items.Count - 1; i++)
                    {
                        param = "";
                        if (TextAnswer.Text != "") TextAnswer.Text += "\n";
                        object d = dataTable.Rows[i].ItemArray[0];
                        param += $"{dataTable.Rows[i][0]}";
                        TextAnswer.Text += $"client.Get(\"{NameTable.Text}\" , \"{param}\");";
                    }
                    break;
            }
            int id = 0;
            while(client.Get("Tasks", id.ToString()).ResultCode != RiakClient.ResultCode.NotFound)
            {
                id++;
            }
            RiakObject o = new RiakObject("Tasks", id.ToString(), new Tasks {ID=id,adminLogin=Settings.user.login,query=TextAnswer.Text,text=TextTask.Text });
            client.Put(o);
            MessageBox.Show("Задание успешно создано!");
            //MessageBox.Show(client.Get("Tasks", id.ToString()).Value.GetObject<Tasks>().query);
        }
        private void Datas_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            getAnswer();
        }

        private void RowColumn_Click(object sender, RoutedEventArgs e)
        {

            dataTable.Rows.Add();
            Datas.Items.Refresh();

        }
        private void Datas_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            getAnswer();
        }
    }
}
