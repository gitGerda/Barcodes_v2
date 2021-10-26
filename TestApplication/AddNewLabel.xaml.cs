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
using System.Data;
using System.Data.SqlClient;
using NiceLabel.SDK.DemoApp;
using System.IO;



namespace NiceLabel.SDK
{
    /// <summary>
    /// Логика взаимодействия для AddNewLabel.xaml
    /// </summary>
    public partial class AddNewLabel : Window
    {
        /// <summary>
        /// SQL команда
        /// </summary>
        SqlCommand command;
        /// <summary>
        /// Режим добавления шаблона: 1 - добавление автозаполняемого шаблона, 2 - добавление шаблона ручного ввода
        /// </summary>
        int mode;
        /// <summary>
        /// Флаг принимает значение true при открытие формы по средством нажатия кнопки "Изменить" формы WindowSettings
        /// </summary>
        bool ChangeFlag;
        /// <summary>
        /// Переменная для хранения ID изменяемой записи
        /// </summary>
        string ID;

        /// <summary>
        /// Таблица сопоставлений
        /// </summary>
        DataTable mappingTable;
        /// <summary>
        /// Таблица переменных шаблона
        /// </summary>
        DataTable modelVariables;

        /// <summary>
        /// MainWindowViewModel object
        /// </summary>
        MainWindowViewModel MVM;
        /// <summary>
        /// Адаптер для считывания данных из БД
        /// </summary>
        SqlDataAdapter adapter;
        /// <summary>
        /// Переменная для хранения названия шаблона
        /// </summary>
        string CurrentModelName;
        /// <summary>
        /// Переменная для хранения текущего пути к шаблону
        /// </summary>
        string CurrentPathToModel;

        public AddNewLabel(int mode,bool changeFlag, string ID, MainWindowViewModel x)
        {
            InitializeComponent();

            this.MVM = x;
            this.mode = mode;
            this.ChangeFlag = changeFlag;
            this.ID = ID;

            mappingTable = new DataTable();
            DataColumn column1 = new DataColumn();
            column1.ColumnName = "Параметр";
            column1.ReadOnly = true;
            DataColumn column2 = new DataColumn();
            column2.ColumnName = "Переменная";
            column2.ReadOnly = true;
            mappingTable.Columns.Add(column1);
            mappingTable.Columns.Add(column2);

            modelVariables = new DataTable();
            DataColumn column3 = new DataColumn();
            column3.ColumnName = "Переменная";
            column3.ReadOnly = true;
            modelVariables.Columns.Add(column3);

            if (mode == 1)//режим автозаполняемого шаблона
            {
                rb_1CFillMode.IsChecked = true;
                rb_1CFillMode.IsEnabled = false;

                rb_ManualFillMode.IsChecked = false;
                rb_ManualFillMode.IsEnabled = false;

                rb_Internal.IsChecked = true;

                //построение таблицы сопоставлений
                mappingTable.Rows.Add("Наименование", "");
                mappingTable.Rows.Add("Артикул", "");
                mappingTable.Rows.Add("Штрихкод", "");
                mappingTable.Rows.Add("Внутренний код", "");
                mappingTable.Rows.Add("Количество", "");
                mappingTable.Rows.Add("Стандарт", "");
                mappingTable.Rows.Add("Экспортер", "");
                mappingTable.Rows.Add("Импортер", "");

                dg_mapping.ItemsSource = mappingTable.DefaultView;
            }
            else//режим ручного шаблона
            {
                rb_1CFillMode.IsChecked = false;
                rb_1CFillMode.IsEnabled = false;

                rb_ManualFillMode.IsChecked = true;
                rb_ManualFillMode.IsEnabled = false;

                rb_Internal.IsChecked = false;
                rb_Internal.IsEnabled = false;

                rb_External.IsChecked = false;
                rb_External.IsEnabled = false;

                //построение таблицы сопоставлений
                mappingTable.Rows.Add("A", "");
                mappingTable.Rows.Add("B", "");
                mappingTable.Rows.Add("C", "");
                mappingTable.Rows.Add("D", "");
                mappingTable.Rows.Add("E", "");
                mappingTable.Rows.Add("F", "");
                mappingTable.Rows.Add("G", "");
                mappingTable.Rows.Add("H", "");
                mappingTable.Rows.Add("I", "");
                mappingTable.Rows.Add("J", "");

                dg_mapping.ItemsSource = mappingTable.DefaultView;
            }

            dg_mapping.IsEnabled = false;
            dg_ModelVariables.IsEnabled = false;

            if(changeFlag)
            {
                FillFormWhenChangeMode();
            }
        }
        /// <summary>
        /// Функция заполнения формы в режиме изменения данных
        /// </summary>
        private void FillFormWhenChangeMode()
        {
            this.Title = "BarcodesApp # Настройки - Изменение шаблона";

            bool ContinueFlag = true;

            if (NiceLabel.SDK.DemoApp.MainWindow.connection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    MainWindow.connection.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    ContinueFlag = false;
                }
            }

