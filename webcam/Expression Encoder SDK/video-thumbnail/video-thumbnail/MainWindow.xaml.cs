using Microsoft.Expression.Encoder;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace video_thumbnail
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            var path = @"C:\Users\ManojKumar\Documents\GitHub\PatientRecordsWindows\PatientRecordsWPF2\bin\Debug\Media\2013Jan31-195929-Thu.wmv";

            var video = new MediaItem(path);
            using (var bitmap = video.MainMediaFile.GetThumbnail(
                new TimeSpan(0, 0, 1),
                new System.Drawing.Size(30, 30)))
            {
                // do something with the bitmap like:
                var bpath = path + ".thumbnail.jpg";
                bitmap.Save(bpath);
                img.Source = new BitmapImage(new Uri(bpath));
            }
        }
    }
}
