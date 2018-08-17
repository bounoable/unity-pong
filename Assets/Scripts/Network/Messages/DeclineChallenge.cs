using GameNet.Messages;

namespace Pong.Network.Messages
{
    class DeclineChallenge: AcknowledgeRequest
    {
        public string Secret { get; }

        public DeclineChallenge(string secret, string ackToken = null): base(ackToken)
        {
            Secret = secret;
        }
    }
}