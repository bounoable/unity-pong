using GameNet.Messages;

namespace Pong.Network.Messages
{
    class Challenge: AcknowledgeRequest
    {
        public int ChallengerId { get; }
        public int ChallengedId { get; }

        public Challenge(int challengerId, int challengedId, string ackToken = null): base(ackToken)
        {
            ChallengerId = challengerId;
            ChallengedId = challengedId;
        }
    }
}