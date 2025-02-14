using System;
using Newtonsoft.Json;
using SequenceSDK.WaaS;

namespace Sequence.WaaS
{
    [Serializable]
    public class SendERC1155 : Sequence.WaaS.Transaction
    {
        public const string TypeIdentifier = "erc1155send";
        public string data { get; private set; }
        public string to { get; private set; }
        public string tokenAddress { get; private set; }
        public string type { get; private set; } = TypeIdentifier;
        public SendERC1155Values[] vals { get; private set; }

        public SendERC1155(string tokenAddress, string to, SendERC1155Values[] sendErc1155Values, string data = null)
        {
            this.tokenAddress = tokenAddress;
            this.to = to;
            this.vals = sendErc1155Values;
            this.data = data;
        }

        [JsonConstructor]
        public SendERC1155(string data, string to, string tokenAddress, string type, SendERC1155Values[] vals)
        {
            this.data = data;
            this.to = to;
            this.tokenAddress = tokenAddress;
            this.type = type;
            this.vals = vals;
        }
    }
}