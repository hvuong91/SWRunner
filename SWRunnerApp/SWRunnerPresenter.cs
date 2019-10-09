using SWEmulator;
using SWRunner;
using SWRunner.Filters;
using SWRunner.Runners;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace SWRunnerApp
{
    class SWRunnerPresenter
    {
        public IRunner ActiveRunner { get; set; }

        public CairosRunner CairosRunner { get; private set; }

        public RunnerLogger Logger { get; private set; }

        public SWRunnerPresenter(RunnerLogger logger)
        {
            Logger = logger;
            InitCairosRunner();
        }

        private void InitCairosRunner()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CairosRunnerConfig), new XmlRootAttribute("RunConfig"));
            string configXml = ConfigurationManager.AppSettings["CairosRunnerConfig"];
            string runLog = ConfigurationManager.AppSettings["RunsLog"];
            string fullLog = ConfigurationManager.AppSettings["FullLog"];

            CairosRunnerConfig runConfig;

            using (Stream reader = new FileStream(configXml, FileMode.Open))
            {
                // Call the Deserialize method to restore the object's state.
                runConfig = (CairosRunnerConfig)serializer.Deserialize(reader);
            }

            // TODO
            CairosRunner = new CairosRunner(new CairosFilter(), runLog, fullLog, runConfig, new NoxEmulator(), Logger);
        }

    }
}
