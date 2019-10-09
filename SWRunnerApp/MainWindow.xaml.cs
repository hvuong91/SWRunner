using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SWRunnerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SWRunnerPresenter Presenter { get; } = new SWRunnerPresenter();

        private CancellationTokenSource tokenSource;

        BackgroundWorker backgroundWorker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();

            backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            backgroundWorker.ProgressChanged += BackgroundWorkerOnProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorkerRunCompleted;
        }

        private void BackgroundWorkerOnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            object userObject = e.UserState;
            int percentage = e.ProgressPercentage;

            log.Text += DateTime.Now + ": " + userObject + Environment.NewLine;
        }

        private void BackgroundWorkerRunCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                log.Text += DateTime.Now + ": === Canceled ===" + Environment.NewLine;
            }
            else if (e.Error != null)
            {
                log.Text += DateTime.Now + ": === Error!!! ===" + Environment.NewLine;
            }
            else
            {
                log.Text += DateTime.Now + ": === Stopped ===" + Environment.NewLine; 
            }
        }

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            while (!worker.CancellationPending)
            {
                //TODO: Call actual runner from argument
                Thread.Sleep(1000);
                worker.ReportProgress(0, e.Argument);
            }
        }

        private void StartCairos_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add runner object to runworkerasync call
            backgroundWorker.RunWorkerAsync("This should be runner object");

            btnStopRun.IsEnabled = true;

            btnCairos.IsEnabled = false;
        }

        private void BtnStopRun_Click(object sender, RoutedEventArgs e)
        {
            // Cancel backgroundworkers
            backgroundWorker.CancelAsync();
            btnStopRun.IsEnabled = false;

            // Enable other buttons
            btnCairos.IsEnabled = true;
        }

        private void Log_TextChanged(object sender, TextChangedEventArgs e)
        {
            log.ScrollToEnd();
        }
    }
}
