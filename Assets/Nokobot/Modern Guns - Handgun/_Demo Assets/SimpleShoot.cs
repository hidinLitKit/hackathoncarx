using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{

    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;
    public int Current_Ammo = 10;
    public int Total_Ammo = 100;
    public int Max_Ammo = 10;
    TMP_Text Reloading;
    TMP_Text TMP_T_CA;
    TMP_Text TMP_T_TA;
    [SerializeField]float reloadTime;
    public bool CanShoot = true;
    void Start()
    {

        Reloading = GameObject.Find("Reloading...").GetComponent<TMP_Text>();
        TMP_T_CA = GameObject.Find("AmmoCurrent/Max").GetComponent<TMP_Text>();
        TMP_T_TA = GameObject.Find("AmmoTotal").GetComponent<TMP_Text>();
        TMP_T_CA.text = Current_Ammo.ToString() + " / " + Max_Ammo.ToString();
        TMP_T_TA.text = Total_Ammo.ToString();
        Reloading.gameObject.SetActive(false);
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        //If you want a different input, change it here
        if (Input.GetButtonDown("Fire1"))
        {
            //Calls animation on the gun that has the relevant animation events that will fire
            if (CanShoot)
            {
                if (Current_Ammo > 0)
                {
                    gunAnimator.SetTrigger("Fire");
                    Current_Ammo -= 1;
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Current_Ammo < 10) { StartCoroutine(WaitForReload()); }
        }
    }


    //This function creates the bullet behavior
    void Shoot()
    {

        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        GameObject bull = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        bull.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
        RefreshAmmo();
        Destroy(bull, 10f);
    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

    void Reload()
    {
        Total_Ammo -= Max_Ammo - Current_Ammo;
        Current_Ammo = 10;
        Reloading.gameObject.SetActive(false);
        CanShoot = true;
        RefreshAmmo();
        StopCoroutine(WaitForReload());
    }
    IEnumerator WaitForReload()
    {
        CanShoot= false;
        Reloading.gameObject.SetActive(true);
        yield return new WaitForSeconds(reloadTime);
        Reload();
    }
    public void RefreshAmmo()
    {
        TMP_T_CA.text = Current_Ammo.ToString() + " / " + Max_Ammo.ToString();
        TMP_T_TA.text = Total_Ammo.ToString();
    }
}
