using GameNet.Messages;

namespace Pong.Network.Messages
{
    class PlayerDisconnected: AcknowledgeRequest
    {
        public int Id { get; }

        public PlayerDisconnected(int id, string ackToken = null): base(ackToken)
        {
            Id = id;
        }
    }
}