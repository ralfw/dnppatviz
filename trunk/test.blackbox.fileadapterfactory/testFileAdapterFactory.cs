using System;
using System.Collections.Generic;
using System.Text;

using System.Configuration;

using NUnit.Framework;

using dnppv.contracts.fileadapter;
using dnppv.fileadapterfactory;


namespace test.blackbox.fileadapterfactory
{
    [TestFixture]
    public class testFileAdapterFactory
    {
        [Test]
        public void testExtensions()
        {
            ralfw.Microkernel.DynamicBinder.ClearBindings();
            ralfw.Microkernel.DynamicBinder.LoadBindings();

            IFileAdapterFactory faf;
            faf = new FileAdapterFactory();
            string[] ext = faf.FileExtensionsSupported;
            Assert.AreEqual(2, ext.Length);
            Assert.AreEqual("txt", ext[0]);
            Assert.AreEqual("mid", ext[1]);
        }


        [Test]
        public void testCreate()
        {
            ralfw.Microkernel.DynamicBinder.ClearBindings();
            ralfw.Microkernel.DynamicBinder.LoadBindings();

            IFileAdapterFactory faf;
            faf = new FileAdapterFactory();

            IFileAdapter fa;
            fa = faf.CreateFileAdapter("test.txt");
            Assert.IsInstanceOfType(typeof(MockupFileAdapterA), fa);
            Assert.AreEqual("test.txt", fa.Filename);

            fa = faf.CreateFileAdapter("test.mid");
            Assert.IsInstanceOfType(typeof(MockupFileAdapterB), fa);
            Assert.AreEqual("test.mid", fa.Filename);
        }
    }
}
