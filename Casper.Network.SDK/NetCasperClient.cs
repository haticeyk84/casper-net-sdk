using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Casper.Network.SDK.JsonRpc;
using Casper.Network.SDK.JsonRpc.ResultTypes;
using Casper.Network.SDK.Types;

namespace Casper.Network.SDK
{
    /// <summary>
    /// Client to communicate with a Casper node.
    /// </summary>
    public class NetCasperClient : IDisposable
    {
        private volatile bool _disposed;

        private readonly RpcClient _rpcClient;

        /// <summary>
        /// Create a new instance of the Casper client for a specific node in the network determined
        /// by the node address. Optionally, indicate a logging handler to log the requests and responses
        /// exchanged with tne node.
        /// </summary>
        /// <param name="nodeAddress">URL of the node. Example: 'http://127.0.0.1:7777/rpc'.</param>
        /// <param name="loggingHandler">Optional. An instance of a logging handler to log the requests
        /// and responses exchanged with the network.</param>
        public NetCasperClient(string nodeAddress, RpcLoggingHandler loggingHandler = null)
        {
            _rpcClient = new RpcClient(nodeAddress, loggingHandler);
        }
        
        public NetCasperClient(string nodeAddress, HttpClient httpClient)
        {
            _rpcClient = new RpcClient(nodeAddress, httpClient);
        }

        private Task<RpcResponse<TRpcResult>> SendRpcRequestAsync<TRpcResult>(RpcMethod method)
        {
            return _rpcClient.SendRpcRequestAsync<TRpcResult>(method);
        }

        /// <summary>
        /// Request the state root hash at a given Block.
        /// </summary>
        /// <param name="blockHash">Block hash for which the state root is queried. Null for the most recent.</param>
        /// <returns></returns>
        public async Task<string> GetStateRootHash(string blockHash = null)
        {
            var method = new GetStateRootHash(blockHash);
            var rpcResponse = await SendRpcRequestAsync<GetStateRootHashResult>(method);
            return rpcResponse.Result.GetProperty("state_root_hash").GetString();
        }

        /// <summary>
        /// Request the state root hash at a given Block.
        /// </summary>
        /// <param name="blockHeight">Block height for which the state root is queried.</param>
        /// <returns></returns>
        public async Task<string> GetStateRootHash(int blockHeight)
        {
            var method = new GetStateRootHash(blockHeight);
            var rpcResponse = await SendRpcRequestAsync<GetStateRootHashResult>(method);
            return rpcResponse.Result.GetProperty("state_root_hash").GetString();
        }

        /// <summary>
        /// Request the current status of the node. 
        /// </summary>
        public async Task<RpcResponse<GetNodeStatusResult>> GetNodeStatus()
        {
            var method = new GetNodeStatus();
            return await SendRpcRequestAsync<GetNodeStatusResult>(method);
        }

        /// <summary>
        /// Request a list of peers connected to the node.
        /// </summary>
        public async Task<RpcResponse<GetNodePeersResult>> GetNodePeers()
        {
            var method = new GetNodePeers();
            return await SendRpcRequestAsync<GetNodePeersResult>(method);
        }

        /// <summary>
        /// Request the bids and validators at a given block.
        /// </summary>
        /// <param name="blockHash">Block hash for which the auction info is queried. Null for the most recent auction info.</param>
        /// <returns></returns>
        public async Task<RpcResponse<GetAuctionInfoResult>> GetAuctionInfo(string blockHash = null)
        {
            var method = new GetAuctionInfo(blockHash);
            return await SendRpcRequestAsync<GetAuctionInfoResult>(method);
        }

        /// <summary>
        /// Request the bids and validators at a given block. 
        /// </summary>
        /// <param name="blockHeight">Block height for which the auction info is queried.</param>
        public async Task<RpcResponse<GetAuctionInfoResult>> GetAuctionInfo(int blockHeight)
        {
            var method = new GetAuctionInfo(blockHeight);
            return await SendRpcRequestAsync<GetAuctionInfoResult>(method);
        }

