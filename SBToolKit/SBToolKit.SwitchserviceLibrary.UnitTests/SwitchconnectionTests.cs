using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwitchServiceLibrary;
using System.Threading.Tasks;

namespace SBToolKit.SwitchserviceLibrary.UnitTests
{
    [TestClass]
    public class SwitchconnectionTests
    {
        [TestMethod]
        public void LoginAsync_UsernameOrPasswordIsWrong_LoggedInIsFalse()
        {
            Switchconnection connection = new Switchconnection();
            Task.Run(async () =>
            {
                await connection.LoginAsync("bad username", "bad password");
            }).GetAwaiter().GetResult();
            Assert.IsTrue(connection.LoggedIn);
        }
    }
}
