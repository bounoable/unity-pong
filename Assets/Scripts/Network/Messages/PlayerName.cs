using GameNet.Messages;

namespace Pong.Network.Messages
{
    class PlayerName: AcknowledgeRequest
    {
        public string Name { get; }
        public string Secret { get; }

        public PlayerName(string name, string secret, string ackToken = null): base(ackToken)
        {
            Name = name;
            Secret = secret;
        }
    }
}