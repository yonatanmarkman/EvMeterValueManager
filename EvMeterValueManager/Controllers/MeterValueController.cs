using EvMeterValueManager.Data;
using EvMeterValueManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Numerics;

namespace EvMeterValueManager.Controllers
{
    [ApiController]
    [Route("MeterValueController")]
    public class MeterValueController : Controller
    {
        public MeterValueController()
        {

        }

        // Send a new MeterValue to the system
        [HttpPost("ReceiveMeterValue")]
        public IActionResult ReceiveMeterValue(MeterValue meterValue)
        {
            try
            {
                MeterValueCache.Instance.StoreMeterValue(meterValue);
            }
            catch (Exception ex)
            {
                return BadRequest("Exception occurred: " + ex.Message);
            }
            return Ok("MeterValue added successfully. ");
        }

        // Get all stored MeterValues
        [HttpGet("MeterValues")]
        public IList<MeterValue> GetAllMeterValues()
        {
            IList<MeterValue> meterValues = new List<MeterValue>();
            meterValues = MeterValueCache.Instance.RetrieveMeterValues();
            return meterValues;
        }

        // Get all stored MeterValues with suggestion on how to fix anomaly
        [HttpGet("MeterValuesWithAnomalies")]
        public IList<MeterValueWithAnomaly> GetAllMeterValuesWithAnomalies()
        {
            IList<MeterValueWithAnomaly> meterValuesWithAnomalies = new List<MeterValueWithAnomaly>();
            IList<MeterValue> meterValues = MeterValueCache.Instance.RetrieveMeterValues();

            decimal sumOfDifferences = 0;

            for (int i = 1; i < meterValues.Count; i++)
            {
                sumOfDifferences += (meterValues[i].Energy - meterValues[i - 1].Energy);
                
                double timeDiff = Math.Floor((meterValues[i].Timestamp - meterValues[i - 1].Timestamp).TotalSeconds);
                
                if (timeDiff > EvMeterWorker.SECONDS_DELAY)
                {
                    decimal correctionValue = sumOfDifferences / i;

                    MeterValueWithAnomaly mvWithAnomaly = new MeterValueWithAnomaly()
                    {
                        ChargerId = meterValues[i].ChargerId,
                        Energy = meterValues[i].Energy + correctionValue,
                        ConnectorId = meterValues[i].ConnectorId,
                        Timestamp = meterValues[i].Timestamp,
                        EnergyCorrectionValue = correctionValue
                    };

                    meterValuesWithAnomalies.Add(mvWithAnomaly);
                }
            }

            return meterValuesWithAnomalies;
        }
    }
}
