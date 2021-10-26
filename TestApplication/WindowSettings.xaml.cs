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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using NiceLabel.SDK.DemoApp;



namespace NiceLabel.SDK
{
    /// <summary>
    /// Логика взаимодействия для WindowSettings.xaml
    /// </summary>
    public partial class WindowSettings : Window
    {
        NiceLabel.SDK.DemoApp.MainWindow MW;

        SqlCommand command;
        SqlDataAdapter adapter;

        MainWindowViewModel MVM;

        public WindowSettings(NiceLabel.SDK.DemoApp.MainWindow f, MainWindowViewModel x)
        {
            InitializeComponent();
            MW = f;
            MVM = x;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTables();
            
        }

        private void UpdateTables()
        {
            bool flag = true;
            tb_ComPort.Text = Properties.Settings.Default.ComPort;

            tb_Server.Text = Properties.Settings.Default.Server;
            tb_Db.Text = Properties.Settings.Default.DataBase;
            tb_User.Text = Properties.Settings.Default.User;
            tb_Pwd.Password = Properties.Settings.Default.Pwd;

            cb_scanOnOff.IsChecked = Properties.Settings.Default.ScanOn;

            if (NiceLabel.SDK.DemoApp.MainWindow.connection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    MainWindow.connection.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    flag = false;
                }
            }

            if (flag)
            {
                string sql = "select ID,ModelName,ExternalMarketFlag " +
                             "from MappingModels order by ID";

                command = new SqlCommand(sql, MainWindow.connection);
                adapter = new SqlDataAdapter(command);
                DataTable f = new DataTable();
                adapter.Fill(f);

                dg_autoModels.ItemsSource = f.DefaultView;

                sql = "select ManualLabelsDescID,ModelName " +
                      "from ManualLabelsDesc order by ManualLabelsDescID";

                command = new SqlCommand(sql, MainWindow.connection);
                adapter = new SqlDataAdapter(command);
                DataTable c = new DataTable();
                adapter.Fill(c);

                dg_manualModels.ItemsSource = c.DefaultView;
            }

        }

