using UnityEngine;
using GameNet.Messages;
using Pong.Network.Messages;

namespace Pong.Network.Serializers
{
    class GameStateUpdateSerializer: ObjectSerializer<GameStateUpdate>
    {
        override public byte[] GetBytes(GameStateUpdate message)
            => Build()
                .Float(message.Player1Position.x).Float(message.Player1Position.y)
                .Float(message.Player2Position.x).Float(message.Player2Position.y)
                .Int(message.Player1Points)
                .Int(message.Player2Points)
                .Data;
        
        override public GameStateUpdate GetObject(byte[] data) => new GameStateUpdate(
            new Vector2(PullFloat(ref data), PullFloat(ref data)),
            new Vector2(PullFloat(ref data), PullFloat(ref data)),
            PullInt(ref data),
            PullInt(ref data)
        );
    }
}