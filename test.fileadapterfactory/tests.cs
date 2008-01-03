using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

namespace test.fileadapterfactory
{
    [TestFixture]
    public class tests
    {
        [Test]
        public void testConfig()
        {
            System.Configuration.Configuration c;
            c = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
            Assert.IsNotNull(c);
        }
    }
}
