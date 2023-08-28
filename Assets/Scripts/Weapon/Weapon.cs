using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using GameKit.Utilities.ObjectPooling.Examples;
using UnityEngine;
namespace ApocalipseZ
{
    [RequireComponent(typeof(AudioSource))]
    public class Weapon : NetworkBehaviour, IWeapon
    {
        [SerializeField] private DataArmsWeapon weaponSetting;
        public DataArmsWeapon WeaponSetting { get => weaponSetting; }
        [SerializeField] private string weaponName;
        public string WeaponName { get => weaponName; }
        private float scopeSensitivityX, scopeSensitivityY;

        [Header("Transforms Objects Spaws")]
        [Tooltip("Transform to instantiate particle system shot fx")]
        public Transform muzzleFlashTransform;
        [Tooltip("Transform to eject shell after shot")]
        public Transform shellTransform;
        [Tooltip("Transform to instantiate bullet on shot")]
        public Transform bulletTransform;

        [Tooltip("If you have animations for your weapon so better to use animator. Play animations if true and not if false")]
        public bool useAnimator = true;

        [Tooltip("How long reload animation is? Time in seconds to synch reloading animation with script")]
        public float reloadAnimationDuration = 3.0f;

        [Tooltip("Should weapon reload when ammo is 0")]
        public bool autoReload = true;

        [Header("Ammo")]
        [Tooltip("Ammo count in weapon magazine")]

        [SerializeField] private int currentAmmo = 30;
        public int CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }


        [Tooltip("Max weapon ammo capacity")]
        public int maxAmmo = 30;
        public int currentClip = 120;
        public int MaxClip = 120;

        public enum FireMode { automatic, single }
        [Header("Fire mode")]
        public FireMode fireMode;

        [SerializeField]private Animator Animator;
        [SerializeField] private Sway sway;
        [SerializeField] private Recoil recoilComponent;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private ParticleSystem temp_MuzzleFlashParticlesFX;
        [SerializeField] private AudioClip shotSFX;
        [SerializeField] private AudioClip reloadSFX;
        [SerializeField] private AudioClip emptySFX;
        //prefabs
        public FirstPersonCamera Cam;
        [SerializeField] private GameObject PrefabProjectile;

        private float nextFireTime;

        [HideInInspector]
        public bool reloading = false;
        [HideInInspector]
        public bool canShot = true;

        [SerializeField] private bool setAim = false;
        public bool SetAim { get => setAim; }

        public bool isThrowingGrenade;

        public void setCurrent(int oldcurrent, int newcurrent)
        {
            currentAmmo = newcurrent;
        }
     private void OnValidate()
    {
        
    }
 
        // Start is called before the first frame update
        void Start()
        {
            weaponSetting = GameController.Instance.DataManager.GetArmsWeapon(weaponName);
            PrefabProjectile = GameController.Instance.DataManager.GetDataBullet(weaponSetting.projectile).Bullet;
            DataParticles DataParticles = GameController.Instance.DataManager.GetDataParticles("FlashParticles");
            shotSFX = GameController.Instance.DataManager.GetDataAudio(weaponSetting.shotSFX).Audio;
            reloadSFX = GameController.Instance.DataManager.GetDataAudio(weaponSetting.reloadingSFX).Audio;
            emptySFX = GameController.Instance.DataManager.GetDataAudio(weaponSetting.emptySFX).Audio;
            sway = transform.GetComponentInParent<Sway>();
            recoilComponent = GameObject.FindObjectOfType<Recoil>();
            audioSource = GetComponent<AudioSource>();
            if (GetComponentInChildren<Animator>())
                Animator = GetComponentInChildren<Animator>();

            temp_MuzzleFlashParticlesFX = Instantiate(DataParticles.Particles, muzzleFlashTransform);
        }
        // Update is called once per frame
        

