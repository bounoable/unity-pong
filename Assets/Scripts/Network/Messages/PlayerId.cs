using GameNet.Messages;

namespace Pong.Network.Messages
{
    class PlayerId: AcknowledgeRequest
    {
        public int Id { get; }
        
        public PlayerId(int id, string ackToken = null): base(ackToken)
        {
            Id = id;
        }
    }
}