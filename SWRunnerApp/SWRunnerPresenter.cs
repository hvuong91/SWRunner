using SWEmulator;
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
        public CairosRunner CairosRunner { get; private set; }

        public bool Stop { get; set; } = false;

        public SWRunnerPresenter()
        {
            InitCairosRunner();
        }

        private void InitCairosRunner()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CairosRunnerConfig), new XmlRootAttribute("RunConfig"));
            string testConfigXml = ConfigurationManager.AppSettings["CairosRunnerConfig"];
            CairosRunnerConfig runConfig;

            using (Stream reader = new FileStream(testConfigXml, FileMode.Open))
            {
                // Call the Deserialize method to restore the object's state.
                runConfig = (CairosRunnerConfig)serializer.Deserialize(reader);
            }

            // TODO
            CairosRunner = new CairosRunner(new CairosFilter(), "", runConfig, new NoxEmulator(""));
        }

    }
}
