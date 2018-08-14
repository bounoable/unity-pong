using GameNet;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Pong.Core
{
    class GameManager
    {
        public enum Scene
        {
            MainMenu,
            ServerControl,
            Lobby,
        }

        public static GameManager Instance => GetInstance();
        static GameManager _instance;

        public GameNetFactory NetworkFactory { get; } = new GameNetFactory();

        public Server Server { get; private set; }
        public Client Client { get; private set; }

        public static GameManager GetInstance()
        {
            if (_instance == null) {
                _instance = new GameManager();
            }

            return _instance;
        }

        GameManager()
        {}

        /// <summary>
        /// Create and start a game server.
        /// </summary>
        /// <param name="ip">The IP address of the server.</param>
        /// <param name="tcpPort">The TCP port the server uses.</param>
        /// <param name="udpPort">The UDP port the server uses.</param>
        /// <returns>The game server.</returns>
        public Server CreateServer(string ip, ushort tcpPort, ushort udpPort)
        {
            Server server = NetworkFactory.CreateServer(ip, tcpPort, udpPort);

            server.Start();
            Server = server;

            return server;
        }

        public void StopServer()
        {
            Server?.Stop();
            Server = null;
        }
        
        /// <summary>
        /// Create a game client and connect to a server.
        /// </summary>
        /// <param name="ip">The server's ip address.</param>
        /// <param name="tcpPort">The server's TCP port.</param>
        /// <param name="udpPort">The UDP port the client uses.</param>
        /// <returns>The game client.</returns>
        public Client CreateClient(string ip, ushort tcpPort, ushort udpPort)
        {
            Client client = NetworkFactory.CreateClient(udpPort);

            client.Connect(ip, tcpPort);
            Client = client;

            return client;
        }

        public void StopClient()
        {
            Client?.Disconnect();
            Client = null;
        }
        
        async public Task PrepareQuit()
        {
            if (Client != null) {
                await Client.Disconnect();
            }

            Server?.Stop();

            Debug.ClearDeveloperConsole();
        }

        async public Task LoadScene(Scene scene)
        {
            switch (scene) {
                case Scene.MainMenu:
                    await LoadMainMenu();
                    break;
                
                case Scene.ServerControl:
                    await LoadServerControl();
                    break;
                
                case Scene.Lobby:
                    await LoadLobby();
                    break;
            }
        }

        public Task LoadMainMenu()
            => LoadSceneByName("MainMenu");
        
        async public Task LoadServerControl()
        {
            if (Server == null)
                return;
            
            await LoadSceneByName("ServerControl");
        }

        async public Task LoadLobby()
        {
            if (Client == null)
                return;
            
            await LoadSceneByName("Lobby");
        }

        async Task LoadSceneByName(string name)
        {
            AsyncOperation load = SceneManager.LoadSceneAsync(name);

            while (!load.isDone) {
                await Task.Delay(100);
            }
        }
    }
}