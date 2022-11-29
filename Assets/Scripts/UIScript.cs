using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{

    [SerializeField] private GameObject player;
    private PlayerControl plctrl;
    private float currentHP;
    private float maxHP;
    private int pistolCurrentAmmo;
    private int shotgunCurrentAmmo;
    private int pistolMagCap;
    private int shotgunMagCap;
    private int activeWeapon;
    private bool isReloading;
    private float pistolReloadTime;
    private float shotgunReloadTime;

    [SerializeField] private TMP_Text currentHPText;
    [SerializeField] private TMP_Text maxHPText;
    [SerializeField] private TMP_Text activeWeaponCurrentAmmoText;
    [SerializeField] private TMP_Text activeWeaponMagCapText;
    [SerializeField] private Image activeWeaponIcon;
    [SerializeField] private Sprite pistolIcon;
    [SerializeField] private Sprite shotgunIcon;
    [SerializeField] private Sprite noWeaponIcon;

    // Start is called before the first frame update
    void Start()
    {
        plctrl = player.GetComponent<PlayerControl>();
    }
    // Update is called once per frame
    void Update()
    {
        // Max health is only checked at the start of the map.
        maxHP = plctrl.GetMaxHealth();
        maxHPText.text = maxHP.ToString();

        // Get weapon reload times. Only checked at the start of the map.
        pistolReloadTime = plctrl.GetPistolReloadTime();
        shotgunReloadTime = plctrl.GetShotgunReloadTime();
    }

    private void FixedUpdate()
    {
        // Update values for UI elements.
        currentHP = plctrl.GetCurrentHealth();
        pistolCurrentAmmo = plctrl.GetPistolAmmo();
        shotgunCurrentAmmo = plctrl.GetShotgunAmmo();
        pistolMagCap = plctrl.GetPistolMagazineCapacity();
        shotgunMagCap = plctrl.GetShotgunMagazineCapacity();
        activeWeapon = plctrl.GetActiveWeapon();
        isReloading = plctrl.IsReloading();

        // Show current HP in UI.
        currentHPText.text = currentHP.ToString();

        // Check which weapon is active and show correct UI information.
        switch(activeWeapon)
        {
            case 0:
                activeWeaponCurrentAmmoText.text = pistolCurrentAmmo.ToString();
                activeWeaponMagCapText.text = pistolMagCap.ToString();
                activeWeaponIcon.sprite = pistolIcon;
                break;

            case 1:
                activeWeaponCurrentAmmoText.text = shotgunCurrentAmmo.ToString();
                activeWeaponMagCapText.text = shotgunMagCap.ToString();
                activeWeaponIcon.sprite = shotgunIcon;
                break;

            default:
                activeWeaponCurrentAmmoText.text = "0";
                activeWeaponMagCapText.text = "0";
                activeWeaponIcon.sprite = noWeaponIcon;
                break;
        }

        // Reload indicator on WeaponIcon
        if(Input.GetButtonDown("Reload") && !isReloading)
        {
            //ReloadAnimation(activeWeapon);

            activeWeaponIcon.fillAmount = 0;
            Debug.Log("UI Reloading.");

            if (activeWeapon == 0)
            {
                activeWeaponIcon.fillAmount += 1 / pistolReloadTime * Time.deltaTime;
            }
            else if (activeWeapon == 1)
            {
                activeWeaponIcon.fillAmount += 1 / shotgunReloadTime * Time.deltaTime;
            }

        }

        IEnumerator ReloadAnimation(int activeWeapon)
        {
            activeWeaponIcon.fillAmount = 0;
            Debug.Log("UI Reloading.");

            if (activeWeapon == 0)
            {
                yield return new WaitForSeconds(pistolReloadTime);
                activeWeaponIcon.fillAmount += 1 / pistolReloadTime * Time.deltaTime;
            }
            else if (activeWeapon == 1)
            {
                yield return new WaitForSeconds(shotgunReloadTime);
                activeWeaponIcon.fillAmount += 1 / shotgunReloadTime * Time.deltaTime;
            }

        }
    }
}
