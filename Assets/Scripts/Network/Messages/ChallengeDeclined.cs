using GameNet.Messages;

namespace Pong.Network.Messages
{
    class ChallengeDeclined: AcknowledgeRequest
    {
        public ChallengeDeclined(string ackToken = null): base(ackToken)
        {}
    }
}