        private void tb_ComPort_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.ComPort = tb_ComPort.Text;
            Properties.Settings.Default.Save();
        }

        private void tb_Server_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.Server = tb_Server.Text;
            Properties.Settings.Default.Save();
        }

        private void tb_Db_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.DataBase = tb_Db.Text;
            Properties.Settings.Default.Save();
        }

        private void tb_User_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.User = tb_User.Text;
            Properties.Settings.Default.Save();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string ConnectionStr = "server =" + tb_Server.Text + "; database =" + tb_Db.Text + "; user id ="
                                                               + tb_User.Text + "; password =" + tb_Pwd.Password + "; connection timeout = 30";
                SqlConnection connection = new SqlConnection(ConnectionStr);
                connection.Open();

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    MessageBox.Show("Соединение с базой данных успешно установлено!");
                    connection.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка соединения с базой данных.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string ConnectionStr = "server =" + tb_Server.Text + "; database =" + tb_Db.Text + "; user id ="
                                                               + tb_User.Text + "; password =" + tb_Pwd.Password + "; connection timeout = 30";
            NiceLabel.SDK.DemoApp.MainWindow.connection = new SqlConnection(ConnectionStr);
            MW.UpdateCom();
        }

        private void tb_Pwd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Pwd = tb_Pwd.Password;
            Properties.Settings.Default.Save();
        }

        private void cb_scanOnOff_Click(object sender, RoutedEventArgs e)
        {
            if(Properties.Settings.Default.ScanOn)
            {
                Properties.Settings.Default.ScanOn = false;
                cb_scanOnOff.IsChecked = false;

                if(MainWindow._serialPort!=null)
                {
                    if(MainWindow._serialPort.IsOpen)
                    {
                        MainWindow._serialPort.Close();
                    }
                }
            }
            else
            {
                Properties.Settings.Default.ScanOn = true;
                cb_scanOnOff.IsChecked = true;
            }

            Properties.Settings.Default.Save();
        }

        private void btn_AutoModelAdd_Click(object sender, RoutedEventArgs e)
        {
            AddNewLabel f = new AddNewLabel(1,false,"",MVM);
            f.Owner = this;
            f.ShowDialog();
            UpdateTables();
        }

        private void btn_manualModel_Click(object sender, RoutedEventArgs e)
        {
            AddNewLabel f = new AddNewLabel(2,false,"",MVM);
            f.Owner = this;
            f.ShowDialog();
            UpdateTables();
        }

        private void btn_AutoModelChange_Click(object sender, RoutedEventArgs e)
        {
            AutoModelChangeFunc();
        }
        public void AutoModelChangeFunc()
        {
            if (dg_autoModels.SelectedItem != null)
            {
                string id = (dg_autoModels.Columns[0].GetCellContent(dg_autoModels.SelectedItem) as TextBlock).Text;
                AddNewLabel f = new AddNewLabel(1, true, id, MVM);
                f.Owner = this;
                f.ShowDialog();
                UpdateTables();
            }
            else
            {
                MessageBox.Show("Не выбран шаблон!", "Предупреждение");
            }
        }
        private void btn_ManualModelChange_Click(object sender, RoutedEventArgs e)
        {
            ManualModelChangeFunc();
        }
        private void ManualModelChangeFunc()
        {
            if (dg_manualModels.SelectedItem != null)
            {
                string id = (dg_manualModels.Columns[0].GetCellContent(dg_manualModels.SelectedItem) as TextBlock).Text;
                AddNewLabel f = new AddNewLabel(2, true, id, MVM);
                f.Owner = this;
                f.ShowDialog();
                UpdateTables();
            }
            else
            {
                MessageBox.Show("Не выбран шаблон!", "Предупреждение");
            }
        }

        private void dg_autoModels_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AutoModelChangeFunc();
        }

        private void dg_manualModels_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ManualModelChangeFunc();
        }

        private void btn_AutoModelDel_Click(object sender, RoutedEventArgs e)
        {
            if (dg_autoModels.SelectedItem != null)
            {
                string name = (dg_autoModels.Columns[1].GetCellContent(dg_autoModels.SelectedItem) as TextBlock).Text;

                DeleteQuestion f = new DeleteQuestion(name);
                f.Owner = this;

                if (f.ShowDialog() == true)
                {
                    bool flag = true;

                    if (NiceLabel.SDK.DemoApp.MainWindow.connection.State != System.Data.ConnectionState.Open)
                    {
                        try
                        {
                            MainWindow.connection.Open();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            flag = false;
                        }
                    }

                    if (flag)
                    {
                        string id = (dg_autoModels.Columns[0].GetCellContent(dg_autoModels.SelectedItem) as TextBlock).Text;

                        string sql = "delete from MappingModels where ID=" + id + ";";
                        command = new SqlCommand(sql, MainWindow.connection);
                        command.ExecuteNonQuery();
                    }
                }
                UpdateTables();
            }
            else
            {
                MessageBox.Show("Не выбран шаблон!", "Предупреждение");
            }
        }

        private void btn_ManualDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dg_manualModels.SelectedItem != null)
            {
                string name = (dg_manualModels.Columns[1].GetCellContent(dg_manualModels.SelectedItem) as TextBlock).Text;

                DeleteQuestion f = new DeleteQuestion(name);
                f.Owner = this;

                if (f.ShowDialog() == true)
                {
                    bool flag = true;

                    if (NiceLabel.SDK.DemoApp.MainWindow.connection.State != System.Data.ConnectionState.Open)
                    {
                        try
                        {
                            MainWindow.connection.Open();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            flag = false;
                        }
                    }

                    if (flag)
                    {
                        string id = (dg_manualModels.Columns[0].GetCellContent(dg_manualModels.SelectedItem) as TextBlock).Text;

                        string sql = "delete from ManualLabelsDesc where ManualLabelsDescID=" + id + ";";
                        command = new SqlCommand(sql, MainWindow.connection);
                        command.ExecuteNonQuery();
                    }
                }
                UpdateTables();
            }
            else
            {
                MessageBox.Show("Не выбран шаблон!", "Предупреждение");
            }
        }
    }
}
