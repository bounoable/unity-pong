using GameNet.Messages;

namespace Pong.Network.Messages
{
    class ChallengePlayer: AcknowledgeRequest
    {
        public int ChallengedId { get; }
        public string Secret { get; }

        public ChallengePlayer(int challengedId, string secret, string ackToken = null): base(ackToken)
        {
            ChallengedId = challengedId;
            Secret = secret;
        }
    }
}