        /// <summary>
        /// Request the information of an Account in the network.
        /// </summary>
        /// <param name="publicKey">The public key of the account.</param>
        /// <param name="blockHash">A block hash for which the information of the account is queried. Null for most recent information.</param>
        public async Task<RpcResponse<GetAccountInfoResult>> GetAccountInfo(PublicKey publicKey,
            string blockHash = null)
        {
            var method = new GetAccountInfo(publicKey.ToAccountHex(), blockHash);
            return await SendRpcRequestAsync<GetAccountInfoResult>(method);
        }

        /// <summary>
        /// Request the information of an Account in the network.
        /// </summary>
        /// <param name="publicKey">The public key of the account formatted as a string.</param>
        /// <param name="blockHash">A block hash for which the information of the account is queried. Null for most recent information.</param>
        public async Task<RpcResponse<GetAccountInfoResult>> GetAccountInfo(string publicKey, 
            string blockHash = null)
        {
            var method = new GetAccountInfo(publicKey, blockHash);
            return await SendRpcRequestAsync<GetAccountInfoResult>(method);
        }

        /// <summary>
        /// Request the information of an Account in the network.
        /// </summary>
        /// <param name="publicKey">The public key of the account.</param>
        /// <param name="blockHeight">A block height for which the information of the account is queried.</param>
        public async Task<RpcResponse<GetAccountInfoResult>> GetAccountInfo(PublicKey publicKey, int blockHeight)
        {
            var method = new GetAccountInfo(publicKey.ToAccountHex(), blockHeight);
            return await SendRpcRequestAsync<GetAccountInfoResult>(method);
        }

        /// <summary>
        /// Request the information of an Account in the network.
        /// </summary>
        /// <param name="publicKey">The public key of the account formatted as an hex-string.</param>
        /// <param name="blockHeight">A block height for which the information of the account is queried.</param>
        public async Task<RpcResponse<GetAccountInfoResult>> GetAccountInfo(string publicKey, int blockHeight)
        {
            var method = new GetAccountInfo(publicKey, blockHeight);
            return await SendRpcRequestAsync<GetAccountInfoResult>(method);
        }

        /// <summary>
        /// Request a stored value from the network. This RPC is deprecated, use `QueryGlobalState` instead.
        /// </summary>
        /// <param name="keyHash">A global state key formatted as a string</param>
        /// <param name="path">The path components starting from the key as base (use '/' as separator).</param>
        /// <param name="stateRootHash"></param>
        /// <returns></returns>
        [Obsolete("Use QueryGlobalState() to retrieve stored values from the network.", false)]
        public async Task<RpcResponse<GetItemResult>> QueryState(string keyHash, List<string> path = null,
            string stateRootHash = null)
        {
            if (stateRootHash == null)
                stateRootHash = await GetStateRootHash();

            var method = new GetItem(keyHash, stateRootHash, path);
            return await SendRpcRequestAsync<GetItemResult>(method);
        }

        /// <summary>
        /// Request the stored value in a global state key.
        /// </summary>
        /// <param name="key">The global state key formatted as a string to query the value from the network.</param>
        /// <param name="stateRootHash">Hash of the state root. Null for the most recent stored value..</param>
        /// <param name="path">The path components starting from the key as base (use '/' as separator).</param>
        public async Task<RpcResponse<QueryGlobalStateResult>> QueryGlobalState(string key, string stateRootHash = null,
            string path = null)
        {            
            if (stateRootHash == null)
                stateRootHash = await GetStateRootHash();

            var method = new QueryGlobalState(key, stateRootHash, isBlockHash: false, path?.Split(new char[] {'/'}));
            return await SendRpcRequestAsync<QueryGlobalStateResult>(method);
        }

        /// <summary>
        /// Request the stored value in a global state key.
        /// </summary>
        /// <param name="key">The global state key to query the value from the network.</param>
        /// <param name="stateRootHash">Hash of the state root. Null for the most recent stored value..</param>
        /// <param name="path">The path components starting from the key as base (use '/' as separator).</param>
        public async Task<RpcResponse<QueryGlobalStateResult>> QueryGlobalState(GlobalStateKey key, string stateRootHash = null,
            string path = null)
        {
            return await QueryGlobalState(key.ToString(), stateRootHash, path);
        }
        
