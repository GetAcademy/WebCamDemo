using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
using AForge.Video;
using AForge.Video.DirectShow;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VideoCaptureDevice _videoSource;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // enumerate video devices
            var videoDevices = new FilterInfoCollection(
                FilterCategory.VideoInputDevice);
            // create video source
            _videoSource = new VideoCaptureDevice(
                videoDevices[0].MonikerString);
            // set NewFrame event handler
            _videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            // start the video source
            _videoSource.Start();
            // ...
            // signal to stop
            //videoSource.SignalToStop();
            // ...


        }

        private void video_NewFrame(object sender,
            NewFrameEventArgs eventArgs)
        {
            _videoSource.SignalToStop();
            // get new frame
            var bitmap = eventArgs.Frame;
            // process the frame

            MyImage.Dispatcher.Invoke(
                new Action(() => MyImage.Source = GetBitmapImage(bitmap)));

        }

        private static BitmapImage GetBitmapImage(Bitmap bitmap)
        {
            BitmapImage bitmapImage;
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }
    }
}
