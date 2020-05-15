using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for PrintWindow.xaml
    /// </summary>
    public partial class PrintWindow : Window
    {
        public PrintWindow()
        {
            InitializeComponent();
            borderGranicaLista.Margin = new Thickness(0);

        }
        private void BtnStampaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int Margin = 24;              
                this.IsEnabled = false;
                btnOdustani.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();            
                borderGranicaLista.Margin = new Thickness(72);
                if (printDialog.ShowDialog() == true)
                {
                    Size pageSize = new Size(printDialog.PrintableAreaWidth - Margin, printDialog.PrintableAreaHeight - Margin);
                    okvirZaStampu.Measure(pageSize);
                    okvirZaStampu.Arrange(new Rect(Margin, Margin, pageSize.Width, pageSize.Height));
                    printDialog.PrintVisual(okvirZaStampu, "Faktura");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.IsEnabled = true;
                btnOdustani.IsEnabled = true;
            }


        }

        private void BtnOdustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}
