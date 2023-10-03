using System.Collections;
using System.Collections.Generic;
using FishNet;
using UnityEngine;

namespace ApocalipseZ
{
    public class GameController : MonoBehaviour
    {

        [Header("Prefab Managers")]
        public string playerName;
        public string characterName;
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
        private PlayerSpawPointsManager playerSpawPoints;
        private SpawObjectsManager spawObjectsManager;


        // Start is called before the first frame update
        void Awake()
        {
            characterName = "Yasmin";
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
                if (item.GetComponent<SceneManager>())
                {
                    if (GameObject.FindObjectOfType<SceneManager>() == null)
                    {
                        Instantiate(item);
                    }
                }
                else
                {
                    Instantiate(item, transform);
                }

            }

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
        public PlayerSpawPointsManager PlayerSpawPoints
        {
            get
            {
                if (playerSpawPoints == null)
                {
                    playerSpawPoints = transform.GetComponentInChildren<PlayerSpawPointsManager>();
                }
                return playerSpawPoints;
            }
        }

        public SpawObjectsManager SpawObjectsManager
        {
            get
            {
                if (spawObjectsManager == null)
                {
                    spawObjectsManager = transform.GetComponentInChildren<SpawObjectsManager>();
                }
                return spawObjectsManager;
            }
        }


    }
}