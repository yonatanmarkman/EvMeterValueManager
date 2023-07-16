using EvMeterValueManager.Data;
using EvMeterValueManager.Models;
using System.ComponentModel;

namespace Testing
{
    [TestClass]
    public class CacheTests
    {
        public MeterValueCache cache;
        public MeterValue testObject;

        public CacheTests()
        {
            cache = MeterValueCache.Instance;
            testObject = new MeterValue()
            {
                ChargerId = 1,
                ConnectorId = 1,
                Energy = 1,
                Timestamp = DateTime.Now
            };
        }

        [TestMethod]
        public void TestCacheAddition()
        {
            cache.StoreMeterValue(testObject);
            List<MeterValue> cachedObjects = cache.RetrieveMeterValues();

            Assert.IsNotNull(cachedObjects);
            Assert.IsTrue(cachedObjects.Contains(testObject));
        }

        [TestMethod]
        public void TestCacheRemoval()
        {
            cache.StoreMeterValue(testObject);
            cache.RemoveMeterValue(testObject);
            List<MeterValue> cachedObjects = cache.RetrieveMeterValues();

            Assert.IsNotNull(cachedObjects);
            Assert.IsTrue(!cachedObjects.Contains(testObject));
        }
    }
}