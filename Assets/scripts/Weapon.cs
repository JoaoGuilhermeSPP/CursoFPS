using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Animations;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float range = 100f; //ranged ate onde a arma alcança
    public int totalbullet = 30; //total de municao por pente
    public int bulletsLeft = 90; // o restante
    public int currentBullet; //atual do pente
    public float fireRate = 0.4f; //tempo de cowdown por tiro
    private float fireTimer; // tempo
    private Animator Anim; 

    public Transform shootPoint;//Ponto do raycast 

    public ParticleSystem FireEfect;// efx de atirar

    private bool isReload; //verifica se carregou

    public AudioClip shooterSounds; //som da arma
    private AudioSource audioSource;
    void Start()
    {
        currentBullet = totalbullet;
        Anim = GetComponent<Animator>(); //recebe o animator
        audioSource = GetComponent<AudioSource>(); //recebe o audio
    }

  
    void Update()
    {
        if (Input.GetButton("Fire1"))// atira
        {
            if(currentBullet > 0) 
            {
                Fire();
               
            }else if (bulletsLeft > 0) 
            {
                DoReload();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))//recarregar 
        {
            if(currentBullet < totalbullet && bulletsLeft > 0) //Caso esteja maior recarrega
            {
                DoReload();
            }
        }
        if (fireTimer < fireRate) //determina o tempo de um tiro após o outro
        {
            fireTimer += Time.deltaTime;
        }
        
    }
    private void Fire() //Fogo 
    {
        if (fireTimer < fireRate || isReload || currentBullet <= 0 )
        {
            return;
        }

        fireTimer = 0f;
        RaycastHit hit;
        

        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range)) //raycast onde identtifica o ojeto que foi  acertado
        {
            Debug.Log(hit.transform.name);
        }
        Anim.CrossFadeInFixedTime("atirar", 0.01f);//chama animacao pele nome e tempo e transicao
        FireEfect.Play();//inicia o efeito de atirar
        PlayShootSound();
        currentBullet--; //Decai a muniçao
        fireTimer = 0f;
    }
    private void FixedUpdate()
    {
        AnimatorStateInfo info = Anim.GetCurrentAnimatorStateInfo(0);
        isReload = info.IsName("recarregar");// isReload recebe o valor da execucao da animacao reload

    }
    void DoReload() //realiza a animacao de recarregar
    {
        if (isReload)
        {
            return;
        }
        Anim.CrossFadeInFixedTime("recarregar", 0.1f);
    }
    public void Reload() // Logica para recarregar o pente 
    {
        if (bulletsLeft <= 0) 
        {
            return;
        }

        int bulletsToLoad = totalbullet - currentBullet;
        int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft; //if ternario para determinar se a condicao foi aceita recebe o bulletsToLoad caso nao bulletsLeft
        bulletsLeft -= bulletsToDeduct;
        currentBullet += bulletsToDeduct;
             
    }
    public void PlayShootSound() 
    {
        audioSource.PlayOneShot(shooterSounds);
    }
}
