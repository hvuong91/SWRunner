using SWRunner.Runners;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SWRunnerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SWRunnerPresenter Present { get; } = new SWRunnerPresenter();

        private CancellationTokenSource tokenSource;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(ConfigurationManager.AppSettings["CairosRunnerConfig"]);

            if (tokenSource != null)
            {
                tokenSource.Cancel();
            }
            else
            {
                tokenSource = new CancellationTokenSource();
                ((Button)sender).Content = "Cancel";
                try
                {
                    await DoWorkAsyncInfiniteLoop(tokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    log.Text += "Canceled";
                }
            }

            tokenSource = null;
        }

        private async Task DoWorkAsyncInfiniteLoop(CancellationToken ct)
        {
            while (true)
            {
                // do the work in the loop
                string newData = DateTime.Now.ToLongTimeString();

                // update the UI
                log.Text += newData + Environment.NewLine;

                // don't run again for at least 200 milliseconds
                await Task.Delay(1000, ct);
            }
        }

        private void Log_TextChanged(object sender, TextChangedEventArgs e)
        {
            log.ScrollToEnd();
        }
    }
}
