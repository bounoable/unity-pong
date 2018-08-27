using UnityEngine;
using GameNet.Messages;
using Pong.Network.Messages;

namespace Pong.Network.Serializers
{
    class GameStateUpdateSerializer: ObjectSerializer<GameStateUpdate>
    {
        override public byte[] GetBytes(GameStateUpdate message)
            => Build()
                .Float(message.Player1Position.x).Float(message.Player1Position.y).Float(message.Player1Position.z)
                .Float(message.Player2Position.x).Float(message.Player2Position.y).Float(message.Player2Position.z)
                .Int(message.Player1Points)
                .Int(message.Player2Points)
                .Float(message.BallPosition.x).Float(message.BallPosition.y).Float(message.BallPosition.z)
                .Data;
        
        override public GameStateUpdate GetObject(byte[] data) => new GameStateUpdate(
            new Vector3(PullFloat(ref data), PullFloat(ref data), PullFloat(ref data)),
            new Vector3(PullFloat(ref data), PullFloat(ref data), PullFloat(ref data)),
            PullInt(ref data),
            PullInt(ref data),
            new Vector3(PullFloat(ref data), PullFloat(ref data), PullFloat(ref data))
        );
    }
}