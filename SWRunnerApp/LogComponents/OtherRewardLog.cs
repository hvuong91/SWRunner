using SWRunner.Rewards;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using static SWRunner.RunnerLogger;

namespace SWRunnerApp.LogComponents
{
    class OtherRewardLog : Grid
    {
        public OtherRewardLog(ACTION action, Reward reward, DateTime timeStamp)
        {
            Initialize(action, reward, timeStamp);
        }

        private void Initialize(ACTION action, Reward reward, DateTime timeStamp)
        {
            // Main container
            this.Margin = new Thickness(0, 10, 0, 10);
            this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(330) });
            this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(200) });

            // Message
            TextBlock messageTextBlock = new TextBlock
            {
                Margin = new Thickness(10, 0, 0, 10),
                Text = action + ": " +reward.ToString()
            };
            Grid.SetColumn(messageTextBlock, 0);

            // Timestamp
            TextBlock timeStampTextBlock = new TextBlock
            {
                Text = timeStamp.ToString()
            };
            Grid.SetColumn(timeStampTextBlock, 1);

            this.Children.Add(messageTextBlock);
            this.Children.Add(timeStampTextBlock);
        }
    }
}
