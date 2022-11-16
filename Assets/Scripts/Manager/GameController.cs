using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ApocalipseZ
{
    public class GameController : MonoBehaviour
    {

        [Header("Prefab Managers")]
        [SerializeField] private DataManager PrefabDataManager;
        [SerializeField] private SceneManager PrefabSceneManager;
        [SerializeField] private InputManager PrefabInputManager;
        [SerializeField] private SoundManager PrefabSoundManager;
        [SerializeField] private TimerManager PrefabTimerManager;
        [SerializeField] private HitFXManager PrefabHitFxManager;
        [SerializeField] private DecalFxManager PrefabDecalFxManager;



        //privados 
        GameObject SpawPoint;
        private FpsPlayer fpsPlayer;
        private CanvasFpsPlayer canvasFpsPlayer;
        private DataManager dataManager;
        private SceneManager _sceneManager;
        private InputManager Input;
        private SoundManager sound;
        private TimerManager timerManager;
        private HitFXManager hitfxManager;
        private DecalFxManager decalfxManager;


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
            InitManagers();
        
        }
            
        public void InitManagers()
        {

            Instantiate(PrefabDataManager, transform);
            Instantiate(PrefabInputManager, transform);
            Instantiate(PrefabSoundManager, transform);
            Instantiate(PrefabTimerManager, transform);
            Instantiate(PrefabHitFxManager, transform);
            Instantiate(PrefabDecalFxManager, transform);

            if (GameObject.FindObjectOfType<SceneManager>() == null)
            {
                Instantiate(PrefabSceneManager);
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
      
        public CanvasFpsPlayer CanvasFpsPlayer
        {
            get
            {
                if (canvasFpsPlayer == null)
                {
                    canvasFpsPlayer = GameObject.FindObjectOfType<CanvasFpsPlayer>();
                }
                return canvasFpsPlayer;
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