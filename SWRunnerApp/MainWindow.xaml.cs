using SWRunner;
using SWRunner.Filters;
using SWRunner.Rewards;
using SWRunner.Runners;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using static SWRunner.Rewards.Rune;

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

        private GemStoneFilter gemStoneFilter;


        public MainWindow()
        {
            InitializeComponent();
            InitializeBackgroundWorker();

            LoadGemStoneFilter();
            InitializeGemStoneFilter();

            Logger = new RunnerLogger();
            Presenter = new SWRunnerPresenter(Logger, gemStoneFilter.GemStoneList);
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            backgroundWorker.ProgressChanged += BackgroundWorkerOnProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorkerRunCompleted;
        }

        private void InitializeGemStoneFilter()
        {
            cbSet.ItemsSource = Enum.GetValues(typeof(RUNESET)).Cast<RUNESET>();
            cbType.Items.Add(REWARDTYPE.GRINDSTONE);
            cbType.Items.Add(REWARDTYPE.ENCHANTEDGEM);

            // TODO: Update the list
            List<string> allMainStats = new List<string>()
            {
                "HP flat", "HP%", "ATK flat", "ATK%", "DEF flat", "DEF%", "SPD", "CRate", "CDmg", "RES", "ACC", "ALL"
            };

            cbMainStat.ItemsSource = allMainStats;
            cbRarity.ItemsSource = Enum.GetValues(typeof(RARITY)).Cast<RARITY>();

            lvGemStoneList.ItemsSource = gemStoneFilter.GemStoneList;

        }

        private void LoadGemStoneFilter()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GemStoneFilter), new XmlRootAttribute("GemStoneFilter"));

            // Declare an object variable of the type to be deserialized.
            string gemStoneFilterXml = @"RunnersConfig/GemStoneFilter.xml";

            using (Stream reader = new FileStream(gemStoneFilterXml, FileMode.Open))
            {
                // Call the Deserialize method to restore the object's state.
                gemStoneFilter = (GemStoneFilter)serializer.Deserialize(reader);
            }
        }

        private void BackgroundWorkerOnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            object userObject = e.UserState;

            RunnerLogger logger = (RunnerLogger)e.UserState;

            while(logger.Message.Count > 0){
                log.Text += logger.Message.Dequeue();
            }

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

        private void btnRemoveGemStone_Click(object sender, RoutedEventArgs e)
        {
            List<GemStone> list = (List<GemStone>)lvGemStoneList.ItemsSource;
            foreach (GemStone eachItem in lvGemStoneList.SelectedItems)
            {
                list.Remove(eachItem);
            }
            lvGemStoneList.ItemsSource = null;
            lvGemStoneList.ItemsSource = list;
        }

        private void BtnSaveGemStoneList_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer writer = new XmlSerializer(typeof(GemStoneFilter));

            var path = @"RunnersConfig/GemStoneFilter.xml";
            FileStream file = File.Create(path);

            writer.Serialize(file, gemStoneFilter);
            file.Close();

            MessageBox.Show("Filter has been saved!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnAddGemStoneFilter_Click(object sender, RoutedEventArgs e)
        {
            REWARDTYPE type = (REWARDTYPE) cbType.SelectedItem;
            RUNESET set = (RUNESET)cbSet.SelectedItem;
            string mainStat = (string)cbMainStat.SelectedItem;
            RARITY rarity = (RARITY)cbRarity.SelectedItem;

            List<GemStone> list = (List<GemStone>)lvGemStoneList.ItemsSource;

            if (type == REWARDTYPE.GRINDSTONE)
            {
                GrindStone grindStone = new GrindStone(set, mainStat, rarity);
                list.Add(grindStone);
            }
            else
            {
                EnchantedGem enchantedGem = new EnchantedGem(set, mainStat, rarity);
                list.Add(enchantedGem);
            }

            lvGemStoneList.ItemsSource = null;
            lvGemStoneList.ItemsSource = list;

        }

        private void btnTestLog_Click(object sender, RoutedEventArgs e)
        {
            Logger.Log("Log 1");
            Logger.Log("Log 2");

            while (Logger.Message.Count >0)
            {
                log.Text = Logger.Message.Dequeue() + log.Text;
            }
        }
    }
}