        public bool Fire()
        {

            if (Time.time > nextFireTime && !reloading && canShot /*&& !controller.isClimbing*/ ) //Allow fire statement
            {

                if (currentAmmo > 0)
                {
                    currentAmmo -= 1;
                    PlayFX();
                    muzzleFlashTransform.LookAt(Cam.transform.position + Cam.transform.forward * 3000);
                    SpawnProjectile(muzzleFlashTransform.position, muzzleFlashTransform.forward, 0f);
                    //calculatedDamage = Random.Range ( damageMin , damageMax );
                    CmdFire(muzzleFlashTransform.position , muzzleFlashTransform.forward , base.TimeManager.Tick);
                    // ProjectilesManager ( );
                    recoilComponent.AddRecoil(weaponSetting.recoil);
                    //Calculating when next fire call allowed
                    nextFireTime = Time.time + weaponSetting.fireRate;
                    return true;
                }
                else
                {
                    if (!reloading && autoReload)
                    {
                        ReloadBegin();
                    }
                    else
                        audioSource.PlayOneShot(emptySFX);

                    nextFireTime = Time.time + weaponSetting.fireRate;
                    return false;
                }
            }
            return false;
        }
        private const float MAX_PASSED_TIME = 0.3f;
        private void ClientFire()
        {
            Vector3 position = transform.position;
            Vector3 direction = transform.forward;

            /* Spawn locally with 0f passed time.
             * Since this is the firing client
             * they do not need to accelerate/catch up
             * the projectile. */
            SpawnProjectile(position, direction, 0f);
            //Ask server to also fire passing in current Tick.
            CmdFire(position, direction, base.TimeManager.Tick);
        }
        private void SpawnProjectile(Vector3 position, Vector3 direction, float passedTime)
        {
                    BalisticProjectile go = Instantiate(PrefabProjectile, position, Quaternion.identity).GetComponent<BalisticProjectile>();
                    go.Initialize(direction, passedTime);
        }
        [ServerRpc(RequireOwnership = false)]
        private void CmdFire(Vector3 position, Vector3 direction, uint tick)
        {
            
            /* You may want to validate position and direction here.
             * How this is done depends largely upon your game so it
             * won't be covered in this guide. */

            //Get passed time. Note the false for allow negative values.
            float passedTime = (float)base.TimeManager.TimePassed(tick, false);
            /* Cap passed time at half of constant value for the server.
             * In our example max passed time is 300ms, so server value
             * would be max 150ms. This means if it took a client longer
             * than 150ms to send the rpc to the server, the time would
             * be capped to 150ms. This might sound restrictive, but that would
             * mean the client would have roughly a 300ms ping; we do not want
             * to punish other players because a laggy client is firing. */
            passedTime = Mathf.Min(MAX_PASSED_TIME / 2f, passedTime);

            //Spawn on the server.
            SpawnProjectile(position, direction, passedTime);
            //Tell other clients to spawn the projectile.
            ObserversFire(position, direction, tick);
        }
        [ObserversRpc(ExcludeOwner = true)]
        private void ObserversFire(Vector3 position, Vector3 direction, uint tick)
        {
            //Like on server get the time passed and cap it. Note the false for allow negative values.
            float passedTime = (float)base.TimeManager.TimePassed(tick, false);
            passedTime = Mathf.Min(MAX_PASSED_TIME, passedTime);

            //Spawn the projectile locally.
            SpawnProjectile(position, direction, passedTime);
        }
        public void ReloadBegin()
        {
            if (reloading)
            {
                return;
            }
            if (CalculateTotalAmmo() > 0)
            {
                setAim = false;
                reloading = true;
                canShot = false;

                if (useAnimator)
                {
                    Animator.SetBool("Aim", false);
                    Animator.Play("Reload");
                }

                audioSource.PlayOneShot(reloadSFX);

                Invoke("ReloadEnd", reloadAnimationDuration);
            }
            else
                return;
        }
        public int CalculateTotalAmmo()
        {
            int totalAmmo = new int();
            totalAmmo = 30;
            // foreach ( var item in inventory.characterItems )
            // {
            //     if ( item.id == ammoItemID )
            //     {
            //         totalAmmo += item.ammo;
            //     }
            // }

            return totalAmmo;
        }
        void ReloadEnd()
        {

            var neededAmmo = maxAmmo - currentAmmo;

            if (currentClip >= neededAmmo)
            {
                currentClip -= neededAmmo;
                currentAmmo += neededAmmo;


            }
            else if (currentClip < neededAmmo)
            {
                currentAmmo += currentClip;
                neededAmmo -= currentClip;
                currentClip = 0;

            }

            reloading = false;
            canShot = true;

            if (setAim && useAnimator)
            {
                Animator.SetBool("Aim", true);
            }

        }

        public void Aim(bool isAim)
        {
            setAim = isAim;
            if (Animator == null)
            {
                print("opa");
                return;
            }
            if (useAnimator)
            {
                Animator.SetBool("Aim", isAim);
            }
            if (setAim)
            {
                sway.AmountX = sway.AmountX * 0.3f;
                sway.AmountY = sway.AmountY * 0.3f;
            }
            else
            {
                sway.AmountX = sway.startX;
                sway.AmountY = sway.startY;
            }


            if (!reloading && useAnimator)
            {
                Animator.SetBool("Aim", setAim);
            }
            else
            {
                setAim = false;
            }
        }
        /*
        public void ThrowGrenade()
        {

            var obj = Instantiate(weaponSetting.grenadePrefab, muzzleFlashTransform.position, Quaternion.identity);
            //obj.transform.position = transform.position + transform.forward * 0.3f;
            obj.GetComponent<Rigidbody>().AddForce(transform.forward * weaponSetting.throwForce);
            isThrowingGrenade = false;
            // inventory.RemoveItem ( "Grenade" , true );
            // weaponManager.UnhideWeaponAfterGrenadeDrop ( );
        }
*/
        public void PlayFX()
        {
            if (useAnimator)
                Animator.Play("Shot");

            temp_MuzzleFlashParticlesFX.time = 0;
            temp_MuzzleFlashParticlesFX.Play();

            audioSource.Stop();
            audioSource.PlayOneShot(shotSFX);

        }
        public DataArmsWeapon GetScriptableWeapon()
        {
            return weaponSetting;
        }
    }

}