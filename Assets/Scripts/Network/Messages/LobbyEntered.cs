using GameNet.Messages;

namespace Pong.Network.Messages
{
    class LobbyEntered: AcknowledgeRequest
    {
        public string Secret { get; }

        public LobbyEntered(string secret, string ackToken = null): base(ackToken)
        {
            Secret = secret;
        }
    }
}