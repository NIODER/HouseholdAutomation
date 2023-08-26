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

namespace HouseholdAutomationDesktop.View.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для SelectResourceForOrderWindow.xaml
    /// </summary>
    public partial class SelectResourceWindow : Window
    {
        public SelectResourceWindow()
        {
            InitializeComponent();
        }

        private void OnChoseResourceButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
