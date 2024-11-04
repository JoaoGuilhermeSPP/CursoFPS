using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponmanager : MonoBehaviour
{

    public GameObject[] Weapons;
    private int index;
    public float switchDelay = 1f;
    private bool isSwitching;
    // Start is called before the first frame update
    void Start()
    {
        InitializeWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel")>0 && !isSwitching)
        {
            index++;
            if(index >= Weapons.Length)
            {
                index = 0;
            }

            StartCoroutine(switchWeaponDeleay(index));

        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0 && !isSwitching)
        {
            index--;
            if (index < 0)
            {
                index = Weapons.Length - 1;
            }
            StartCoroutine(switchWeaponDeleay(index));
        }
    }
    IEnumerator switchWeaponDeleay(int newIndex) 
    {
        isSwitching = true;
        yield return new WaitForSeconds(switchDelay);
        isSwitching = false;
        SwitchWeapons(newIndex);
    }
    void SwitchWeapons(int newIndex)
    {
        for (int i = 0; i < Weapons.Length; i++)
        {
            Weapons[i].SetActive(false);
        }
        Weapons[newIndex].SetActive(true);
    }
        void InitializeWeapons()
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                Weapons[i].SetActive(false);
            }
            Weapons[0].SetActive(true);
        }
 }
