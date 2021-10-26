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

namespace NiceLabel.SDK
{
    /// <summary>
    /// Логика взаимодействия для DeleteQuestion.xaml
    /// </summary>
    public partial class DeleteQuestion : Window
    {
        public DeleteQuestion(string q)
        {
            InitializeComponent();

            tb_quest.Text = "Вы действительно хотите удалить шаблон: " + q + " ?";
        }

        private void btn_yes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
