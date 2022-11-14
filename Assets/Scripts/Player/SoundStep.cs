using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using Random = UnityEngine.Random;
namespace ApocalipseZ
{
    [System.Serializable]
    public struct TextureType
    {
        public string NomeGrupo;
        public Texture[] textures;
        public AudioClip[] footStepSounds;

    }
    public class SoundStep : NetworkBehaviour
    {
        [SerializeField] private ScriptableTextureSounds scriptableTextureSounds;

        public GetCollisionGround GetCollisionGround;
        private AudioSource source;
        public float DelayStep;
        public float timeStep;

        [SyncVar]
        public float fatorDelay = 1;
        [SyncVar]
        public bool IsGround;
        [SyncVar]
        public bool IsMoviment;

        // Start is called before the first frame update
        void Start()
        {
            source = GetComponent<AudioSource>();
            GetCollisionGround = GetComponent<GetCollisionGround>();
            scriptableTextureSounds = GameController.Instance.DataManager.GetScriptableTextureSounds();
        }
        public void PlaySound()
        {
            if (GetCollisionGround.CollisionObject == null)
            {
                return;
            }
            if (GetCollisionGround.CollisionObject.GetComponent<MeshRenderer>())
            {
                PlayMeshRenderer(GetCollisionGround.CollisionObject.GetComponent<MeshRenderer>());
            }
            else if (GetCollisionGround.CollisionObject.GetComponent<Terrain>())
            {

                PlayTerrain(GetCollisionGround.CollisionObject.GetComponent<Terrain>());
            }
        }

        private void PlayTerrain(Terrain terrain)
        {
            GetTerrainData Terrain = terrain.gameObject.GetComponent<GetTerrainData>();

            foreach (TextureType type in scriptableTextureSounds.TextureType)
            {
                foreach (Texture tex in type.textures)
                {
                    if (Terrain.GetTexture(transform.position) == tex)
                    {
                        AudioClip clip = type.footStepSounds[Random.Range(0, type.footStepSounds.Length)];
                        source.PlayOneShot(clip);
                    }
                }
            }

        }


        private void PlayMeshRenderer(MeshRenderer renderer)
        {
            if (scriptableTextureSounds.TextureType.Length > 0)
            {
                foreach (TextureType type in scriptableTextureSounds.TextureType)
                {
                    foreach (Texture tex in type.textures)
                    {
                        if (renderer.material.mainTexture == tex)
                        {
                            AudioClip clip = type.footStepSounds[Random.Range(0, type.footStepSounds.Length)];
                            source.PlayOneShot(clip);
                        }
                    }
                }
            }
        }

        private void Update()
        {

            if (IsGround)
            {
                if (IsMoviment)
                {
                    if (timeStep == 0)
                    {
                        PlaySound();
                    }
                    timeStep += Time.deltaTime;

                    if (timeStep >= DelayStep * fatorDelay)
                    {
                        timeStep = 0;
                    }
                }
                else
                {
                    timeStep = 0;
                }
            }
        }

        internal void SetIsGround(bool _IsGround)
        {
            IsGround = _IsGround;
        }

        internal void SetIsMoviment(bool _moviment)
        {
            IsMoviment = _moviment;
        }

        internal void SetFatorDelay(float _fatordelay)
        {
            fatorDelay = _fatordelay;
        }
    }
}