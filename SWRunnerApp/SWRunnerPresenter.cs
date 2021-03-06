﻿using SWEmulator;
using SWRunner;
using SWRunner.Filters;
using SWRunner.Rewards;
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
        public ToARunner ToaRunner { get; private set; }
        public RiftRunner RiftRunner { get; private set; }
        public DimensionalRunner DimensionalRunner { get; private set; }
        public RaidRunner RaidRunner { get; private set; }
        public RunnerLogger Logger { get; private set; }

        

        public List<GemStone> AcceptedGemStones { get; set; }

        public SWRunnerPresenter(RunnerLogger logger, List<GemStone> acceptedGemStones)
        {
            Logger = logger;
            AcceptedGemStones = acceptedGemStones;

            InitCairosRunner();
            InitToaRunner();
            InitRiftRunner();
            InitRaidRunner();
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
            CairosRunner.MaxRunTime = new TimeSpan(0, 0, Int32.Parse(ConfigurationManager.AppSettings["CairosMaxRunTimeInSeconds"]));
        }

        private void InitToaRunner()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ToaRunnerConfig), new XmlRootAttribute("RunConfig"));
            string configXml = ConfigurationManager.AppSettings["ToaRunnerConfig"];
            string toaLog = ConfigurationManager.AppSettings["ToaLog"];
            string fullLog = ConfigurationManager.AppSettings["FullLog"];

            ToaRunnerConfig runConfig;

            using (Stream reader = new FileStream(configXml, FileMode.Open))
            {
                // Call the Deserialize method to restore the object's state.
                runConfig = (ToaRunnerConfig)serializer.Deserialize(reader);
            }

            // TODO
            ToaRunner = new ToARunner(toaLog, fullLog, runConfig, new NoxEmulator(), Logger);
        }

        private void InitRiftRunner()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RiftRunnerConfig), new XmlRootAttribute("RunConfig"));
            string configXml = ConfigurationManager.AppSettings["RiftRunnerConfig"];
            string riftLog = ConfigurationManager.AppSettings["Riftlog"];
            string fullLog = ConfigurationManager.AppSettings["FullLog"];

            RiftRunnerConfig runConfig;

            using (Stream reader = new FileStream(configXml, FileMode.Open))
            {
                // Call the Deserialize method to restore the object's state.
                runConfig = (RiftRunnerConfig)serializer.Deserialize(reader);
            }

            RiftRunner = new RiftRunner(new RiftFilter(AcceptedGemStones),riftLog, fullLog, runConfig, new NoxEmulator(), Logger);
        }

        private void InitRaidRunner()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RaidRunnerConfig), new XmlRootAttribute("RunConfig"));
            string configXml = ConfigurationManager.AppSettings["RaidRunnerConfig"];
            string riftLog = ConfigurationManager.AppSettings["Riftlog"];
            string fullLog = ConfigurationManager.AppSettings["FullLog"];

            RaidRunnerConfig runConfig;

            using (Stream reader = new FileStream(configXml, FileMode.Open))
            {
                // Call the Deserialize method to restore the object's state.
                runConfig = (RaidRunnerConfig)serializer.Deserialize(reader);
            }

            RaidRunner = new RaidRunner(new RaidFilter(AcceptedGemStones), riftLog, fullLog, runConfig, new NoxEmulator(), Logger);
        }

    }
}
