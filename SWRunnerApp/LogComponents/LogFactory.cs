using SWRunner.Rewards;
using System;
using System.Collections.Generic;
using System.Windows;
using static SWRunner.RunnerLogger;

namespace SWRunnerApp.LogComponents
{
    public static class LogFactory
    {
        public static UIElement Build((ACTION action, Object message, DateTime timeStamp) logEntry)
        {
            // TODO: new class for Grindstone/gem
            if (logEntry.message.GetType() == typeof(Rune))
            {
                return new RuneLog(logEntry.action, (Rune)logEntry.message, logEntry.timeStamp);
            }
            else if (logEntry.message is Reward)
            {
                return new OtherRewardLog(logEntry.action, (Reward)logEntry.message, logEntry.timeStamp);
            }
            else 
            {
                return new GeneralLog(logEntry.message.ToString(), logEntry.timeStamp);
            }

        }
    }
}
