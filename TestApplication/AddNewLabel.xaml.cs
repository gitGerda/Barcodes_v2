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

namespace NiceLabel.SDK
{
    /// <summary>
    /// Логика взаимодействия для AddNewLabel.xaml
    /// </summary>
    public partial class AddNewLabel : Window
    {
        /// <summary>
        /// Режим добавления шаблона: 1 - добавление автозаполняемого шаблона, 2 - добавление шаблона ручного ввода
        /// </summary>
        int mode;

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

        public AddNewLabel(int mode,MainWindowViewModel x)
        {
            InitializeComponent();

            this.MVM = x;
            this.mode = mode;

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
                mappingTable.Rows.Add("", "");
                mappingTable.Rows.Add("", "");

                dg_mapping.ItemsSource = mappingTable.DefaultView;
            }

            dg_mapping.IsEnabled = false;
            dg_ModelVariables.IsEnabled = false;
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

            modelVariables.Clear();

            mappingTable.Columns[1].ReadOnly = false;
            foreach (DataRow f in mappingTable.Rows)
            {
                f["Переменная"] = "";
                
            }
            mappingTable.Columns[1].ReadOnly = true;
            dg_mapping.ItemsSource = mappingTable.DefaultView;

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

                dg_mapping.IsEnabled = true;
                dg_ModelVariables.IsEnabled = true;
            }
            else
            {
                if(ErrorString== "LabelError")
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

            if(NullVariableFlag)
            {
                MessageBox.Show("Обнаружены переменные с пустым именем (данные переменные были исключены из выборки)", "Предупреждение");
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

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_CloseWithSave_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
