//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Euro Plus">
//     Copyright © Euro Plus 2014.
// </copyright>
// <summary>This is the MainWindow Window.</summary>
//-----------------------------------------------------------------------
namespace NiceLabel.SDK.DemoApp
{
    using System.Windows;
    using System.IO.Ports;
    using System.Threading;
    using System.Windows.Threading;
    using System.Data.SqlClient;
    using System.Data;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MainWindow Window.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>

        public static SerialPort _serialPort;
        private delegate void SetTextDeleg(string text);

        public static string connectionString;
        string curCom;
        SqlCommand command;
        SqlDataAdapter adapter;
        DataTable BarcodesTable;
        DataTable BarcodesTable2;
        public static SqlConnection connection;

        public static string currentBarcode;

        System.Windows.Controls.TextBlock art;
        System.Windows.Controls.TextBlock name;
        System.Windows.Controls.TextBlock count;
        System.Windows.Controls.TextBlock barcode;

        public static bool artToLowFlag = false;
        public static string BarCodeFromTbl;

        public MainWindow()
        {
            this.InitializeComponent();

            this.DataContext = new MainWindowViewModel();

            connectionString = "server =" + Properties.Settings.Default.Server + 
                               "; database =" + Properties.Settings.Default.DataBase+
                               "; user id ="+ Properties.Settings.Default.User +
                               "; password =" + Properties.Settings.Default.Pwd + 
                               "; connection timeout = 30";

            connection = new SqlConnection(connectionString);
            BarcodesTable = new DataTable();
            BarcodesTable2 = new DataTable();

            art = new TextBlock();
            name = new TextBlock();
            count = new TextBlock();
            barcode = new TextBlock();

            currentBarcode = "";

           

        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            WindowSettings WSett = new WindowSettings(this);

            curCom = Properties.Settings.Default.ComPort;

            WSett.Owner = this;
           // this.Hide();
            WSett.ShowDialog();
           // this.Show();
            

            UpdateCom();
            
        }

