using EvMeterValueManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace EvMeterValueManager.Data
{
    public sealed class MeterValueCache
    {
        static MeterValueCache instance = null;
        static readonly object padlock = new object();

        public static MeterValueCache Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new MeterValueCache();
                    }
                    return instance;
                }
            }
        }

        System.Runtime.Caching.MemoryCache cache { get; set; }

        private MeterValueCache()
        {
            cache = new System.Runtime.Caching.MemoryCache("MyTestCache");

            cache["MeterValues"] = new List<MeterValue>();
        }

        public void StoreMeterValue(MeterValue meterValue)
        {
            List<MeterValue> meterValues = (List<MeterValue>)cache["MeterValues"];
            meterValues.Add(meterValue);
            cache["MeterValues"] = meterValues;
        }

        public List<MeterValue> RetrieveMeterValues()
        {
            var list = (List<MeterValue>)cache["MeterValues"];
            return list;
        }

        public void RemoveMeterValue(MeterValue meterValue)
        {
            List<MeterValue> meterValues = (List<MeterValue>)cache["MeterValues"];
            meterValues.Remove(meterValue);
            cache["MeterValues"] = meterValues;
        }
    }

}
