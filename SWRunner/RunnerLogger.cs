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
        public int FailedRuns { get; private set; } = 0;

        public int GetRunes { get; private set; } = 0;

        public int SellRunes { get; private set; } = 0;

        // TODO: stack of messages? Need to log run failed, refill etc. on the same run
        public Queue<(ACTION action, Object message, DateTime timeStamp)> Message { get; private set; } = new Queue<(ACTION action, Object message, DateTime timeStamp)>();

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
                FailedRuns += 1;
            }

            if (reward.Type == REWARDTYPE.RUNE)
            {
                if (getReward)
                {
                    GetRunes += 1;
                    Log(ACTION.GET,reward);
                }
                else
                {
                    SellRunes += 1;
                    Log(ACTION.SELL, reward);
                }
            }
            else
            {
                Log(ACTION.GET, reward);
            }
        }

        public void Log(Object message)
        {
            Message.Enqueue((ACTION.NONE, message, DateTime.Now));
        }

        public void Log(ACTION action, Object message)
        {
            Message.Enqueue((action, message, DateTime.Now));
        }

        public enum ACTION
        {
            NONE, GET, SELL
        }
    }
}
