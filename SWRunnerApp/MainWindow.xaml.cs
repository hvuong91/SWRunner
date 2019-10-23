using SWRunner;
using SWRunner.Runners;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

            cmbColors.ItemsSource = typeof(Colors).GetProperties();
        }

        private void BackgroundWorkerOnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            object userObject = e.UserState;

            RunnerLogger logger = (RunnerLogger)e.UserState;

            log.Text += DateTime.Now + ": " + logger.Message + Environment.NewLine;

            lblRuneCollect.Content = "Rune Collect: " + Logger.GetRunes;
            lblRuneSell.Content = "Rune Sell: " + Logger.SellRunes;
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
                log.Text += e.ToString() + Environment.NewLine;
            }
            else
            {
                log.Text += DateTime.Now + ": === Stopped ===" + Environment.NewLine; 
            }
        }

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            IRunner runner = (IRunner)e.Argument;
            runner.ReadyRunner();

            while (!worker.CancellationPending)
            {
                runner.Run();
                Debug.WriteLine("Finish run");
                if (!worker.CancellationPending)
                {
                    Debug.WriteLine("Start logging ...");
                    worker.ReportProgress(0, Logger);
                }

            }
        }

        private void StartCairos_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add runner object to runworkerasync call
            while (backgroundWorker.IsBusy) { }
            Presenter.ActiveRunner = Presenter.CairosRunner;
            backgroundWorker.RunWorkerAsync(Presenter.ActiveRunner);

            // Update buttons
            UpdateButtons();
        }

        private void StartToa_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add runner object to runworkerasync call
            while (backgroundWorker.IsBusy) { }
            Presenter.ActiveRunner = Presenter.ToaRunner;
            backgroundWorker.RunWorkerAsync(Presenter.ActiveRunner);

            // Update buttons
            UpdateButtons();
        }

        private void StartRift_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Add runner object to runworkerasync call
            while (backgroundWorker.IsBusy) { }
            Presenter.ActiveRunner = Presenter.RiftRunner;
            backgroundWorker.RunWorkerAsync(Presenter.ActiveRunner);

            // Update buttons
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            btnStopRun.IsEnabled = !btnStopRun.IsEnabled;

            btnCairos.IsEnabled = !btnCairos.IsEnabled;
            btnToa.IsEnabled = !btnToa.IsEnabled;
            btnRift.IsEnabled = !btnRift.IsEnabled;
        }

        private void BtnStopRun_Click(object sender, RoutedEventArgs e)
        {
            // Cancel backgroundworkers
            Presenter.ActiveRunner.StopRunner();
            Presenter.ActiveRunner = null;

            backgroundWorker.CancelAsync();

            // Update buttons
            UpdateButtons();

        }

        private void Log_TextChanged(object sender, TextChangedEventArgs e)
        {
            log.ScrollToEnd();
        }

    }
}
