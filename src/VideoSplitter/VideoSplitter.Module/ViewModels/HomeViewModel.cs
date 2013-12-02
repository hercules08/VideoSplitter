using System;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Emgu.CV;
using WF = System.Windows.Forms;
using VideoSplitter.Module.Views;

namespace VideoSplitter.Module.ViewModels
{
    public class HomeViewModel : NotificationObject
    {
        string _videoFilename;
        string _outputDirectory;

        public ICommand SplitCommand { get; private set; }
        public ICommand ReassembleCommand { get; private set; }

        public HomeViewModel()
        {
            SplitCommand = new DelegateCommand<object>(OnSplit);
            ReassembleCommand = new DelegateCommand<object>(OnReassemble, CanReassemble);
        }

        void OnSplit(object arg)
        {

            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.FileName = "";
            fileDialog.DefaultExt = ".mp4";
            fileDialog.Filter = "MP4 Files (.mp4)|*.mp4|AVI Files (*.avi)|*.avi"; //Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = fileDialog.ShowDialog();

            if (result == true)
            {
                _videoFilename = fileDialog.FileName;

                var folderDialog = new WF.FolderBrowserDialog();
                WF.DialogResult folderResult = folderDialog.ShowDialog();


                if (folderResult == System.Windows.Forms.DialogResult.OK)
                {
                    _outputDirectory = folderDialog.SelectedPath;

                    //Split video
                    VideoSplittingView splittingWindow = new VideoSplittingView();
                    //splittingWindow.Show();

                    Capture capture = new Capture(_videoFilename);

                    if (capture != null)
                    {
                        IntPtr frame;
                        int i = 0;
                        while ((frame = CvInvoke.cvQueryFrame(capture)) != null && (frame.ToInt32() != 0))
                        {
                            i++;
                            string saveLoc = _outputDirectory + "\\" + i + ".jpg";
                            CvInvoke.cvSaveImage(saveLoc, frame, frame);
                        }

                    }

                    //splittingWindow.Close();

                    System.Diagnostics.Process prc = new System.Diagnostics.Process();
                    prc.StartInfo.FileName = _outputDirectory;
                    prc.Start();
                }
            }
        }

        void OnReassemble(object arg)
        {

        }

        bool CanReassemble(object arg)
        {
            return false;
        }



    }
}
