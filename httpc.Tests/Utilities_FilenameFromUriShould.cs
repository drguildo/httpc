namespace httpc.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Utilities_FilenameFromUriShould
    {
        [TestMethod]
        public void ThrowArgumentExceptionOnNull()
        {
            Assert.ThrowsException<ArgumentException>(() => Utilities.FilenameFromUri(null));
        }
    }
}
