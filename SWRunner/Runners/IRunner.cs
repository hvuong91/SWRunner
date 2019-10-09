using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SWRunner.Runners
{
    interface IRunner
    {
        Task Run(CancellationToken ct);
        void Collect();
        bool IsEnd();
        bool IsFailed();
        void SkipRevive();
        void CheckRefill();
        void StartNewRun();
    }
}
