using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class GunSystem : MonoBehaviour
{

    public float fireRate = 0.25f;                                      // Number in seconds which controls how often the player can fire
    public float weaponRange = 20f;                                     // Distance in Unity units over which the player can fire

    public Transform gunEnd;
    public ParticleSystem muzzleFlash;
    public ParticleSystem cartridgeEjection;

    public GameObject metalHitEffect;
    public GameObject sandHitEffect;
    public GameObject stoneHitEffect;
    public GameObject waterLeakEffect;
    public GameObject waterLeakExtinguishEffect;
    public GameObject[] fleshHitEffects;
    public GameObject woodHitEffect;

    private float nextFire;                                             // Float to store the time the player will be allowed to fire again, after firing
    private Animator anim;
    private GunAim gunAim;


    [SerializeField]
    private TrailRenderer BulletTrail;

    public AudioClip shootingSound;
    public AudioClip hitSound;
    public AudioClip reloadingSound;

    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    //public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    //Graphics
    //public ParticleSystem muzzleFlash, bulletHoleGraphic;
    public CameraShake camShake;
    public float camShakeMagnitude, camShakeDuration;
    public TextMeshProUGUI text;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;

        anim = GetComponent<Animator>();
        gunAim = GetComponentInParent<GunAim>();
    }
    private void Update()
    {
        MyInput();

        //SetText
        text.text = bulletsLeft + " / " + magazineSize;

    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)// && !gunAim.GetIsOutOfBounds())
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }
    private void Shoot()
    {
        Debug.Log("Vurdu");
        readyToShoot = false;

        AudioSource.PlayClipAtPoint(shootingSound, transform.position);

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, z);

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);
            HandleHit(rayHit);

            /*if (rayHit.collider.CompareTag("Enemy"))
                rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);*/

            TrailRenderer trail = Instantiate(BulletTrail, gunEnd.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, rayHit));
        }

        //ShakeCamera
        StartCoroutine(camShake.Shake(camShakeDuration, camShakeMagnitude));

        //Graphics
        //Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        //Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        muzzleFlash.Play();
        cartridgeEjection.Play();
        anim.SetTrigger("Fire");

        /*Vector3 rayOrigin = gunEnd.position;
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, gunEnd.forward, out hit, weaponRange))
        {
            HandleHit(hit);
        }*/

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        AudioSource.PlayClipAtPoint(reloadingSound, transform.position);
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }


    void HandleHit(RaycastHit hit)
    {
        if (hit.collider.sharedMaterial != null)
        {
            string materialName = hit.collider.sharedMaterial.name;

            if (hit.collider.gameObject.layer == 12)
            {
                hit.collider.gameObject.SetActive(false);
            }

            switch (materialName)
            {
                case "Metal":
                    SpawnDecal(hit, metalHitEffect);
                    AudioSource.PlayClipAtPoint(hitSound, transform.position);
                    break;
                case "Sand":
                    SpawnDecal(hit, sandHitEffect);
                    break;
                case "Stone":
                    SpawnDecal(hit, stoneHitEffect);
                    break;
                case "WaterFilled":
                    SpawnDecal(hit, waterLeakEffect);
                    SpawnDecal(hit, metalHitEffect);
                    break;
                case "Wood":
                    SpawnDecal(hit, woodHitEffect);
                    break;
                case "Meat":
                    SpawnDecal(hit, fleshHitEffects[Random.Range(0, fleshHitEffects.Length)]);
                    break;
                case "Character":
                    SpawnDecal(hit, fleshHitEffects[Random.Range(0, fleshHitEffects.Length)]);
                    break;
                case "WaterFilledExtinguish":
                    SpawnDecal(hit, waterLeakExtinguishEffect);
                    SpawnDecal(hit, metalHitEffect);
                    break;
                default:
                    SpawnDecal(hit, metalHitEffect);
                    break;

            }
        }
    }

    void SpawnDecal(RaycastHit hit, GameObject prefab)
    {
        GameObject spawnedDecal = GameObject.Instantiate(prefab, hit.point, Quaternion.LookRotation(hit.normal));
        spawnedDecal.transform.SetParent(hit.collider.transform);
    }


    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;
        
        while (time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, Hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }

        Destroy(Trail.gameObject, Trail.time);
    }

}