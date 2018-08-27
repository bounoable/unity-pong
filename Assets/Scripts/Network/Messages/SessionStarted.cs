using GameNet.Messages;

namespace Pong.Network.Messages
{
    class SessionStarted: AcknowledgeRequest
    {
        public int SessionId { get; }
        public int ChallengerId { get; }
        public int ChallengedId { get; }

        public SessionStarted(int sessionId, int challengerId, int challengedId, string ackToken = null): base(ackToken)
        {
            SessionId = sessionId;
            ChallengerId = challengerId;
            ChallengedId = challengedId;
        }
    }
}