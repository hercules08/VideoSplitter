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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emgu.CV;
using WF = System.Windows.Forms;

namespace VideoSplitter.Module.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        string videoFilename;
        string outputDirectory;

        public HomeView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //IntPtr image = CvInvoke.cvCreateImage(new System.Drawing.Size(400, 300), Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_8U, 1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.FileName = "";
            fileDialog.DefaultExt = ".mp4";
            fileDialog.Filter = "MP4 Files (.mp4)|*.mp4|AVI Files (*.avi)|*.avi"; //Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = fileDialog.ShowDialog();

            if (result == true)
            {
                videoFilename = fileDialog.FileName;

                var folderDialog = new WF.FolderBrowserDialog();
                WF.DialogResult folderResult = folderDialog.ShowDialog();


                if (folderResult == System.Windows.Forms.DialogResult.OK)
                {
                    outputDirectory = folderDialog.SelectedPath;

                    //Split video
                    VideoSplittingView splittingWindow = new VideoSplittingView();
                    //splittingWindow.Show();

                    Capture capture = new Capture(videoFilename);

                    if (capture != null)
                    {
                        IntPtr frame;
                        int i = 0;
                        while ((frame = CvInvoke.cvQueryFrame(capture)) != null && (frame.ToInt32() != 0))
                        {
                            i++;
                            string saveLoc = outputDirectory + "\\" + i + ".jpg";
                            CvInvoke.cvSaveImage(saveLoc, frame, frame);
                        }

                    }

                    //splittingWindow.Close();

                    System.Diagnostics.Process prc = new System.Diagnostics.Process();
                    prc.StartInfo.FileName = outputDirectory;
                    prc.Start();
                }
            }
        }
    }
}
