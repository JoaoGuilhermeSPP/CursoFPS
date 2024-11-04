using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [Header("Parametros")]
    public float range = 100f; //ranged ate onde a arma alcança
    public int totalbullet = 30; //total de municao por pente
    public int bulletsLeft = 90; // o restante
    public int currentBullet; //atual do pente
    public float fireRate = 0.4f; //tempo de cowdown por tiro
    private float fireTimer; // tempo
    public float spreadFactor;
    private Animator Anim;

    [Header("Shoots")]
    public Transform shootPoint;//Ponto do raycast 
    private bool isReload; //verifica se carregou
   
    [Header("Sound")]
    public AudioClip shooterSounds; //som da arma
    private AudioSource audioSource;
   
    [Header("Effects")]
    public ParticleSystem FireEfect;// efx de atirar
    public GameObject hitEffect;//Efeito ao colidir
    public GameObject bulletEfect;
  
    [Header("Dano - Semi/auto")]
    public int damage;


    [Header("UI")]
    public Text amoText;

    public enum ShootMode //Modo de tiro semi automatico
    {
       auto,
       semi
    }
    public ShootMode shootMode;
    private bool ShootInput;

    private void OnEnable()//Chama-se quando objeto é ativado
    {
        UpdateamoText();
    }

    [Header("Mira")]//Variaveis da mira
    public Vector3 miraPos;
    public float miraSpeed = 10f;
    private Vector3 originalPos;

    void Start()
    {
        currentBullet = totalbullet;
        Anim = GetComponent<Animator>(); //recebe o animator
        audioSource = GetComponent<AudioSource>(); //recebe o audio
        originalPos = transform.localPosition; //Posicap original
        UpdateamoText();//UI
    }

  
    void Update()
    {

        /*if (Input.GetButton("Fire1"))// atira
        {
            if(currentBullet > 0) 
            {
                Fire();

            }else if (bulletsLeft > 0) 
            {
                DoReload();
            }
         }*/
        
        switch (shootMode)// Determina se é semi ou automatico
        {
            case ShootMode.auto:
                ShootInput = Input.GetButton("Fire1");
            break;
            
            case ShootMode.semi:
                ShootInput = Input.GetButtonDown("Fire1");
            break;

        }
        if (ShootInput)//logica de tiro
        {
            if (currentBullet > 0)
            {
                Fire();
            }

            else if (bulletsLeft > 0)
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
        Toaim();
    }
    private void Fire() //Fogo 
    {
        if (fireTimer < fireRate || isReload || currentBullet <= 0)
        {
            return;
        }

        fireTimer = 0f;
        RaycastHit hit;
        Vector3 ShootDirection = shootPoint.transform.forward; //Direcao do tiro
        ShootDirection = ShootDirection + shootPoint.transform.TransformDirection(new Vector3(Random.Range(-spreadFactor, spreadFactor), Random.Range(-spreadFactor, spreadFactor)));//Logica para o tiro nao ficar centralizado

        if (Physics.Raycast(shootPoint.position,ShootDirection, out hit, range)) //raycast onde identtifica o ojeto que foi  acertado
        {
            GameObject hitParticle = Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)); //Instancia a fumaça do tiro
            GameObject bullt = Instantiate(bulletEfect, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal)); //Instancia o buraco do tiro
            
            Destroy(hitParticle, 0.7f);
            Destroy(bullt, 0.7f);

            if (hit.transform.GetComponent<ObjectHealth>())
            {
                bulletEfect.transform.SetParent(hit.transform); //Faz com que a municao entre dentro do objeto acertado o destruindo
                hit.transform.GetComponent<ObjectHealth>().applyDamage(damage);//a variavel serve para retira o damage
            }

            Anim.CrossFadeInFixedTime("atirar", 0.01f);//chama animacao pele nome e tempo e transicao
            FireEfect.Play();//inicia o efeito de atirar
            PlayShootSound();//som
            UpdateamoText();//UI
            currentBullet--; //Decai a muniçao
            fireTimer = 0f;
        }
    }

    public void Toaim() 
    {
        if(Input.GetButton("Fire2") && !isReload) 
        {
            Anim.applyRootMotion = true;//Como o objeto foi animado no animation usei para ativar
            transform.localPosition = Vector3.Lerp(transform.localPosition, miraPos, Time.deltaTime * miraSpeed);
            UnityEngine.Debug.Log("Pressed");
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime * miraSpeed);
            UnityEngine.Debug.Log("NoPressed");
        }
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
            Anim.applyRootMotion = false;//corrige bug de rotacionar o objeto por conta de ter feito a animacao da arma na unity
            return;
        }
        Anim.CrossFadeInFixedTime("recarregar", 0.1f);
        UpdateamoText();//UI
    }
    public void Reload() // Logica para recarregar o pente.
    {
        Anim.applyRootMotion = false;
        if (bulletsLeft <= 0) 
        {
            return;
        }
        UpdateamoText();//UI
        int bulletsToLoad = totalbullet - currentBullet;
        int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft; //if ternario para determinar se a condicao foi aceita recebe o bulletsToLoad caso nao bulletsLeft
        bulletsLeft -= bulletsToDeduct;
        currentBullet += bulletsToDeduct;
             
    }
    public void PlayShootSound() 
    {
        audioSource.PlayOneShot(shooterSounds);
    }

    void UpdateamoText() //UI
    {
        amoText.text = currentBullet + "/" + bulletsLeft; //manipula o texto UI
    }
}
