using EvMeterValueManager.Models;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net.Http.Headers;

namespace EvMeterValueManager
{
    public class EvMeterWorker
    {
        public const int SECONDS_DELAY = 5;
        public const int MILI_TO_SECONDS = 1000;
        const int DIVIDER = 3;

        private static readonly HttpClient client = new HttpClient();

        private BackgroundWorker worker = new BackgroundWorker();

        private MeterValueGenerator meterValueGenerator = new MeterValueGenerator();

        private int countCreatedMeterValues = 0;

        const string PostUrl = "https://localhost:7098/MeterValueController/ReceiveMeterValue";


        public EvMeterWorker()
        {
            worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
        }

        public void RunWorker()
        {
            worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs eventArgs)
        {
            CreateMeterValueAndPostAsync().Wait();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs eventArgs)
        {

        }

        private async Task CreateMeterValueAndPostAsync()
        {
            while (true)
            {
                await CreateMeterValueAndSend();
            }
        }

        public async Task<string> CreateMeterValueAndSend()
        {
            var content = JsonConvert.SerializeObject(CreateMeterValue());
            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var URL = PostUrl;

            var response = await client.PostAsync(URL, byteContent);

            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        private MeterValue CreateMeterValue()
        {
            Random rnd = new Random();

            if (countCreatedMeterValues > 0)
            {
                if (countCreatedMeterValues % DIVIDER == 0)
                {
                    Task.Delay(MILI_TO_SECONDS * (rnd.Next(SECONDS_DELAY) + SECONDS_DELAY + 1)).Wait();
                }
                else
                {
                    Task.Delay(MILI_TO_SECONDS * SECONDS_DELAY).Wait();
                }
            }

            countCreatedMeterValues++;

            return meterValueGenerator.CreateMeterValue();
        }
    }

    public class MeterValueGenerator
    {
        private long[] ChargerIds = new long[] { 4000001, 4000002 };
        private short[] ConnectorIds = new short[] { 1, 2 };
        private int TotalEnergy = 0;

        private Random rnd = new Random();

        public MeterValue CreateMeterValue()
        {
            TotalEnergy++;

            return new MeterValue()
            {
                ChargerId = ChargerIds[rnd.Next(2)],
                ConnectorId = ConnectorIds[rnd.Next(2)],
                Energy = TotalEnergy,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
