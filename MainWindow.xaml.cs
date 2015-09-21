using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sha1hash
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            hashDat.Focus();

            // when F1 is pressed, show help window
            CommandBinding helpBinding = new CommandBinding(ApplicationCommands.Help);
            helpBinding.Executed += f1pressed;
            CommandBindings.Add(helpBinding);
        }

        private void f1pressed(object sender, ExecutedRoutedEventArgs e)
        {
            HelpWindow hlp = new HelpWindow();
            hlp.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            log.Clear();

            string dirName = dir.Text.Trim();
            if (string.IsNullOrEmpty(dirName))
            {
                dirName = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            if (!Directory.Exists(dirName))
            {
                MessageBox.Show(
                    "По указанному пути нет папки с таким именем.",
                    "Не найден каталог",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error                    
                    );
                return;
            }
            
            try
            {
                List<string> files = Directory.GetFiles(
                    dirName,
                    "*.*",
                    SearchOption.TopDirectoryOnly
                    ).Where(f => f.ToLower().EndsWith("png") || f.ToLower().EndsWith("jpg") || f.ToLower().EndsWith("gif")).ToList<string>();
                foreach (string file in files)
                {
                    using (FileStream stream = File.OpenRead(file))
                    {
                        SHA1Managed sha = new SHA1Managed();
                        byte[] checksum = sha.ComputeHash(stream);
                        //log.Text += System.IO.Path.GetFileName(file) + " - " + BitConverter.ToString(checksum).Replace("-", string.Empty) + Environment.NewLine;
                        log.Text += BitConverter.ToString(checksum).Replace("-", string.Empty) + Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось получить SHA1 для файлов по указанному пути. " + ex.Message,
                    "Ошибка вычисления SHA1",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error                    
                    );
            }
        }
    }
}
