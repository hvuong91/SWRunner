using System;
using System.Collections.Generic;
using System.Text;

namespace SWRunner.Runners
{
    public abstract class AbstractRunner : IRunner
    {
        public void CheckRefill()
        {
            throw new NotImplementedException();
        }

        public abstract void Collect();

        public abstract bool IsEnd();

        public abstract bool IsFailed();

        public abstract void Run();

        public void SkipRevive()
        {
            throw new NotImplementedException();
        }

        public void StartNewRun()
        {
            throw new NotImplementedException();
        }
    }
}
