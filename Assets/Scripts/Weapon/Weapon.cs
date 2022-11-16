using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    [RequireComponent(typeof(AudioSource))]
    public class Weapon : MonoBehaviour, IWeapon
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

        private Animator Animator;
        [SerializeField] private Sway sway;
        [SerializeField] private Recoil recoilComponent;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private ParticleSystem temp_MuzzleFlashParticlesFX;
        [SerializeField] private AudioClip shotSFX;
        [SerializeField] private AudioClip reloadSFX;
        [SerializeField] private AudioClip emptySFX;
        //prefabs
        public FirstPersonCamera Cam;
        private GameObject PrefabProjectile;

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

        // Start is called before the first frame update
        void Start()
        {
            weaponSetting = GameController.Instance.DataManager.GetArmsWeapon(weaponName);
            DataParticles DataParticles = GameController.Instance.DataManager.GetDataParticles("FlashParticles");

            shotSFX = GameController.Instance.DataManager.GetDataAudio(weaponSetting.shotSFX).Audio;
            reloadSFX = GameController.Instance.DataManager.GetDataAudio(weaponSetting.reloadingSFX).Audio;
            emptySFX = GameController.Instance.DataManager.GetDataAudio(weaponSetting.emptySFX).Audio;

            PrefabProjectile = weaponSetting.projectile;
            muzzleFlashTransform = transform.Find("ArmsAk/Muzzle flash transform");

            sway = transform.GetComponentInParent<Sway>();
            recoilComponent = GameObject.FindObjectOfType<Recoil>();
            audioSource = GetComponent<AudioSource>();
            if (GetComponentInChildren<Animator>())
                Animator = GetComponentInChildren<Animator>();

            temp_MuzzleFlashParticlesFX = Instantiate(DataParticles.Particles, muzzleFlashTransform);
        }
        // Update is called once per frame
        void Update()
        {

        }


        public bool Fire()
        {

            if (Time.time > nextFireTime && !reloading && canShot /*&& !controller.isClimbing*/ ) //Allow fire statement
            {

                if (currentAmmo > 0)
                {
                    currentAmmo -= 1;

                    PlayFX();
                    muzzleFlashTransform.LookAt(Cam.transform.position + Cam.transform.forward * 3000);
                    GameObject tempbala = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube));
                    tempbala.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    // player.CmdSpawBullet(new SpawBulletTransform(weaponSetting.projectile.name, muzzleFlashTransform.position, muzzleFlashTransform.rotation), player.GetConnection());
                    //Getting random damage from minimum and maximum damage.
                    //calculatedDamage = Random.Range ( damageMin , damageMax );

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