            if (ContinueFlag)
            {
                if (mode == 1)
                {
                    string sql = "select PathToModel,ModelName,ProductName_desc,Art_desc,BarcodeMain_desc,BarcodeLocal_desc," +
                                 "Count_desc,StandardizationDoc_desc,Exporter_desc,Importer_desc,ExternalMarketFlag " +
                                 "from MappingModels " +
                                 "where ID="+ID;

                    command = new SqlCommand(sql, MainWindow.connection);
                    adapter = new SqlDataAdapter(command);
                    DataTable c = new DataTable();
                    adapter.Fill(c);

                    CurrentPathToModel = c.Rows[0]["PathToModel"].ToString();
                    tb_PathToModel.Text = c.Rows[0]["PathToModel"].ToString();
                    tb_ModelName.Text = c.Rows[0]["ModelName"].ToString();
                    CurrentModelName = tb_ModelName.Text;
                    

                    rb_1CFillMode.IsChecked = true;
                    rb_ManualFillMode.IsChecked = false;

                    rb_1CFillMode.IsEnabled = false;
                    rb_ManualFillMode.IsEnabled = false;

                    rb_Internal.IsEnabled = true;
                    rb_External.IsEnabled = true;
                    if (c.Rows[0]["ExternalMarketFlag"].ToString()=="False")
                    {
                        rb_Internal.IsChecked = true;
                        rb_External.IsChecked = false;
                    }
                    else
                    {
                        rb_Internal.IsChecked = false;
                        rb_External.IsChecked = true;
                    }

                    mappingTable.Columns["Переменная"].ReadOnly = false;

                    mappingTable.Rows[0]["Переменная"] = c.Rows[0]["ProductName_desc"].ToString();
                    mappingTable.Rows[1]["Переменная"] = c.Rows[0]["Art_desc"].ToString();
                    mappingTable.Rows[2]["Переменная"] = c.Rows[0]["BarcodeMain_desc"].ToString();
                    mappingTable.Rows[3]["Переменная"] = c.Rows[0]["BarcodeLocal_desc"].ToString();
                    mappingTable.Rows[4]["Переменная"] = c.Rows[0]["Count_desc"].ToString();
                    mappingTable.Rows[5]["Переменная"] = c.Rows[0]["StandardizationDoc_desc"].ToString();
                    mappingTable.Rows[6]["Переменная"] = c.Rows[0]["Exporter_desc"].ToString();
                    mappingTable.Rows[7]["Переменная"] = c.Rows[0]["Importer_desc"].ToString();

                    mappingTable.Columns["Переменная"].ReadOnly = true;

                    //необходимо проверить все ли переменные шаблона сопоставлены с полями таблицы. если не все, то не сопоставленные переменные вывести в DataGrid Переменные Шаблона

                    string ErrorString = "";
                    List<string> Variables = MVM.GetLabelVariables(tb_PathToModel.Text, out ErrorString);
                    List<string> VariablesFiltr = new List<string>();

                    if (ErrorString == "")
                    {
                        foreach(string f in Variables)
                        {
                            object z = (from t in mappingTable.AsEnumerable()
                                        where t["Переменная"].ToString() == f
                                        select t).FirstOrDefault();
                            
                            if(z==null)
                            {
                                VariablesFiltr.Add(f);
                            }
                        }

                        modelVariables.Columns["Переменная"].ReadOnly = false;
                        foreach(string f in VariablesFiltr)
                        {
                            modelVariables.Rows.Add(f);
                        }
                        modelVariables.Columns["Переменная"].ReadOnly = true;
                        dg_ModelVariables.ItemsSource = modelVariables.DefaultView;

                        dg_mapping.IsEnabled = true;
                        dg_ModelVariables.IsEnabled = true;
                    }
                    else
                    {
                        if (ErrorString == "LabelError")
                        {
                            MessageBox.Show("Не удалось загрузить переменные шаблона. Ошибка проектирования шаблона!");
                        }
                        else if (ErrorString == "FileAccessError")
                        {
                            MessageBox.Show("Не удалось загрузить переменные шаблона. Ошибка доступа к шаблону!");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось загрузить переменные шаблона. Ошибка открытия шаблона!");
                        };
                    }
                }
                else
                {
                    string sql = "select ModelPath,ModelName,A_desc,B_desc,C_desc,D_desc,E_desc,F_desc,G_desc,H_desc, " +
                                 "I_desc,J_desc " +
                                 "from  ManualLabelsDesc " +
                                 "where ManualLabelsDescID=" + ID;

                    command = new SqlCommand(sql, MainWindow.connection);
                    adapter = new SqlDataAdapter(command);
                    DataTable c = new DataTable();
                    adapter.Fill(c);

                    CurrentPathToModel = c.Rows[0]["ModelPath"].ToString();
                    tb_PathToModel.Text = c.Rows[0]["ModelPath"].ToString();
                    tb_ModelName.Text = c.Rows[0]["ModelName"].ToString();
                    CurrentModelName = tb_ModelName.Text;


                    rb_1CFillMode.IsChecked = false;
                    rb_ManualFillMode.IsChecked = true;

                    rb_1CFillMode.IsEnabled = false;
                    rb_ManualFillMode.IsEnabled = false;

                    rb_Internal.IsChecked = false;
                    rb_External.IsChecked = false;
                    rb_Internal.IsEnabled = false;
                    rb_External.IsEnabled = false;


                    mappingTable.Columns["Переменная"].ReadOnly = false;

                    mappingTable.Rows[0]["Переменная"] = c.Rows[0]["A_desc"].ToString();
                    mappingTable.Rows[1]["Переменная"] = c.Rows[0]["B_desc"].ToString();
                    mappingTable.Rows[2]["Переменная"] = c.Rows[0]["C_desc"].ToString();
                    mappingTable.Rows[3]["Переменная"] = c.Rows[0]["D_desc"].ToString();
                    mappingTable.Rows[4]["Переменная"] = c.Rows[0]["E_desc"].ToString();
                    mappingTable.Rows[5]["Переменная"] = c.Rows[0]["F_desc"].ToString();
                    mappingTable.Rows[6]["Переменная"] = c.Rows[0]["G_desc"].ToString();
                    mappingTable.Rows[7]["Переменная"] = c.Rows[0]["H_desc"].ToString();
                    mappingTable.Rows[8]["Переменная"] = c.Rows[0]["I_desc"].ToString();
                    mappingTable.Rows[9]["Переменная"] = c.Rows[0]["J_desc"].ToString();

                    mappingTable.Columns["Переменная"].ReadOnly = true;

                    //необходимо проверить все ли переменные шаблона сопоставлены с полями таблицы. если не все, то не сопоставленные переменные вывести в DataGrid Переменные Шаблона

                    string ErrorString = "";
                    List<string> Variables = MVM.GetLabelVariables(tb_PathToModel.Text, out ErrorString);
                    List<string> VariablesFiltr = new List<string>();

                    if (ErrorString == "")
                    {
                        foreach (string f in Variables)
                        {
                            object z = (from t in mappingTable.AsEnumerable()
                                        where t["Переменная"].ToString() == f
                                        select t).FirstOrDefault();

                            if (z == null)
                            {
                                VariablesFiltr.Add(f);
                            }
                        }

                        modelVariables.Columns["Переменная"].ReadOnly = false;
                        foreach (string f in VariablesFiltr)
                        {
                            modelVariables.Rows.Add(f);
                        }
                        modelVariables.Columns["Переменная"].ReadOnly = true;
                        dg_ModelVariables.ItemsSource = modelVariables.DefaultView;

                        dg_mapping.IsEnabled = true;
                        dg_ModelVariables.IsEnabled = true;
                    }
                    else
                    {
                        if (ErrorString == "LabelError")
                        {
                            MessageBox.Show("Не удалось загрузить переменные шаблона. Ошибка проектирования шаблона!");
                        }
                        else if (ErrorString == "FileAccessError")
                        {
                            MessageBox.Show("Не удалось загрузить переменные шаблона. Ошибка доступа к шаблону!");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось загрузить переменные шаблона. Ошибка открытия шаблона!");
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Функция открытия проводника для выбора шаблона
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog OPF = new System.Windows.Forms.OpenFileDialog();
            if (OPF.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_PathToModel.Text = OPF.FileName;
            }
        }

        /// <summary>
        /// Функция реализующая событие заполнения строки адреса шаблона.
        /// В этой функции необходимо получить переменные шаблона и заполнить ими
        /// DataGrid переменных шаблона.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_PathToModel_TextChanged(object sender, TextChangedEventArgs e)
        {
                bool NullVariableFlag = false;
                bool ContinueFlag = true;
                bool ContinueFlag2 = true;

                if (NiceLabel.SDK.DemoApp.MainWindow.connection.State != System.Data.ConnectionState.Open)
                {
                    try
                    {
                        MainWindow.connection.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        ContinueFlag = false;
                    }
                }

                if (ContinueFlag)
                {
                    string sql = "";

                    if (mode == 1)
                    {
                        sql = "select * from MappingModels where PathToModel like '" + tb_PathToModel.Text + "';";
                    }
                    else
                    {
                        sql = "select * from ManualLabelsDesc where ModelPath like '" + tb_PathToModel.Text + "';";
                    }

                    command = new SqlCommand(sql, MainWindow.connection);
                    object z = command.ExecuteScalar();

                    if (z != null)
                    {
                        ContinueFlag2 = false;
                        MessageBox.Show("Данный шаблон уже добавлен в базу данных!", "Предупреждение");
                    }
                    else
                    {
                        CurrentPathToModel = tb_PathToModel.Text;
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось подключиться к базе данных!", "Предупреждение");
                    ContinueFlag2 = false;
                }

                if (ContinueFlag2)
                {
                    modelVariables.Clear();

                    string ErrorString = "";
                    List<string> Variables = MVM.GetLabelVariables(tb_PathToModel.Text, out ErrorString);

                    if (ErrorString == "")
                    {
                        foreach (string variable in Variables)
                        {
                            if (!String.IsNullOrEmpty(variable))
                            {
                                modelVariables.Rows.Add(variable);
                            }
                            else { NullVariableFlag = true; }
                        }

                        dg_ModelVariables.ItemsSource = modelVariables.DefaultView;

                        mappingTable.Columns[1].ReadOnly = false;
                        foreach(System.Data.DataRow f in mappingTable.AsEnumerable())
                        {
                            f["Переменная"] = "";
                        }
                        mappingTable.Columns[1].ReadOnly = true;

                        dg_mapping.IsEnabled = true;
                        dg_ModelVariables.IsEnabled = true;
                    }
                    else
                    {
                        if (ErrorString == "LabelError")
                        {
                            MessageBox.Show("Ошибка проектирования шаблона!");
                        }
                        else if (ErrorString == "FileAccessError")
                        {
                            MessageBox.Show("Ошибка доступа к шаблону!");
                        }
                        else
                        {
                            MessageBox.Show("Ошибка открытия шаблона!");
                        };
                    }
                    dg_mapping.SelectedIndex = 0;

                    if (NullVariableFlag)
                    {
                        MessageBox.Show("Обнаружены переменные с пустым именем (данные переменные были исключены из выборки)", "Предупреждение");
                    }
                }
                else
                {
                     if (!ChangeFlag)
                     {
                         tb_PathToModel.Text = "";
                     }
                     else
                     {
                         tb_PathToModel.Text = CurrentPathToModel;
                     }
                }
        }

        /// <summary>
        /// Функция выбора переменной шаблона и перемещения её в таблицу сопоставления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_ModelVariables_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int ind = dg_ModelVariables.SelectedIndex;

            if (ind != -1)
            {
                if (ind <= (modelVariables.Rows.Count - 1))
                {
                    if (modelVariables.Rows.Count != 0)
                    {
                        string variable = modelVariables.Rows[ind]["Переменная"].ToString();
                        int indexMapping = dg_mapping.SelectedIndex;

                        if (indexMapping != -1 )
                        {
                            if (indexMapping <= (mappingTable.Rows.Count - 1))
                            {
                                if (mappingTable.Rows[indexMapping]["Переменная"].ToString() == "")
                                {
                                    mappingTable.Columns[1].ReadOnly = false;
                                    mappingTable.Rows[indexMapping]["Переменная"] = variable;
                                    mappingTable.Columns[1].ReadOnly = true;

                                    dg_mapping.ItemsSource = mappingTable.DefaultView;

                                    modelVariables.Rows.Remove(modelVariables.Rows[ind]);
                                    dg_ModelVariables.ItemsSource = modelVariables.DefaultView;
                                }
                                else
                                {
                                    string v = mappingTable.Rows[indexMapping]["Переменная"].ToString();

                                    mappingTable.Columns[1].ReadOnly = false;
                                    mappingTable.Rows[indexMapping]["Переменная"] = variable;
                                    mappingTable.Columns[1].ReadOnly = true;

                                    dg_mapping.ItemsSource = mappingTable.DefaultView;

                                    modelVariables.Rows.Remove(modelVariables.Rows[ind]);

                                    modelVariables.Rows.Add(v);
                                    dg_ModelVariables.ItemsSource = modelVariables.DefaultView;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Не выбрана строка в таблице сопоставления!", "Предупреждение");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Вы сопоставили все переменные шаблона!", "Предупреждение");
                    }
                }
            }
            dg_mapping.SelectedIndex += 1;

        }

        /// <summary>
        /// Функция перемещения переменной из таблицы сопоставления в таблицу переменных шаблона
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_mapping_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int ind = dg_mapping.SelectedIndex;

            if(ind!=-1)
            {
                if(ind<=(mappingTable.Rows.Count-1))
                {
                    string variable = mappingTable.Rows[ind]["Переменная"].ToString();
                    if (!String.IsNullOrEmpty(variable))
                    {
                        mappingTable.Columns[1].ReadOnly = false;
                        mappingTable.Rows[ind]["Переменная"] = "";
                        mappingTable.Columns[1].ReadOnly = true;

                        modelVariables.Rows.Add(variable);
                        dg_ModelVariables.ItemsSource = modelVariables.DefaultView;
                    }
                }
            }
        }
        /// <summary>
        /// Функция реализующая нажатие кнопки "Закрыть"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Функция сохранения изменений и закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CloseWithSave_Click(object sender, RoutedEventArgs e)
        {
            bool AutoFill1C_flag = false;
            bool InternalMarket_flag = false;
            bool ContinueFlag = true;


            if (tb_PathToModel.Text!="" && tb_ModelName.Text!="")
            {
                if (NiceLabel.SDK.DemoApp.MainWindow.connection.State != System.Data.ConnectionState.Open)
                {
                    try
                    {
                        MainWindow.connection.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        ContinueFlag = false;
                    }
                }

                if (ContinueFlag)
                {
                    if (rb_1CFillMode.IsChecked == true)
                    {
                        AutoFill1C_flag = true;
                    }

                    if (rb_Internal.IsChecked == true)
                    {
                        InternalMarket_flag = true;
                    }

                        if (AutoFill1C_flag)
                        {
                            if (InternalMarket_flag)
                            {
                                string sql="";
                                if (!ChangeFlag)
                                {
                                    sql = "insert into MappingModels(PathToModel,ModelName,ProductName_desc,Art_desc," +
                                                 "BarcodeMain_desc,BarcodeLocal_desc,Count_desc,StandardizationDoc_desc," +
                                                 "Exporter_desc,Importer_desc,ExternalMarketFlag) " +
                                                 "values('" + tb_PathToModel.Text + "','"
                                                 + tb_ModelName.Text + "','" + mappingTable.Rows[0]["Переменная"].ToString() +
                                                 "','" + mappingTable.Rows[1]["Переменная"].ToString() + "','" +
                                                 mappingTable.Rows[2]["Переменная"].ToString() + "','" +
                                                 mappingTable.Rows[3]["Переменная"].ToString() + "','" +
                                                 mappingTable.Rows[4]["Переменная"].ToString() + "','" +
                                                 mappingTable.Rows[5]["Переменная"].ToString() + "','" +
                                                 mappingTable.Rows[6]["Переменная"].ToString() + "','" +
                                                 mappingTable.Rows[7]["Переменная"].ToString() + "'," + "0);";
                                }
                                else
                                {
                                    sql = "update MappingModels " +
                                                 "set PathToModel='" + tb_PathToModel.Text + "',ModelName='"
                                                 + tb_ModelName.Text + "',ProductName_desc='" +
                                                 mappingTable.Rows[0]["Переменная"].ToString() +
                                                 "',Art_desc='" + mappingTable.Rows[1]["Переменная"].ToString() +
                                                 "',BarcodeMain_desc='" + mappingTable.Rows[2]["Переменная"].ToString() +
                                                 "',BarcodeLocal_desc='" + mappingTable.Rows[3]["Переменная"].ToString() +
                                                 "',Count_desc='" + mappingTable.Rows[4]["Переменная"].ToString() +
                                                 "',StandardizationDoc_desc='" + mappingTable.Rows[5]["Переменная"].ToString() +
                                                 "',Exporter_desc='" + mappingTable.Rows[6]["Переменная"].ToString() +
                                                 "',Importer_desc='" + mappingTable.Rows[7]["Переменная"].ToString() +
                                                 "',ExternalMarketFlag=0 " +
                                                 "where ID=" + ID + ";";
                                }

                                try
                                {
                                    command = new SqlCommand(sql, MainWindow.connection);
                                    command.ExecuteNonQuery();
                                    MessageBox.Show("Успех!", "Уведомление");
                                }
                                catch (Exception ex)
                                {
                                    if (ex.Message.Contains("Не удается вставить повторяющийся ключ"))
                                    {
                                        MessageBox.Show("Данный шаблон уже добавлен в базу данных!","Предупреждение");
                                    }
                                    else 
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }

                            }
                            else
                            {
                                string sql = "";
                                if (!ChangeFlag)
                                {
                                    sql = "insert into MappingModels(PathToModel,ModelName,ProductName_desc,Art_desc," +
                                                  "BarcodeMain_desc,BarcodeLocal_desc,Count_desc,StandardizationDoc_desc," +
                                                  "Exporter_desc,Importer_desc,ExternalMarketFlag) " +
                                                  "values('" + tb_PathToModel.Text + "','"
                                                  + tb_ModelName.Text + "','" + mappingTable.Rows[0]["Переменная"].ToString() +
                                                  "','" + mappingTable.Rows[1]["Переменная"].ToString() + "','" +
                                                  mappingTable.Rows[2]["Переменная"].ToString() + "','" +
                                                  mappingTable.Rows[3]["Переменная"].ToString() + "','" +
                                                  mappingTable.Rows[4]["Переменная"].ToString() + "','" +
                                                  mappingTable.Rows[5]["Переменная"].ToString() + "','" +
                                                  mappingTable.Rows[6]["Переменная"].ToString() + "','" +
                                                  mappingTable.Rows[7]["Переменная"].ToString() + "'," + "1);";
                                }
                                else
                                {
                                    sql = "update MappingModels " +
                                                 "set PathToModel='" + tb_PathToModel.Text + "',ModelName='"
                                                 + tb_ModelName.Text + "',ProductName_desc='" + 
                                                 mappingTable.Rows[0]["Переменная"].ToString() +
                                                 "',Art_desc='" + mappingTable.Rows[1]["Переменная"].ToString() +
                                                 "',BarcodeMain_desc='" + mappingTable.Rows[2]["Переменная"].ToString() +
                                                 "',BarcodeLocal_desc='" + mappingTable.Rows[3]["Переменная"].ToString() +
                                                 "',Count_desc='" +mappingTable.Rows[4]["Переменная"].ToString() +
                                                 "',StandardizationDoc_desc='" + mappingTable.Rows[5]["Переменная"].ToString() +
                                                 "',Exporter_desc='" +mappingTable.Rows[6]["Переменная"].ToString() +
                                                 "',Importer_desc='" +mappingTable.Rows[7]["Переменная"].ToString() +
                                                 "',ExternalMarketFlag=1 " +
                                                 "where ID=" + ID + ";";
                                }

                                try
                                {
                                    command = new SqlCommand(sql, MainWindow.connection);
                                    command.ExecuteNonQuery();
                                    MessageBox.Show("Успех!", "Уведомление");
                                }
                                catch (Exception ex)
                                {
                                    if (ex.Message.Contains("Не удается вставить повторяющийся ключ"))
                                    {
                                        MessageBox.Show("Данный шаблон уже добавлен в базу данных!", "Предупреждение");
                                    }
                                    else
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                            }
                            
                        }
                        else
                        {
                            string sql = "";
                            if (!ChangeFlag)
                            {
                                sql = "insert into ManualLabelsDesc(ModelPath,ModelName,A_desc,B_desc,C_desc,D_desc,E_desc," +
                                             "F_desc,G_desc,H_desc,I_desc,J_desc) " +
                                             "values('" + tb_PathToModel.Text + "','"
                                             + tb_ModelName.Text + "','" + mappingTable.Rows[0]["Переменная"].ToString() +
                                             "','" + mappingTable.Rows[1]["Переменная"].ToString() + "','" +
                                             mappingTable.Rows[2]["Переменная"].ToString() + "','" +
                                             mappingTable.Rows[3]["Переменная"].ToString() + "','" +
                                             mappingTable.Rows[4]["Переменная"].ToString() + "','" +
                                             mappingTable.Rows[5]["Переменная"].ToString() + "','" +
                                             mappingTable.Rows[6]["Переменная"].ToString() + "','" +
                                             mappingTable.Rows[7]["Переменная"].ToString() + "','" +
                                             mappingTable.Rows[8]["Переменная"].ToString() + "','" +
                                             mappingTable.Rows[9]["Переменная"].ToString() + "');";
                            }
                            else
                            {
                                sql = "update ManualLabelsDesc " +
                                             "set ModelPath='" + tb_PathToModel.Text + 
                                             "',ModelName='"+ tb_ModelName.Text + 
                                             "',A_desc='" + mappingTable.Rows[0]["Переменная"].ToString() +
                                             "',B_desc='" + mappingTable.Rows[1]["Переменная"].ToString() +
                                             "',C_desc='" +mappingTable.Rows[2]["Переменная"].ToString() +
                                             "',D_desc='" +mappingTable.Rows[3]["Переменная"].ToString() +
                                             "',E_desc='" +mappingTable.Rows[4]["Переменная"].ToString() +
                                             "',F_desc='" +mappingTable.Rows[5]["Переменная"].ToString() +
                                             "',G_desc='" +mappingTable.Rows[6]["Переменная"].ToString() +
                                             "',H_desc='" +mappingTable.Rows[7]["Переменная"].ToString() +
                                             "',I_desc='" +mappingTable.Rows[8]["Переменная"].ToString() +
                                             "',J_desc='" +mappingTable.Rows[9]["Переменная"].ToString() + "' " +
                                             "where ManualLabelsDescID = "+ID+";";
                            }    

                            try
                            {
                                command = new SqlCommand(sql, MainWindow.connection);
                                command.ExecuteNonQuery();
                                MessageBox.Show("Успех!", "Уведомление");
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("Не удается вставить повторяющийся ключ"))
                                {
                                    MessageBox.Show("Данный шаблон уже добавлен в базу данных!", "Предупреждение");
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                        
                    }
                        
                    
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Обнаружены незаполненные поля!", "Предупреждение");
            }
        }
        /// <summary>
        /// Функция исключения повторяющихся имен шаблонов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_ModelName_LostFocus(object sender, RoutedEventArgs e)
        {
            bool ContinueFlag = true;

            if (NiceLabel.SDK.DemoApp.MainWindow.connection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    MainWindow.connection.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    ContinueFlag = false;
                }
            }

            if (ContinueFlag)
            {
                string sql="";

                if(mode==1)
                {
                    sql = "select * from MappingModels where ModelName like '" + tb_ModelName.Text + "';";
                }
                else 
                { 
                    sql = "select * from ManualLabelsDesc where ModelName like '"+ tb_ModelName.Text + "';";
                }

                command = new SqlCommand(sql, MainWindow.connection);
                object f = command.ExecuteScalar();

                if (f != null)
                {
                    if (!ChangeFlag)
                    {
                        tb_ModelName.Text = "";
                    }
                    else
                    {
                        tb_ModelName.Text = CurrentModelName;
                    }
                    MessageBox.Show("Введенное имя занято. Придумайте другое имя для шаблона.", "Предупреждение");
                }

                CurrentModelName = tb_ModelName.Text;
            }
            else
            {
                if (!ChangeFlag)
                {
                    tb_ModelName.Text = "";
                }
                else
                {
                    tb_ModelName.Text = CurrentModelName;
                }
                MessageBox.Show("Не удается подключиться к базе данных!", "Предупреждение");
            }
        }
    }
}
