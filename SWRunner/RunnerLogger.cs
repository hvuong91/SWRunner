using SWRunner.Rewards;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWRunner
{
    public class RunnerLogger
    {
        public RunnerLogger() { }

        public List<KeyValuePair<RunResult, Reward>> Results { get; } = new List<KeyValuePair<RunResult, Reward>>();

        // TODO: Need a map of values need to be displayed on the UI
        public int SuccessRuns { get; private set; } = 0;
        public int FailRuns { get; private set; } = 0;

        public int GetRunes { get; private set; } = 0;

        public int SellRunes { get; private set; } = 0;

        // TODO: stack of messages? Need to log run failed, refill etc. on the same run
        public Queue<string> Message { get; private set; } = new Queue<string>();

        public void Log(RunResult runResult, Reward reward, bool getReward)
        {
            // TODO: Might need to keep track of the latest message only
            Results.Add(new KeyValuePair<RunResult, Reward>(runResult, reward));

            // TODO: Add more log details
            if (runResult.Result)
            {
                SuccessRuns += 1;
            }
            else
            {
                FailRuns += 1;
            }

            if (reward.Type == REWARDTYPE.RUNE)
            {
                if (getReward)
                {
                    GetRunes += 1;
                    Log("Get Rune:" + Environment.NewLine + reward.ToString());
                }
                else
                {
                    SellRunes += 1;
                    Log("Sell Rune:" + Environment.NewLine + reward.ToString());
                }
            }
            else
            {
                Log("Get " + reward.ToString());
            }
        }

        public void Log(string message)
        {
            Message.Enqueue(DateTime.Now + ": " + message + Environment.NewLine);
        }

    }
}
