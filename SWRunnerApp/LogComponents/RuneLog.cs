using SWRunner.Rewards;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static SWRunner.Rewards.Rune;
using static SWRunner.RunnerLogger;

namespace SWRunnerApp.LogComponents
{
    class RuneLog : Grid
    {
        public ACTION Action;

        public RuneLog(ACTION action, Rune rune, DateTime timeStamp)
        {
            Action = action;
            Initialize(action, rune, timeStamp);
        }

        private void Initialize(ACTION action, Rune rune, DateTime timeStamp)
        {
            // Main container
            this.Margin = new Thickness(0, 10, 0, 10);

            this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
            this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(250) });
            this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(200) });
            this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20) });
            this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100) });

            // Action
            Border border = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(10),
                Width = 60
            };
            if (action == ACTION.GET)
            {
                border.Background = Brushes.Green;
            }
            else
            {
                border.Background = Brushes.Red;
            }

            Label lbl = new Label
            {
                Content = action
            };
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
            imageGrid.Background = GetRarityColor(rune.Rarity);
            imageGrid.Margin = new Thickness(0, 5, 0, 0);
            Image image = new Image
            {
                Source = new BitmapImage(new Uri($"assets/{rune.Set}.png", UriKind.Relative))
            };
            imageGrid.Children.Add(image);

            // Stars
            StackPanel starPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(5, 0, 0, 0)
            };
            Grid.SetColumn(starPanel, 1);
            Grid.SetRow(starPanel, 0);

            Int32.TryParse(rune.Grade.Replace("*", String.Empty), out int stars);

            while (stars-- > 0)
            {
                Image starImage = new Image();
                starImage.Source = new BitmapImage(new Uri(@"assets/star-unawakened.png", UriKind.Relative));
                Grid.SetRowSpan(starImage, 2);
                starPanel.Children.Add(starImage);
            }

            // Text
            TextBlock textBlock = new TextBlock
            {
                Margin = new Thickness(5, 0, 0, 0),
                Text = rune.ToString()
            };
            Grid.SetRow(textBlock, 1);
            Grid.SetColumn(textBlock, 1);

            // Timestamp
            TextBlock timeStampTextBlock = new TextBlock
            {
                Text = timeStamp.ToString()
            };
            Grid.SetRow(timeStampTextBlock, 0);
            Grid.SetColumn(timeStampTextBlock, 2);


            // Add all to grid container
            this.Children.Add(border);
            this.Children.Add(imageGrid);
            this.Children.Add(starPanel);
            this.Children.Add(textBlock);
            this.Children.Add(timeStampTextBlock);
        }

        private Brush GetRarityColor(RARITY rarity)
        {
            Brush color;

            switch (rarity)
            {
                case RARITY.NORMAL:
                    color = Brushes.Silver;
                    break;
                case RARITY.MAGIC:
                    color = Brushes.ForestGreen;
                    break;
                case RARITY.RARE:
                    color = Brushes.DeepSkyBlue;
                    break;
                case RARITY.HERO:
                    color = Brushes.MediumPurple;
                    break;
                case RARITY.LEGENDARY:
                    color = Brushes.Orange;
                    break;
                default:
                    color = Brushes.White;
                    break;
            }

            return color;
        }
    }
}
