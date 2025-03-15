using System;
using Sulfur.Contract.Helpers;
using Sulfur.Driver;

namespace sulfur.demo_test.Test;

[TestFixture]
public class DriverUsageTest
{
    private SulfurDriver _driver;

    [SetUp]
    public void Setup()
    {
        _driver = new SulfurDriver(new Logger());
    }

    [TearDown]
    public void TearDown()
    {
        _driver?.Dispose();
    }

    [Test]
    public void FondObjectTest()
    {
        SulfurObject obj = _driver.Find("//ProxySetupUI");
        Assert.That(obj.Name, Is.EqualTo("ProxySetupUI"));
    }


    private class Logger : ILogger
    {
        public void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public void Warn(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(string message)
        {
            Console.WriteLine(message);
        }
    }
}