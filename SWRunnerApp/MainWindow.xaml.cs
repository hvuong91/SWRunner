using SWRunner;
using SWRunner.Runners;
using System;
using System.ComponentModel;
using System.Diagnostics;
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
        private SWRunnerPresenter Presenter { get; set; } 
        private RunnerLogger Logger { get; set; }

        BackgroundWorker backgroundWorker = new BackgroundWorker();



        public MainWindow()
        {
            InitializeComponent();

            Logger = new RunnerLogger();
            Presenter = new SWRunnerPresenter(Logger);

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
                IRunner runner = (IRunner) e.Argument;
                runner.Run();
                Thread.Sleep(1000);

                if (!worker.CancellationPending)
                {
                    worker.ReportProgress(0, Logger.Message);
                }

            }
        }

        private void StartCairos_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add runner object to runworkerasync call
            while (backgroundWorker.IsBusy) { }
            Presenter.ActiveRunner = Presenter.CairosRunner;
            backgroundWorker.RunWorkerAsync(Presenter.CairosRunner);

            // Update buttons
            btnStopRun.IsEnabled = true;

            btnCairos.IsEnabled = false;
        }

        private void BtnStopRun_Click(object sender, RoutedEventArgs e)
        {
            // Cancel backgroundworkers
            Presenter.ActiveRunner.StopRunner();
            Presenter.ActiveRunner = null;

            backgroundWorker.CancelAsync();

            // Update buttons
            btnStopRun.IsEnabled = false;

            btnCairos.IsEnabled = true;

        }

        private void Log_TextChanged(object sender, TextChangedEventArgs e)
        {
            log.ScrollToEnd();
        }
    }
}
