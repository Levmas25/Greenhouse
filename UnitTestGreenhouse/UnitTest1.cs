using GreenHouse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestGreenhouse
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAPI()
        {
            Assert.IsNotNull(new AirsPage().GetRequest(1));
        }
    }
}
