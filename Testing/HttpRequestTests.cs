using EvMeterValueManager;
using EvMeterValueManager.Models;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    [TestClass]
    public class HttpRequestTests
    {
        public EvMeterWorker worker = new EvMeterWorker();

        [TestMethod]
        public async Task TestWorkerMethod()
        {
            string resultString = await worker.CreateMeterValueAndSend();
            string successResultMessage = "MeterValue added successfully. ";

            Assert.IsNotNull(resultString);
            Assert.AreEqual(resultString, successResultMessage);
        }
    }
}
