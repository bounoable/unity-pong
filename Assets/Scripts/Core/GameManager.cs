using UnityEngine;
using Pong.Network;
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
            GameSession,
        }

        public static GameManager Instance => GetInstance();
        static GameManager _instance;

        public GameNet.GameNetFactory NetworkFactory { get; } = new GameNet.GameNetFactory();

        public Server Server { get; private set; }
        public Client Client { get; private set; }

        public GameState Session { get; private set; }

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
            Server = new Server(NetworkFactory.CreateServer(ip, tcpPort, udpPort));

            return Server;
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
        public Client CreateClient(ushort udpPort)
        {
            Client = new Client(NetworkFactory.CreateClient(udpPort));

            return Client;
        }

        async public Task StopClient()
        {
            await Client?.Disconnect();
            Client = null;
            StopServer();
        }
        
        async public Task PrepareQuit()
        {
            await StopClient();
            StopServer();
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
                
                case Scene.GameSession:
                    await LoadGameSession();
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

        async public Task LoadGameSession()
        {
            if (Client == null)
                return;
            
            await LoadSceneByName("GameSession");
        }

        async Task LoadSceneByName(string name)
        {
            AsyncOperation load = SceneManager.LoadSceneAsync(name);

            while (!load.isDone) {
                await Task.Delay(100);
            }
        }

        async public void StartSession(int id, Player challenger, Player challenged)
        {
            Session = new GameState(id, challenger.CreateGamePlayer(), challenged.CreateGamePlayer());
            await LoadGameSession();
        }
    }
}