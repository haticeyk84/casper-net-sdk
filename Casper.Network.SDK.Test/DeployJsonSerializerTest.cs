using System.IO;
using System.Text.Json;
using Casper.Network.SDK.Converters;
using Casper.Network.SDK.Types;
using NUnit.Framework;
using Org.BouncyCastle.Utilities.Encoders;

namespace NetCasperTest
{
    public class DeployJsonSerializer
    {
        private string deploy1 = TestContext.CurrentContext.TestDirectory + "/TestData/transfer-deploy.json";
        private string signer1 = "017b8058863aAd49c7b89C77019CEF3a4D863BDf1C0c61499776F94B18465810f7";
        private string signature1 =
            "012A0c5896aB9d6CF029268cf454BA9D42BCD40Bd909C984890F326739eEF13201D1d2A9a95938B2A966F2e650d1bdd80931f0374e3A92403E025b806AA1065109";
        
        [Test]
        public void LoadDeployFromFileTest()
        {
            var deploy = Deploy.Load(deploy1);

            Assert.IsNotNull(deploy);
            Assert.IsTrue(deploy.ValidateHashes(out string msg));
            Assert.AreEqual(string.Empty, msg);
            Assert.AreEqual(1, deploy.Approvals.Count);
            Assert.AreEqual(signer1, deploy.Approvals[0].Signer.ToAccountHex());
            Assert.AreEqual(signature1, deploy.Approvals[0].Signature.ToHexString());
        }

        [Test]
        public void ParseDeployFromJsonString()
        {
            var json = File.ReadAllText(deploy1);
            var deploy = Deploy.Parse(json);
            
            Assert.IsNotNull(deploy);
            Assert.IsTrue(deploy.ValidateHashes(out string msg));
            Assert.AreEqual(string.Empty, msg);
            Assert.AreEqual(1, deploy.Approvals.Count);
            Assert.AreEqual(signer1, deploy.Approvals[0].Signer.ToAccountHex());
            Assert.AreEqual(signature1, deploy.Approvals[0].Signature.ToHexString());
        }

        [Test]
        public void SaveDeployToFile()
        {
            var deploy = Deploy.Load(deploy1);

            var tmpFile = Path.GetTempFileName();
            deploy.Save(tmpFile);

            var deployCopy = Deploy.Load(tmpFile);
            Assert.IsTrue(deployCopy.ValidateHashes(out string msg));
            Assert.AreEqual(string.Empty, msg);
        }

        [Test]
        public void SerializeDeployToJson()
        {
            var deploy = Deploy.Load(deploy1);

            var json = deploy.SerializeToJson();

            var deployCopy = Deploy.Parse(json);
            Assert.IsTrue(deployCopy.ValidateHashes(out string msg));
            Assert.AreEqual(string.Empty, msg);
        }
    }
}