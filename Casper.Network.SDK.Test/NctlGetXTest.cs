using System.Text.Json;
using System.Threading.Tasks;
using Casper.Network.SDK.JsonRpc;
using Casper.Network.SDK.Types;
using Casper.Network.SDK.Utils;
using NUnit.Framework;

namespace NetCasperTest
{
    [Category("NCTL")]
    public class NctlGetXTest : NctlBase
    {
        [Test]
        public async Task GetNodeStatusTest()
        {
            var rpcResponse = await _client.GetNodeStatus();
            Assert.IsNotEmpty(rpcResponse.Result.GetRawText());
            Assert.AreEqual("2.0", rpcResponse.JsonRpc);
            Assert.AreNotEqual(0, rpcResponse.Id);
            
            var nodeStatus = rpcResponse.Parse();
            Assert.IsNotEmpty(nodeStatus.ApiVersion);

            // only put deploy returns a deploy hash. catch exception for other cases
            //
            var ex = Assert.Catch<RpcClientException>(() => rpcResponse.GetDeployHash());
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.Contains("deploy_hash property not found."));
        }
        
        [Test]
        public async Task GetNodePeersTest()
        {
            var rpcResponse = await _client.GetNodePeers();
            Assert.IsNotEmpty(rpcResponse.Result.GetRawText());

            var nodePeers = rpcResponse.Parse();
            Assert.IsNotEmpty(nodePeers.ApiVersion);
            Assert.IsTrue(nodePeers.Peers.Count > 0);
            Assert.IsNotEmpty(nodePeers.Peers[0].Address);
            Assert.IsNotEmpty(nodePeers.Peers[0].NodeId);
        }

        [Test]
        public async Task GetAccountTest()
        {
            try
            {
                var response = await _client.GetAccountInfo(_faucetKey.PublicKey, 1);
                var accountInfo = response.Parse();
                Assert.IsNotEmpty(accountInfo.Account.AccountHash.ToString());
                
                var response2 = await _client.GetAccountBalance(_faucetKey.PublicKey);
                var accountBalance = response2.Parse();
                Assert.IsTrue(accountBalance.BalanceValue > 0);

                var response3 = await _client.GetAccountBalance(accountInfo.Account.MainPurse);
                var accountBalance2 = response3.Parse();
                Assert.AreEqual(accountBalance.BalanceValue, accountBalance2.BalanceValue);
            }
            catch (RpcClientException e)
            {
                Assert.Fail(e.RpcError.Message);
            }

            try
            {
                var key1 = KeyPair.CreateNew(KeyAlgo.ED25519);

                await _client.GetAccountInfo(key1.PublicKey);
                Assert.Fail("Exception expected");
            }
            catch (RpcClientException e)
            {
                Assert.IsNotNull(e.RpcError);
                Assert.IsNotNull(e.RpcError.Message);
            }
            
            try
            {
                var key1 = KeyPair.CreateNew(KeyAlgo.ED25519);

                await _client.GetAccountBalance(key1.PublicKey);
                Assert.Fail("Exception expected");
            }
            catch (RpcClientException e)
            {
                Assert.IsNotNull(e.RpcError);
                Assert.IsNotNull(e.RpcError.Message);
            }
        }

        [Test]
        public async Task GetBlockTest()
        {
            try
            {
                var response = await _client.GetBlock();
                var result = response.Parse();
                Assert.IsNotNull(result.Block.Hash);

                var response2 = await _client.GetBlock(1);
                var result2 = response2.Parse();
                Assert.IsNotNull(result2.Block.Hash);

                var response3 = await _client.GetBlock(result2.Block.Hash);
                var result3 = response3.Parse();
                Assert.AreEqual(result2.Block.Hash, result3.Block.Hash);

                var response4 = await _client.GetBlock((int)result2.Block.Header.Height);
                var result4 = response4.Parse();
                Assert.AreEqual(result2.Block.Hash, result4.Block.Hash);

                var response5 = await _client.GetBlockTransfers(result2.Block.Hash);
                var result5 = response5.Parse();
                Assert.AreEqual(0, result5.Transfers.Count);
                
                var response6 = await _client.GetBlockTransfers((int)result2.Block.Header.Height);
                var result6 = response6.Parse();
                Assert.AreEqual(0, result6.Transfers.Count);
                
                var hash1 = await _client.GetStateRootHash(result2.Block.Hash);
                Assert.AreEqual(32*2, hash1.Length);

                var hash2 = await _client.GetStateRootHash((int)result2.Block.Header.Height);
                Assert.AreEqual(hash1, hash2);
                
                var hash3 = await _client.GetStateRootHash();
                Assert.AreEqual(32*2, hash3.Length);
            }
            catch (RpcClientException e)
            {
                Assert.Fail(e.RpcError.Message);
            }
            
            try
            {
                await _client.GetBlock(100000);
                Assert.Fail("Exception expected");
            }
            catch (RpcClientException e)
            {
                Assert.IsNotNull(e.RpcError);
                Assert.IsNotNull(e.RpcError.Message);
                Assert.AreNotEqual(0, e.RpcError.Code);
                Assert.IsNotNull(e.RpcError.Data);
            }
        }

        [Test]
        public async Task GetAuctionInfoTest()
        {
            try
            {
                var response = await _client.GetAuctionInfo();
                var auctionInfo = response.Parse();
                Assert.IsTrue(auctionInfo.AuctionState.Bids.Count > 0);

                var response2 = await _client.GetAuctionInfo(1);
                var auctionInfo2 = response2.Parse();
                Assert.IsTrue(auctionInfo2.AuctionState.Bids.Count > 0);
                
                var response3 = await _client.GetEraInfoBySwitchBlock(2);
                var eraInfo3 = response3.Parse();
                Assert.IsNotNull(eraInfo3);

                var response4 = await _client.GetEraInfoBySwitchBlock();
                var eraInfo4 = response4.Parse();
                Assert.IsNotNull(eraInfo4);
            }
            catch (RpcClientException e)
            {
                Assert.Fail(e.RpcError.Message);
            }
        }
        
        [Test]
        public async Task GetValidatorChangesTest()
        {
            try
            {
                var response = await _client.GetValidatorChanges();
                var changes = response.Parse();
                Assert.IsNotNull(changes.Changes);
            }
            catch (RpcClientException e)
            {
                Assert.Fail(e.RpcError.Message);
            }
        }
        
        [Test]
        public async Task GetRpcSchemaTest()
        {
            try
            {
                var schema = await _client.GetRpcSchema();
                Assert.IsNotEmpty(schema);

                var doc = JsonDocument.Parse(schema);
                Assert.IsNotNull(doc);
            }
            catch (RpcClientException e)
            {
                Assert.Fail(e.RpcError.Message);
            }
        }
    }
}