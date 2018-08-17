using GameNet.Messages;

namespace Pong.Network.Messages
{
    class AcceptChallenge: AcknowledgeRequest
    {
        public string Secret { get; }

        public AcceptChallenge(string secret, string ackToken = null): base(ackToken)
        {
            Secret = secret;
        }
    }
}