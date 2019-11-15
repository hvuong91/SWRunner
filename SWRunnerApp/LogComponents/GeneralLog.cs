using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SWRunnerApp.LogComponents
{
    class GeneralLog : Grid
    {
        public GeneralLog(string message, DateTime timeStamp)
        {
            Initialize(message, timeStamp);
        }

        private void Initialize(string message, DateTime timeStamp)
        {
            // Main container
            this.Margin = new Thickness(0, 10, 0, 10);
            this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });

            // Message
            TextBlock messageTextBlock = new TextBlock
            {
                Margin = new Thickness(10, 0, 0, 10),
                Text = message
            };
            Grid.SetColumn(messageTextBlock, 0);

            // Timestamp
            TextBlock timeStampTextBlock = new TextBlock
            {
                Margin = new Thickness(0, 0, 10, 0),
                Text = timeStamp.ToString()
            };
            Grid.SetColumn(timeStampTextBlock, 1);

            this.Children.Add(messageTextBlock);
            this.Children.Add(timeStampTextBlock);
        }
    }
}
