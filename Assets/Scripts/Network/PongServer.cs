using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNet;
using System.Net;

class PongServer
{
	readonly static GameNetFactory GameNetFactory = new GameNetFactory();

	readonly IPAddress _ipAddress;
	readonly ushort _tcpPort;
	readonly ushort _udpPort;
	
	Server _server;

	public PongServer(string ip, ushort tcpPort, ushort udpPort)
	{
		_ipAddress = IPAddress.Parse(ip);
		_tcpPort = tcpPort;
		_udpPort = udpPort;
	}

	public void Start()
	{
		_server = GameNetFactory.CreateServer(_ipAddress, _tcpPort, _udpPort);
		_server.Start();

		Debug.Log($"Server started on {_server.IPAddress}:{_server.Config.Port}/{_server.Config.LocalUdpPort}");
	}
}