        /// <summary>
        /// Request the stored value in a global state key.
        /// </summary>
        /// <param name="key">The global state key formatted as a string to query the value from the network.</param>
        /// <param name="blockHash">The block hash.</param>
        /// <param name="path">The path components starting from the key as base (use '/' as separator).</param>
        public async Task<RpcResponse<QueryGlobalStateResult>> QueryGlobalStateWithBlockHash(string key, string blockHash,
            string path = null)
        {
            var method = new QueryGlobalState(key, blockHash, isBlockHash: true, path?.Split(new char[] {'/'}));
            return await SendRpcRequestAsync<QueryGlobalStateResult>(method);
        }

        /// <summary>
        /// Request the stored value in a global state key.
        /// </summary>
        /// <param name="key">The global state key to query the value from the network.</param>
        /// <param name="blockHash">The block hash.</param>
        /// <param name="path">The path components starting from the key as base (use '/' as separator).</param>
        public async Task<RpcResponse<QueryGlobalStateResult>> QueryGlobalStateWithBlockHash(GlobalStateKey key,
            string blockHash, string path = null)
        {
            return await QueryGlobalStateWithBlockHash(key.ToString(), blockHash, path);
        }
        
        /// <summary>
        /// Request a purse's balance from the network.
        /// </summary>
        /// <param name="purseURef">Purse URef formatted as a string.</param>
        /// <param name="stateRootHash">Hash of the state root.</param>
        public async Task<RpcResponse<GetBalanceResult>> GetAccountBalance(string purseURef,
            string stateRootHash = null)
        {
            if (!purseURef.StartsWith("uref-"))
            {
                var response = await GetAccountInfo(purseURef);
                purseURef = response.Result.GetProperty("account")
                    .GetProperty("main_purse").GetString();
            }

            if (stateRootHash == null)
                stateRootHash = await GetStateRootHash();

            var method = new GetBalance(purseURef, stateRootHash);
            return await SendRpcRequestAsync<GetBalanceResult>(method);
        }

        /// <summary>
        /// Request a purse's balance from the network.
        /// </summary>
        /// <param name="purseURef">Purse URef key.</param>
        /// <param name="stateRootHash">Hash of the state root.</param>
        public async Task<RpcResponse<GetBalanceResult>> GetAccountBalance(URef purseURef,
            string stateRootHash = null)
        {
            return await GetAccountBalance(purseURef.ToString(), stateRootHash);
        }

        /// <summary>
        /// Request the balance information of an account given its public key.
        /// </summary>
        /// <param name="publicKey">The public key of the account to request the balance.</param>
        /// <param name="stateRootHash">Hash of the state root.</param>
        public async Task<RpcResponse<GetBalanceResult>> GetAccountBalance(PublicKey publicKey,
            string stateRootHash = null)
        {
            var response = await GetAccountInfo(publicKey);
            var purseUref = response.Result.GetProperty("account")
                .GetProperty("main_purse").GetString();
            return GetAccountBalance(purseUref, stateRootHash).Result;
        }
        
        /// <summary>
        /// Send a Deploy to the network for its execution.
        /// </summary>
        /// <param name="deploy">The deploy object.</param>
        /// <exception cref="System.Exception">Throws an exception if the deploy is not signed.</exception>
        public async Task<RpcResponse<PutDeployResult>> PutDeploy(Deploy deploy)
        {
            if (deploy.Approvals.Count == 0)
                throw new Exception("Sign the deploy before sending it to the network.");

            var method = new PutDeploy(deploy);
            return await SendRpcRequestAsync<PutDeployResult>(method);
        }

        
        /// <summary>
        /// Request a Deploy object from the network by the deploy hash.
        /// When a cancellation token is included this method waits until the deploy is
        /// executed, i.e. until the deploy contains the execution results information.
        /// </summary>
        /// <param name="deployHash">Hash of the deploy to retrieve.</param>
        /// <param name="cancellationToken">A CancellationToken. Do not include this parameter to return
        /// with the first deploy object returned by the network, even it's not executed.</param>
        /// <exception cref="TaskCanceledException">The token has cancelled the operation before the deploy has been executed.</exception>
        public async Task<RpcResponse<GetDeployResult>> GetDeploy(string deployHash,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var method = new GetDeploy(deployHash);

            while (!cancellationToken.IsCancellationRequested)
            {
                var response = await SendRpcRequestAsync<GetDeployResult>(method);
                if (!cancellationToken.CanBeCanceled ||
                    response.Result.GetProperty("execution_results").GetArrayLength() > 0)
                    return response;
                Thread.Sleep(10000);
            }

            throw new TaskCanceledException("GetDeploy operation canceled");
        }

