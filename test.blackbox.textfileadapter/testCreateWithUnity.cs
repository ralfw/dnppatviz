using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using dnppv.contracts.fileadapter;

namespace test.blackbox.textfileadapter
{
    [TestFixture]
    public class testCreateWithUnity
    {
        [Test]
        public void testCreate()
        {
            ralfw.Unity.ContainerProvider.Clear();
            ralfw.Unity.ContainerProvider.Configure();

            IFileAdapter fa = ralfw.Unity.ContainerProvider.Get().Get<IFileAdapter>("raw");
            Assert.IsNotNull(fa);
        }
    }
}
