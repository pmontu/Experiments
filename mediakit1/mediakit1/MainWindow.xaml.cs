﻿using System;
using System.Collections.Generic;
using System.IO;
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

namespace mediakit1
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap(320, 240, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(videoCapElement);
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            Directory.CreateDirectory("C:\\Photos\\");
            string filename = "C:\\Photos\\" + "test.jpg";
            FileStream fstream = new FileStream(filename, FileMode.Create);
            encoder.Save(fstream);
            fstream.Close();

        }

        private void videoCapDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbx = ((ComboBox)sender);
            if (cbx.SelectedIndex != -1)
            {
                videoCapElement.VideoCaptureSource = ((WPFMediaKit.DirectShow.Interop.DsDevice)cbx.SelectedItem).Name;
                videoCapElement.Play();
            }
        }

        private void Window_Activated_1(object sender, EventArgs e)
        {
            videoCapDevices.SelectedIndex = 0;
        }
    }
}
