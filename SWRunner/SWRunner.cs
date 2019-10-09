using SWRunner.Filters;
using SWRunner.Runners;

namespace SWRunner
{
    class SWRunner
    {
        private CairosRunner cairosRunner;
        private DimensionalRunner dimensionalRunner;

        public CairosRunner CairosRunner { get { return cairosRunner; } }
        public DimensionalRunner DimensionalRunner { get { return dimensionalRunner; } }

        SWRunner(CairosFilter cairosFilter, DimensionalFilter dimensionalFilter)
        {
            //TODO: logs, emulator, filter config
        }
    }
}