        public void UpdateCom()
        {
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
                }
                OpenComPort();
            }
        }

        public void sp_DataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(500);

                SerialPort sp = (SerialPort)sender;
                string indata = sp.ReadExisting();

                this.Dispatcher.BeginInvoke(new SetTextDeleg(si_DataReceived), new object[] { indata });
            }
            catch(System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void si_DataReceived(string data)
        {
            currentBarcode = data;
            //UpdateTable();
            //NiceLabel.SDK.MainWindowViewModel.BarcodeUP(data);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenComPort();
            //UpdateTable();
        }

        public void OpenComPort() 
        {
            try
            {
                if (Properties.Settings.Default.ScanOn == true)
                {
                    _serialPort = new SerialPort(Properties.Settings.Default.ComPort);
                    _serialPort.Handshake = Handshake.None;
                    _serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataRecieved);
                    _serialPort.WriteTimeout = 500;

                    _serialPort.Open();
                }
            }
            catch
            {
                MessageBox.Show("Не удалось подключить сканер штрихкодов.");
            }
        }

        //private void checkBox_trans_Click(object sender, RoutedEventArgs e)
        //{
        //    if (checkBox_trans.IsChecked == true)
        //    {
        //        checkBox_individual.IsChecked = false;
        //        checkBox_individualWY.IsChecked = false;

        //        UpdateTable();
        //        UpdateLabelView(0, tb_findBarcode.Text);
        //    }
        //    else
        //    {
        //        checkBox_trans.IsChecked = true;
        //    }
        //}

        //private void checkBox_individual_Click(object sender, RoutedEventArgs e)
        //{
        //    if (checkBox_individual.IsChecked == true)
        //    {
        //        checkBox_trans.IsChecked = false;
        //        checkBox_individualWY.IsChecked = false;

        //        UpdateTable();
        //        UpdateLabelView(1, tb_findBarcode.Text);
        //    }
        //    else
        //    {
        //        checkBox_individual.IsChecked = true;
        //    }
        //}

        //private void checkBox_individualWY_Click(object sender, RoutedEventArgs e)
        //{
        //    if(checkBox_individualWY.IsChecked==true)
        //    {
        //        checkBox_trans.IsChecked = false;
        //        checkBox_individual.IsChecked = false;

        //        UpdateTable();
        //        UpdateLabelView(2, tb_findBarcode.Text);
        //    }
        //    else 
        //    {
        //        checkBox_individualWY.IsChecked = true;
        //    }
        //}

        private void UpdateLabelView(int mode, string currentArt)
        {

            string sql = "";
            bool CanContinue =true;
            DataTable f = new DataTable();

            if (mode==0)
            {
                sql = "select Артикул,НаименованиеПолное,ЕденицаИзмерения,Количество,Штрихкод from NiceLabel where Артикул LIKE '" +
                                        currentArt + "%' and Количество >1 ;";

                command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);

                if (connection.State != ConnectionState.Open)
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        CanContinue = false;
                    }
                }

                if (CanContinue)
                {
                   
                    adapter.Fill(f);

                    if(f.Rows.Count ==1)
                    {
                        NiceLabel.SDK.MainWindowViewModel.BarcodeChoose(f.Rows[0][1].ToString(), f.Rows[0][0].ToString(), f.Rows[0][4].ToString(), System.Convert.ToInt32(f.Rows[0][3]), mode, (NiceLabel.SDK.MainWindowViewModel)DataContext);
                    }
                    else
                    {
                        NiceLabel.SDK.MainWindowViewModel.BarcodeChoose("", "", "", 0, 0, (NiceLabel.SDK.MainWindowViewModel)DataContext);
                    }
                }
            }
            else if(mode==1 || mode == 2)
            {
                sql = "select Артикул,НаименованиеПолное,Штрихкод from NiceLabel where Артикул LIKE '" +
                                     tb_findBarcode.Text + "%'  and Количество = 1;";

                command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);

                if (connection.State != ConnectionState.Open)
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        CanContinue = false;
                    }
                }

                if (CanContinue)
                {
                    adapter.Fill(f);

                    if (f.Rows.Count == 1)
                    {
                        NiceLabel.SDK.MainWindowViewModel.BarcodeChoose(f.Rows[0][1].ToString(), f.Rows[0][0].ToString(), f.Rows[0][2].ToString(), 1, mode, (NiceLabel.SDK.MainWindowViewModel)DataContext);
                    }
                    else
                    {
                        NiceLabel.SDK.MainWindowViewModel.BarcodeChoose("", "", "", 0, mode, (NiceLabel.SDK.MainWindowViewModel)DataContext);
                    }
                }
            }

        }

        //private void UpdateTable()
        //{
        //    try
        //    {
        //        bool CanContinue = true;
        //        string sql = "";

        //        string _name = "";
        //        string _art = "";
        //        string _barcode = "";
        //        string _count = "";
        //        int _mode = 0;

        //        if (checkBox_trans.IsChecked == true)
        //        {

        //            if (currentBarcode == "")
        //            {
        //                sql = "select Артикул,НаименованиеПолное,ЕденицаИзмерения,Количество,Штрихкод from NiceLabel where (Артикул LIKE '" +
        //                                tb_findBarcode.Text + "%' or НаименованиеПолное LIKE '%" + tb_findBarcode.Text + "%') and Количество >1;";
        //            }
        //            else
        //            {
        //                sql = "select Артикул,НаименованиеПолное,ЕденицаИзмерения,Количество,Штрихкод from NiceLabel where Штрихкод='" + currentBarcode + "';";
        //            }
        //            command = new SqlCommand(sql, connection);
        //            adapter = new SqlDataAdapter(command);

        //            if (connection.State != ConnectionState.Open)
        //            {
        //                try
        //                {
        //                    connection.Open();
        //                }
        //                catch (System.Exception ex)
        //                {
        //                    MessageBox.Show(ex.Message);
        //                    CanContinue = false;
        //                }
        //            }

        //            if (CanContinue)
        //            {
        //                BarcodesTable.Clear();
        //                adapter.Fill(BarcodesTable);

        //                if (currentBarcode != "" && BarcodesTable.Rows.Count != 0)
        //                {
        //                    _art = BarcodesTable.Rows[0].ItemArray.GetValue(0).ToString();
        //                    _name = BarcodesTable.Rows[0].ItemArray.GetValue(1).ToString();
        //                    _count = BarcodesTable.Rows[0].ItemArray.GetValue(3).ToString();
        //                    _barcode = BarcodesTable.Rows[0].ItemArray.GetValue(4).ToString();


        //                    tb_findBarcode.Text = _art;

        //                    NiceLabel.SDK.MainWindowViewModel.BarcodeChoose(_name, _art, _barcode, System.Convert.ToInt32(_count), _mode, (NiceLabel.SDK.MainWindowViewModel)DataContext);
        //                }                      
        //                dataGrid_barcodes.ItemsSource = BarcodesTable.DefaultView; 
        //            }
        //        }
        //        else if (checkBox_individual.IsChecked == true || checkBox_individualWY.IsChecked == true)
        //        {
        //            if (checkBox_individual.IsChecked == true)
        //            {
        //                _mode = 1;
        //            }
        //            else { _mode = 2; }

        //            if (currentBarcode == "")
        //            {
        //                sql = "select Артикул,НаименованиеПолное,Штрихкод from NiceLabel where (Артикул LIKE '" +
        //                            tb_findBarcode.Text + "%' or НаименованиеПолное LIKE '%"+tb_findBarcode.Text+"%') and Количество = 1;";
        //            }
        //            else
        //            {
        //                sql = "select Артикул,НаименованиеПолное,Штрихкод from NiceLabel where Штрихкод='" + currentBarcode + "';";
        //            }
        //            command = new SqlCommand(sql, connection);
        //            adapter = new SqlDataAdapter(command);

        //            if (connection.State != ConnectionState.Open)
        //            {
        //                try
        //                {
        //                    connection.Open();
        //                }
        //                catch (System.Exception ex)
        //                {
        //                    MessageBox.Show(ex.Message);
        //                    CanContinue = false;
        //                }
        //            }

        //            if (CanContinue)
        //            {
        //                BarcodesTable2.Clear();
        //                adapter.Fill(BarcodesTable2);

        //                if (currentBarcode != "" && BarcodesTable2.Rows.Count != 0)
        //                {
        //                    _art = BarcodesTable2.Rows[0].ItemArray.GetValue(0).ToString();
        //                    _name = BarcodesTable2.Rows[0].ItemArray.GetValue(1).ToString();
        //                    _count = "1";
        //                    _barcode = BarcodesTable2.Rows[0].ItemArray.GetValue(2).ToString();


        //                    tb_findBarcode.Text = _art;

        //                    NiceLabel.SDK.MainWindowViewModel.BarcodeChoose(_name, _art, _barcode, System.Convert.ToInt32(_count), _mode, (NiceLabel.SDK.MainWindowViewModel)DataContext);
        //                }
        //                dataGrid_barcodes.ItemsSource = BarcodesTable2.DefaultView;
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Не выбран тип этикетки!", "Предупреждение");
        //            tb_findBarcode.Text = "";
        //        }

        //        if (currentBarcode != "")
        //        {
        //            currentBarcode = "";
        //        }
        //    }
        //    catch(System.Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void dataGrid_barcodes_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            //System.Windows.Controls.TextBlock art = dataGrid_barcodes.Columns[0].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;
            //System.Windows.Controls.TextBlock name = dataGrid_barcodes.Columns[1].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;
            //System.Windows.Controls.TextBlock count = dataGrid_barcodes.Columns[3].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;
            //System.Windows.Controls.TextBlock barcode = dataGrid_barcodes.Columns[4].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;


            //int mode = 0;

            //if (checkBox_trans.IsChecked == true)
            //{
            //    mode = 0;

            //    art = dataGrid_barcodes.Columns[0].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;
            //    name = dataGrid_barcodes.Columns[1].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;
            //    count = dataGrid_barcodes.Columns[3].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;
            //    barcode = dataGrid_barcodes.Columns[4].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;

            //}
            //else if (checkBox_individual.IsChecked==true)
            //{
            //    mode = 1;
            //}
            //else
            //{
            //    mode = 2;
            //}

            //if (mode == 1 || mode == 2)
            //{
            //    art = dataGrid_barcodes.Columns[0].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;
            //    name = dataGrid_barcodes.Columns[1].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;
            //    count.Text = "1";
            //    barcode = dataGrid_barcodes.Columns[2].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;
            //}

            //NiceLabel.SDK.MainWindowViewModel.BarcodeChoose(name.Text,art.Text, barcode.Text, System.Convert.ToInt32(count.Text),mode,(NiceLabel.SDK.MainWindowViewModel)DataContext);
            //BarcodeClick();
        }

        //private void BarcodeClick()
        //{
        //    try
        //    {
        //        int mode = 0;

        //        if (checkBox_trans.IsChecked == true)
        //        {
        //            mode = 0;

        //            art = dataGrid_barcodes.Columns[0].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;
        //            name = dataGrid_barcodes.Columns[1].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;
        //            count = dataGrid_barcodes.Columns[3].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;
        //            barcode = dataGrid_barcodes.Columns[4].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;

        //        }
        //        else if (checkBox_individual.IsChecked == true)
        //        {
        //            mode = 1;
        //        }
        //        else
        //        {
        //            mode = 2;
        //        }

        //        if (mode == 1 || mode == 2)
        //        {
        //            art = dataGrid_barcodes.Columns[0].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;
        //            name = dataGrid_barcodes.Columns[1].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;
        //            count.Text = "1";
        //            barcode = dataGrid_barcodes.Columns[2].GetCellContent(dataGrid_barcodes.SelectedItem) as System.Windows.Controls.TextBlock;
        //        }

        //        tb_findBarcode.Text = art.Text;
        //        BarCodeFromTbl = barcode.Text;

        //        NiceLabel.SDK.MainWindowViewModel.BarcodeChoose(name.Text, art.Text, barcode.Text, System.Convert.ToInt32(count.Text), mode, (NiceLabel.SDK.MainWindowViewModel)DataContext);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void tb_findBarcode_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        { 
        //    UpdateTable();
        }

        private void cb_artTolow_Click(object sender, RoutedEventArgs e)
        {
        //    int mode = 0;

        //    if(cb_artTolow.IsChecked==true)
        //    {
        //        artToLowFlag = true;
        //    }
        //    else
        //    {
        //        artToLowFlag = false;
        //    }

        //    if (checkBox_trans.IsChecked==true)
        //    {
        //        mode = 0;
        //    }
        //    if (checkBox_individual.IsChecked == true)
        //    {
        //        mode = 1;
        //    }
        //    if (checkBox_individualWY.IsChecked == true)
        //    {
        //        mode = 2;
        //    }
        //    UpdateLabelView(mode, tb_findBarcode.Text);
        }
    }
}
