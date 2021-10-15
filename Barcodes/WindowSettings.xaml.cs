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


namespace NiceLabel.SDK
{
    /// <summary>
    /// Логика взаимодействия для WindowSettings.xaml
    /// </summary>
    public partial class WindowSettings : Window
    {
        NiceLabel.SDK.DemoApp.MainWindow MW;
        public WindowSettings(NiceLabel.SDK.DemoApp.MainWindow f)
        {
            InitializeComponent();
            MW = f;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tb_ComPort.Text = Properties.Settings.Default.ComPort;

            tb_Server.Text = Properties.Settings.Default.Server;
            tb_Db.Text = Properties.Settings.Default.DataBase;
            tb_User.Text = Properties.Settings.Default.User;
            tb_Pwd.Password = Properties.Settings.Default.Pwd;

            tb_trans.Text = Properties.Settings.Default.PathTransLabel;
            tb_individual.Text = Properties.Settings.Default.PathIndividualLabel;
            tb_individual_WY.Text = Properties.Settings.Default.PathIndividualWYLabel;

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

        private void btn_trans_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog OPF = new System.Windows.Forms.OpenFileDialog();
            if (OPF.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_trans.Text = OPF.FileName;

                Properties.Settings.Default.PathTransLabel = OPF.FileName;
                Properties.Settings.Default.Save();
            }
            
            
        }

        private void btn_indiv_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog OPF = new System.Windows.Forms.OpenFileDialog();
            if (OPF.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_individual.Text = OPF.FileName;

                Properties.Settings.Default.PathIndividualLabel = OPF.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void btn_ind_WY_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog OPF = new System.Windows.Forms.OpenFileDialog();
            if (OPF.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_individual_WY.Text = OPF.FileName;

                Properties.Settings.Default.PathIndividualWYLabel = OPF.FileName;
                Properties.Settings.Default.Save();
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
    }
}
