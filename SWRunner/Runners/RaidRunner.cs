using SWRunner.Rewards;
using System;
using System.Threading;

namespace SWRunner.Runners
{
    class RaidRunner : AbstractRunner<AbstractRunnerConfig>
    {
        public bool IsHost { get; set; }

        public RaidRunner(string logFile) : base(logFile, "",  null, null, null)
        {
            IsHost = true; // Set host as default
        }

        public override void Collect()
        {
            throw new NotImplementedException();
        }

        public override void Run()
        {
            // 1. Check run ends
            // 2. If succeed, collect reward. Otherwise, ignore
            // 3. Start new run
            // 4. Wait for party ready if play as host

            ModifiedTime = DateTime.Now;

            while (!Stop)
            {
                Thread.Sleep(3000);

                // TODO
            }


            throw new NotImplementedException();
        }

        private bool IsAllReady()
        {
            throw new NotImplementedException();
        }

    }
}
