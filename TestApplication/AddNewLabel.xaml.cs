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
    /// Логика взаимодействия для AddNewLabel.xaml
    /// </summary>
    public partial class AddNewLabel : Window
    {
        public AddNewLabel()
        {
            InitializeComponent();
        }

      
        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog OPF = new System.Windows.Forms.OpenFileDialog();
            if (OPF.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_PathToModel.Text = OPF.FileName;
            }
        }
    }
}
