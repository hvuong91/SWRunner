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
using static SWRunner.RunnerLogger;

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
                log.Text += logger.Message.Dequeue().message.ToString();
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
            Rune rune1 = new Rune.RuneBuilder().Grade("6*").Set("Blade").Slot("4").
                        Rarity("Legendary").MainStat("ATK%").PrefixStat("DEF +5").
                        SubStat1("HP% +5").SubStat2("SPD +4").SubStat3("HP +200").
                        SubStat4("CRATE +6").Build();
            Logger.Log(ACTION.GET, rune1);
            Thread.Sleep(1000);
            Rune rune2 = new Rune.RuneBuilder().Grade("4*").Set("Violent").Slot("4").
                        Rarity("Legendary").MainStat("ATK%").PrefixStat("DEF +5").
                        SubStat1("HP% +5").SubStat2("SPD +4").SubStat3("HP +200").
                        SubStat4("CRATE +6").Build();
            Logger.Log(ACTION.SELL, rune2);

            while (Logger.Message.Count >0)
            {
                //log.Text = Logger.Message.Dequeue() + log.Text;
                Grid grid = CreateMessageLog(Logger.Message.Dequeue());
                logPanel.Children.Insert(0, grid);
            }

        }

        private Grid CreateMessageLog((ACTION action, Object message, DateTime timeStamp) log)
        {

            Rune rune = (Rune) log.message;

            // Main container
            Grid grid = new Grid();
            grid.Margin = new Thickness(0, 10, 0, 10);
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(250) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(200) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(80) });

            // Action
            Border border = new Border();
            border.BorderBrush = Brushes.Black;
            border.BorderThickness = new Thickness(2);
            border.CornerRadius = new CornerRadius(10);
            border.Width = 60;
            if (log.action == ACTION.GET)
            {
                border.Background = Brushes.Green;
            }
            else
            {
                border.Background = Brushes.Red;
            }

            Label lbl = new Label();
            lbl.Content = log.action;
            Grid.SetRow(lbl, 0);
            Grid.SetColumn(lbl, 0);
            lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
            lbl.FontSize = 8;
            lbl.FontWeight = FontWeights.Bold;
            lbl.Margin = new Thickness(-2, -2, -2, -2);
            border.Child = lbl;

            // Item image
            Grid imageGrid = new Grid();
            Grid.SetColumn(imageGrid, 0);
            Grid.SetRow(imageGrid, 1);
            imageGrid.Background = Brushes.Orange;
            imageGrid.Margin = new Thickness(0, 5, 0, 0);
            Image image = new Image();
            image.Source = new BitmapImage(new Uri($"assets/{rune.Set}.png", UriKind.Relative));
            imageGrid.Children.Add(image);

            // Stars
            StackPanel starPanel = new StackPanel();
            starPanel.Orientation = Orientation.Horizontal;
            starPanel.Margin = new Thickness(5, 0, 0, 0);
            Grid.SetColumn(starPanel, 1);
            Grid.SetRow(starPanel, 0);

            int stars = 6;
            while (stars-- > 0)
            {
                Image starImage = new Image();
                starImage.Source = new BitmapImage(new Uri(@"assets/star-unawakened.png", UriKind.Relative));
                Grid.SetRowSpan(starImage, 2);
                starPanel.Children.Add(starImage);
            }

            // Text
            TextBlock textBlock = new TextBlock();
            textBlock.Margin = new Thickness(5, 0, 0, 0);
            textBlock.Text = "Message Here" + Environment.NewLine + "With new line";
            Grid.SetRow(textBlock, 1);
            Grid.SetColumn(textBlock, 1);

            // Timestamp
            TextBlock timeStampTextBlock = new TextBlock();
            timeStampTextBlock.Margin = new Thickness(5, 0, 0, 0);
            timeStampTextBlock.Text = log.timeStamp.ToString();
            Grid.SetRow(timeStampTextBlock, 0);
            Grid.SetColumn(timeStampTextBlock, 2);


            // Add all to grid container
            grid.Children.Add(border);
            grid.Children.Add(imageGrid);
            grid.Children.Add(starPanel);
            grid.Children.Add(textBlock);
            grid.Children.Add(timeStampTextBlock);

            return grid;
        }

    }
}
