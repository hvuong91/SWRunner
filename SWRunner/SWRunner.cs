using SWRunner.Filters;
using SWRunner.Runners;

namespace SWRunner
{
    class SWRunner
    {
        private CairosRunner cairosRunner;
        private DimensionalRunner dimensionalRunner;

        private SWLogger logger = new SWLogger();

        public CairosRunner CairosRunner { get { return cairosRunner; } }
        public DimensionalRunner DimensionalRunner { get { return dimensionalRunner; } }
        public SWLogger Logger { get { return logger; } }

        SWRunner(CairosFilter cairosFilter, DimensionalFilter dimensionalFilter)
        {
            //TODO: logs, emulator, filter config
            cairosRunner = new CairosRunner(cairosFilter, null);
            dimensionalRunner = new DimensionalRunner(dimensionalFilter, null);
        }
    }
}
