namespace SWRunner.Runners
{
    public interface IRunner
    {
        void Run();
        void Collect();
        bool IsEnd();
        bool IsFailed();
        void SkipRevive();
        void CheckRefill();
        void StartNewRun();
        void StopRunner();
    }
}