        /// <summary>
        /// Retrieves a Block from the network by its hash. 
        /// </summary>
        /// <param name="blockHash">Hash of the block to retrieve. Null for the most recent block.</param>
        public async Task<RpcResponse<GetBlockResult>> GetBlock(string blockHash = null)
        {
            var method = new GetBlock(blockHash);
            return await SendRpcRequestAsync<GetBlockResult>(method);
        }

        /// <summary>
        /// Request a Block from the network by its height number.
        /// </summary>
        /// <param name="blockHeight">Height of the block to retrieve.</param>
        public async Task<RpcResponse<GetBlockResult>> GetBlock(int blockHeight)
        {
            var method = new GetBlock(blockHeight);
            return await SendRpcRequestAsync<GetBlockResult>(method);
        }

        /// <summary>
        /// Request all transfers for a Block by its block hash.
        /// </summary>
        /// <param name="blockHash">Hash of the block to retrieve the transfers from. Null for the most recent block</param>
        public async Task<RpcResponse<GetBlockTransfersResult>> GetBlockTransfers(string blockHash = null)
        {
            var method = new GetBlockTransfers(blockHash);
            return await SendRpcRequestAsync<GetBlockTransfersResult>(method);
        }

        /// <summary>
        /// Request all transfers for a Block by its height number.
        /// </summary>
        /// <param name="blockHeight">Height of the block to retrieve the transfers from.</param>
        public async Task<RpcResponse<GetBlockTransfersResult>> GetBlockTransfers(int blockHeight)
        {
            var method = new GetBlockTransfers(blockHeight);
            return await SendRpcRequestAsync<GetBlockTransfersResult>(method);
        }

        /// <summary>
        /// Request an EraInfo from the network given a switch block.
        /// For a non-switch block this method returns an empty response.
        /// </summary>
        /// <param name="blockHash">Block hash of a switch block. Null for the latest block.</param>
        public async Task<RpcResponse<GetEraInfoBySwitchBlockResult>> GetEraInfoBySwitchBlock(string blockHash = null)
        {
            var method = new GetEraInfoBySwitchBlock(blockHash);
            return await SendRpcRequestAsync<GetEraInfoBySwitchBlockResult>(method);
        }

        /// <summary>
        /// Request an EraInfo from the network given a switch block.
        /// For a non-switch block this method returns an empty response.
        /// </summary>
        /// <param name="blockHeight">Block height of a switch block.</param>
        public async Task<RpcResponse<GetEraInfoBySwitchBlockResult>> GetEraInfoBySwitchBlock(int blockHeight)
        {
            var method = new GetEraInfoBySwitchBlock(blockHeight);
            return await SendRpcRequestAsync<GetEraInfoBySwitchBlockResult>(method);
        }

        /// <summary>
        /// Lookup a dictionary item from its dictionary item key.
        /// </summary>
        /// <param name="dictionaryItem">The dictionary item key to retrieve.</param>
        /// <param name="stateRootHash">Hash of the state root.</param>
        public async Task<RpcResponse<GetDictionaryItemResult>> GetDictionaryItem(string dictionaryItem, string stateRootHash = null)
        {
            if(stateRootHash == null)
                stateRootHash = await GetStateRootHash();

            var method = new GetDictionaryItem(dictionaryItem, stateRootHash);
            return await SendRpcRequestAsync<GetDictionaryItemResult>(method);
        }

