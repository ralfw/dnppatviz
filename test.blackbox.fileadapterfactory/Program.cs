using System;
using System.Collections.Generic;
using System.Text;

// needed to make tests a console app because NUnit and Testrunner had problems with the app.config
// with its custom sections

namespace test.blackbox.fileadapterfactory
{
    public class Program
    {
        public static void Main()
        {
            testFileAdapterFactory t = new testFileAdapterFactory();
            t.testExtensions();
            t.testCreate();
        }
    }
}
