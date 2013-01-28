using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Microsoft.Expression.Encoder.Devices;
using Microsoft.Expression.Encoder.Live;
using Microsoft.Expression.Encoder;
using WebcamLibrary;
using Point = System.Drawing.Point;

namespace MyWebcam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        // Preset for the quality streaming
        private Preset _quality1, _quality2, _quality3;
        // Object of the library for manage the webcam
        private ControlWebcam _controlWebcam;

        public MainWindow()
        {
            InitializeComponent();
            windowsFormsHost1.Visibility = Visibility.Hidden;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // Create a new instance of the library
            _controlWebcam = new ControlWebcam();
            // Get the list of audio and video devices
            ListAllAudioDevice();
            ListAllVideoDevice();
            // Define the quality presets
            _quality1 = LivePresets.VC1512kDSL4x3;
            _quality2 = LivePresets.VC1256kDSL4x3;
            _quality3 = LivePresets.VC1HighSpeedBroadband4x3;
            // ------- Button visibility ----------
            buttonSettingsVideo.Visibility = Visibility.Hidden;
            buttonSettingsAudio.Visibility = Visibility.Hidden; 
            buttonStop.Visibility = Visibility.Hidden;
            groupBoxStreaming.Visibility = Visibility.Hidden;
            groupBoxRecording.Visibility = Visibility.Hidden;
            buttonTakeSnap.Visibility = Visibility.Hidden;
            
        }

        private void ComboBoxVideoSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxVideo.SelectedIndex != -1)
            {
                // Set the selected video device
                _controlWebcam.SelectedVideoDevice = (EncoderDevice) comboBoxVideo.SelectedItem;
            }
        }

        private void ComboBoxAudioSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxAudio.SelectedIndex != -1)
            {
                // Set the selected audio device
                _controlWebcam.SelectedAudioDevice = (EncoderDevice)comboBoxAudio.SelectedItem;
            }
        }

        private void ButtonRefreshVideoClick(object sender, RoutedEventArgs e)
        {
            // Reload the list of video devices
            ListAllVideoDevice();
        }

        private void ButtonRefreshAudioClick(object sender, RoutedEventArgs e)
        {
            // Reload the list of audio devices
            ListAllAudioDevice();
        }

        private void ButtonSettingsVideoClick(object sender, RoutedEventArgs e)
        {
            // Display the form for the video settings
            _controlWebcam.DisplayVideoSettings(new HandleRef(this, new WindowInteropHelper(this).Handle));
        }

        private void ButtonSettingsAudioClick(object sender, RoutedEventArgs e)
        {
            // Display the form for the audio settings
            _controlWebcam.DisplayAudioSettings(new HandleRef(this, new WindowInteropHelper(this).Handle));
        }

        private void ButtonStartClick(object sender, RoutedEventArgs e)
        {
            // Activate the selected webcam
            _controlWebcam.StartWebcam();
            // Start the preview of the webcam into the previewVideoPanel
            _controlWebcam.LiveDeviceSource.PreviewWindow = 
                new PreviewWindow(new HandleRef(panelPreview, panelPreview.Handle));
            // Fill the comboBox of streaming quality
            comboBoxStreamingQuality.Items.Add(_quality1);
            comboBoxStreamingQuality.Items.Add(_quality2);
            comboBoxStreamingQuality.Items.Add(_quality3);
            // Display the name of the preset into the comboBox
            comboBoxStreamingQuality.DisplayMemberPath = "Name";
            // ------- Button visibility ----------
            buttonSettingsVideo.Visibility = Visibility.Visible;
            buttonSettingsAudio.Visibility = Visibility.Visible;
            buttonStart.IsEnabled = false;
            buttonStop.Visibility = Visibility.Visible;
            groupBoxStreaming.Visibility = Visibility.Visible;
            groupBoxRecording.Visibility = Visibility.Visible;
            buttonStopRecording.Visibility = Visibility.Hidden;
            buttonStopStreaming.Visibility = Visibility.Hidden;
            buttonTakeSnap.Visibility=Visibility.Visible;
            windowsFormsHost1.Visibility= Visibility.Visible;
        }

        private void ButtonStopClick(object sender, RoutedEventArgs e)
        {
            // Stop the webcam
            _controlWebcam.StopWebcam();
            // ------- Button visibility ----------
            buttonSettingsVideo.Visibility = Visibility.Hidden;
            buttonSettingsAudio.Visibility = Visibility.Hidden;
            buttonStart.IsEnabled = true;
            buttonStop.Visibility = Visibility.Hidden;
            groupBoxStreaming.Visibility = Visibility.Hidden;
            groupBoxRecording.Visibility = Visibility.Hidden;
            buttonTakeSnap.Visibility=Visibility.Hidden;
            windowsFormsHost1.Visibility = Visibility.Hidden;
        }

        private void ListAllVideoDevice()
        {
            // Initialize the list who will content the video devices
            _controlWebcam.InitializeListVideoDevices();
            // Get all the video device and add them to the comboBox
            foreach (EncoderDevice videoDevice in _controlWebcam.ListVideoDevices)
            {
                comboBoxVideo.Items.Add(videoDevice);
            }
            // Display the name of the video device into the comboBox
            comboBoxVideo.DisplayMemberPath = "Name";
            // Select the first video device
            if (comboBoxVideo.Items.Count > 0)
            {
                comboBoxVideo.SelectedIndex = 0;
                _controlWebcam.SelectedVideoDevice = (EncoderDevice)comboBoxVideo.SelectedItem;
            }
        }

        private void ListAllAudioDevice()
        {
            // Initialize the list who will content the audio devices
            _controlWebcam.InitializeListAudioDevices();
            // Get all audio device and add them into the comboBox
            foreach (EncoderDevice audioDevice in _controlWebcam.ListAudioDevices)
            {
                comboBoxAudio.Items.Add(audioDevice);
            }
            // Display the name of the audio device into the comboBox
            comboBoxAudio.DisplayMemberPath = "Name";
            // Select the first audio device
            if (comboBoxAudio.Items.Count > 0)
            {
                comboBoxAudio.SelectedIndex = 0;
                _controlWebcam.SelectedAudioDevice = (EncoderDevice)comboBoxAudio.SelectedItem;
            }
        }

        private void ButtonStartStreamingClick(object sender, RoutedEventArgs e)
        {
            if (textBoxStreamingPort.Text != "")
            {
                // Start the streaming
                _controlWebcam.StartStreaming(Convert.ToInt32(textBoxStreamingPort.Text), 5, (Preset) comboBoxStreamingQuality.SelectedItem);
                buttonStartStreaming.IsEnabled = false;
                // Display the stop button
                buttonStopStreaming.Visibility = Visibility.Visible;
            }
        }

        private void ButtonStopStreamingClick(object sender, RoutedEventArgs e)
        {
            // Stop the streaming 
            _controlWebcam.StopStreaming();
            buttonStartStreaming.IsEnabled = true;
            // Hide the stop button
            buttonStopStreaming.Visibility = Visibility.Hidden;
        }

        private void ButtonStartRecordingClick(object sender, RoutedEventArgs e)
        {
            if (textBoxRecordingOutput.Text != "")
            {
                _controlWebcam.StartRecording(textBoxRecordingOutput.Text); 
                buttonStopRecording.Visibility = Visibility.Visible;
                buttonStartRecording.IsEnabled = false;
            }
        }

        private void ButtonStopRecordingClick(object sender, RoutedEventArgs e)
        {
            _controlWebcam.StopRecording();
            buttonStopRecording.Visibility = Visibility.Hidden;
            buttonStartRecording.IsEnabled = true;
        }

        private void ButtonSelectOutputPathClick(object sender, RoutedEventArgs e)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Video"; // Default file name
            dlg.DefaultExt = ".wmv"; // Default file extension
            dlg.Filter = "Video files | *.wmv"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Display path into textbox
                textBoxRecordingOutput.Text = dlg.FileName;
            }
        }

        private void ButtonTakeSnapClick(object sender, RoutedEventArgs e)
        {
            // Create the bitmap image
            using (Bitmap bitmap = new Bitmap(panelPreview.Width,panelPreview.Height))
            {
                // Create the graphics object that will copy from the screen into the bitmap image
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    // Create a rectangle egal to the size of the previewVideoPanel
                    Rectangle rectangleVideoPreview = panelPreview.Bounds;
                    // Create a point egal to the origin location (upper left point) of the previewVideoPanel
                    Point sourcesPoint =
                        panelPreview.PointToScreen(new Point(panelPreview.ClientRectangle.X,
                                                             panelPreview.ClientRectangle.Y));
                    // Copy the content of this rectangle into the bitmap image using the object graphics
                    graphics.CopyFromScreen(sourcesPoint,Point.Empty,rectangleVideoPreview.Size);
                }
                // Define the path for save the file
                String pathMyPictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                String specifiedFolder = @"\Webcam Pictures\";
                String imageName = String.Format("\\Snapshot_{0:yyyyMMdd_hhmmss}.jpg", DateTime.Now);
                String completeImagePath = pathMyPictures + imageName;
                // Save the image 
                bitmap.Save(completeImagePath,ImageFormat.Jpeg);
                // Inform saving path into statut bar
                textBlockStatutBar.Text = "Image saved into: " + completeImagePath;
            }
        }

    }
}
