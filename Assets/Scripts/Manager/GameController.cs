using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using UnityEngine;
using Random = UnityEngine.Random;
namespace ApocalipseZ
{
    public class GameController : MonoBehaviour
    {
        public User user;

        [Header("TESTE")]
        [SerializeField] private ConnectionManager connectionManager;
        [Header("Prefab Managers")]
        [SerializeField] private GameObject[] ObjectsManager;
        //privados 
        public PlayerController playerController;
        private DataManager dataManager;
        private SceneManager _sceneManager;
        private InputManager Input;
        private SoundManager sound;
        private TimerManager timerManager;
        private HitFXManager hitfxManager;
        private DecalFxManager decalfxManager;
        [SerializeField] private List<PlayerSpawPointsManager> playerSpawPointsManagers;
        [SerializeField] private List<SpawObjectsManager> SpawsObjectsManager;
        [SerializeField] private PVPManager pvpManager;


        // Start is called before the first frame update
        void Awake()
        {

            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
            ObjectsManager = Resources.LoadAll<GameObject>("Manager");
            InitManagers();
            DontDestroyOnLoad(this);
        }

        public void InitManagers()
        {

            foreach (var item in ObjectsManager)
            {

                Instantiate(item, transform);

            }

        }

        public void AddPlayerSpawPointManager(PlayerSpawPointsManager playerSpawPointsManager)
        {
            playerSpawPointsManagers.Add(playerSpawPointsManager);
        }
        public void RemovePlayerSpawPointManager(PlayerSpawPointsManager playerSpawPointsManager)
        {
            playerSpawPointsManagers.Remove(playerSpawPointsManager);
        }

        public PlayerSpawPointsManager GetPlayerSpawPointManager(int currentScene)
        {

            PlayerSpawPointsManager teste = playerSpawPointsManagers.Find(x => x.currentSceneHandle == currentScene);
            if (teste)
            {
                return teste;
            }
            System.Random rng = new System.Random();
            playerSpawPointsManagers = playerSpawPointsManagers.OrderBy(item => rng.Next()).ToList();
            var randomItem = playerSpawPointsManagers.FirstOrDefault(x => x.currentSceneName != "SceneFlagTest");
            return randomItem;
        }
        internal void AddSpawObjectsManager(SpawObjectsManager spawObjectsManager)
        {
            SpawsObjectsManager.Add(spawObjectsManager);
        }
        internal void RemoveSpawObjectsManager(SpawObjectsManager spawObjectsManager)
        {
            SpawsObjectsManager.Remove(spawObjectsManager);
        }

        /*
       public void SpawPlayer(GameObject player = null)
       {
           if (Player == null)
           {
               return;
           }

           SpawPoint = GameObject.Find("SpawPoint");

           if (SpawPoint == null)
           {
               if (player == null)
               {
                   Instantiate(Player).transform.position = transform.position;
               }
               else
               {
                   player.transform.position = transform.position;
               }

           }
           else
           {
               if (player == null)
               {
                   Instantiate(Player, SpawPoint.transform.position, SpawPoint.transform.rotation);
               }
               else
               {
                   player.transform.position = SpawPoint.transform.position;
               }

           }
       }
*/
        private static GameController _instance;


        public static GameController Instance
        {
            get
            {

                return _instance;
            }
        }
        public ConnectionManager ConnectionManager
        {
            get
            {
                return connectionManager;
            }
            set
            {
                connectionManager = value;
            }
        }
        public PVPManager PvpManager
        {
            get
            {
                return pvpManager;
            }
            set
            {
                pvpManager = value;
            }
        }

        public DataManager DataManager
        {
            get
            {
                if (dataManager == null)
                {
                    dataManager = GameObject.FindObjectOfType<DataManager>();
                }
                return dataManager;
            }
        }
        public SceneManager SceneManager
        {
            get
            {
                if (_sceneManager == null)
                {
                    _sceneManager = GameObject.FindObjectOfType<SceneManager>();
                }
                return _sceneManager;
            }
        }
        public InputManager InputManager
        {
            get
            {
                if (Input == null)
                {
                    Input = GameObject.FindObjectOfType<InputManager>();
                }
                return Input;
            }
        }
        public SoundManager SoundManager
        {
            get
            {
                if (sound == null)
                {
                    sound = transform.GetComponentInChildren<SoundManager>();
                }
                return sound;
            }
        }
        public TimerManager TimerManager
        {
            get
            {
                if (timerManager == null)
                {
                    timerManager = transform.GetComponentInChildren<TimerManager>();
                }
                return timerManager;
            }
        }
        public HitFXManager HitFXManager
        {
            get
            {
                if (hitfxManager == null)
                {
                    hitfxManager = transform.GetComponentInChildren<HitFXManager>();
                }
                return hitfxManager;
            }
        }
        public DecalFxManager DecalFxManager
        {
            get
            {
                if (decalfxManager == null)
                {
                    decalfxManager = transform.GetComponentInChildren<DecalFxManager>();
                }
                return decalfxManager;
            }
        }





    }
}