        /// <summary>
        /// Lookup a dictionary item via an Account's named keys.
        /// </summary>
        /// <param name="accountKey">The account key as a formatted string whose named keys contains dictionaryName.</param>
        /// <param name="dictionaryName">The named key under which the dictionary seed URef is stored.</param>
        /// <param name="dictionaryItem">The dictionary item key.</param>
        /// <param name="stateRootHash">Hash of the state root.</param>
        public async Task<RpcResponse<GetDictionaryItemResult>> GetDictionaryItemByAccount(string accountKey, string dictionaryName, 
            string dictionaryItem, string stateRootHash = null)
        {
            if(stateRootHash == null)
                stateRootHash = await GetStateRootHash();

            var method = new GetDictionaryItemByAccount(accountKey, dictionaryName, dictionaryItem, stateRootHash);
            return await SendRpcRequestAsync<GetDictionaryItemResult>(method);
        }
        
        /// <summary>
        /// Lookup a dictionary item via a Contract named keys.
        /// </summary>
        /// <param name="contractKey">The contract key as a formatted string whose named keys contains dictionaryName.</param>
        /// <param name="dictionaryName">The named key under which the dictionary seed URef is stored.</param>
        /// <param name="dictionaryItem">The dictionary item key.</param>
        /// <param name="stateRootHash">Hash of the state root.</param>
        public async Task<RpcResponse<GetDictionaryItemResult>> GetDictionaryItemByContract(string contractKey, string dictionaryName, 
            string dictionaryItem, string stateRootHash = null)
        {
            if(stateRootHash == null)
                stateRootHash = await GetStateRootHash();

            var method = new GetDictionaryItemByContract(contractKey, dictionaryName, dictionaryItem, stateRootHash);
            return await SendRpcRequestAsync<GetDictionaryItemResult>(method);
        }
        
        /// <summary>
        /// Lookup a dictionary item via its seed URef.
        /// </summary>
        /// <param name="seedURef">The dictionary's seed URef.</param>
        /// <param name="dictionaryItem">The dictionary item key.</param>
        /// <param name="stateRootHash">Hash of the state root.</param>
        /// <returns></returns>
        public async Task<RpcResponse<GetDictionaryItemResult>> GetDictionaryItemByURef(string seedURef, 
            string dictionaryItem, string stateRootHash = null)
        {
            if(stateRootHash == null)
                stateRootHash = await GetStateRootHash();

            var method = new GetDictionaryItemByURef(seedURef, dictionaryItem, stateRootHash);
            return await SendRpcRequestAsync<GetDictionaryItemResult>(method);
        }
        
        /// <summary>
        /// Request the status changes of active validators.
        /// </summary>
        public async Task<RpcResponse<GetValidatorChangesResult>> GetValidatorChanges()
        {
            var method = new GetValidatorChanges();
            return await SendRpcRequestAsync<GetValidatorChangesResult>(method);
        }

        /// <summary>
        /// Request the RPC Json schema to the network node.
        /// </summary>
        public async Task<string> GetRpcSchema()
        {
            var method = new GetRpcSchema();
            var response = await SendRpcRequestAsync<RpcResult>(method);
            return response.Result.GetRawText();
        }

        /// <summary>
        /// Request the performance metrics of a node in the network.
        /// </summary>
        /// <param name="nodeAddress">URL of the performance metrics endpoint. Example: 'http://127.0.0.1:8888/metrics'.</param>
        public static async Task<string> GetNodeMetrics(string nodeAddress)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);

            try
            {
                var metrics = new StringBuilder();

                using (var streamReader =
                    new StreamReader(await client.GetStreamAsync(nodeAddress)))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var message = await streamReader.ReadLineAsync();
                        metrics.AppendLine(message);
                    }
                }

                return metrics.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not retrieve metrics from {nodeAddress}", ex);
            }
        }

        /// <summary>
        /// Request the performance metrics of a node in the network.
        /// </summary>
        /// <param name="host">IP of the network node.</param>
        /// <param name="port">Port of the performance metrics endpoint (usually 8888).</param>
        public static async Task<string> GetNodeMetrics(string host, int port)
        {
            var uriBuilder = new UriBuilder("http", host, port, "metrics");
            return await GetNodeMetrics(uriBuilder.Uri.ToString());
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                _rpcClient.Dispose();
            }
        }
    }
}