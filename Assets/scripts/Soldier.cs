using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;//biblioteca para criar um Ia

public class Soldier : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;

    public ParticleSystem efect;
    public bool isDead;
    private GameObject Player;
    private PlayerHealth playerHealth;

    public float atkDistance; //distancia para atacar
    public float followDistance; //distancia para seguir
    public float atkProbality; //Probabilidade de acerto

    public int damage;//total de dano que tira do player
    public int health;//vida total

    public Transform shootpoint;
    public float range;
    public float fireRate = 0.1f;
    private float FireTime;

    public AudioClip shooterSounds; //som da arma
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = Player.GetComponent<PlayerHealth>();
        audioSource = Player.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.enabled && !playerHealth.isDead)
        {
            float dist = Vector3.Distance(Player.transform.position, transform.position); //checa a distancia player e inimigo
            bool shoot = false;
            bool follow = (dist < followDistance);//auxiliar a saber se o inimigo ver o player

            if (follow)
            {
                if (dist < atkDistance)
                {
                    shoot = true;
                    Fire();
                }
                agent.SetDestination(Player.transform.position);//Segue o player com todas as condições verdadeiras
               
                shootpoint.LookAt(Player.transform);
                transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));//corrige que o inimigo nao incline

            }
            if (!follow || shoot)
            {
                agent.SetDestination(transform.position);

            }

            anim.SetBool("shoot", shoot);
            anim.SetBool("run", follow);

        }
        if(FireTime < fireRate) 
        {
            FireTime += Time.deltaTime;
        }
    }
        public void Fire()//Logica de tiro do inimigo.
        {
        if (FireTime < fireRate) { return; }
           
            
            RaycastHit Hit;
          if (Physics.Raycast(shootpoint.position, shootpoint.forward, out Hit, range))
          {
            if (Hit.transform.GetComponent<PlayerHealth>())//dano no inimigo
            {
                Hit.transform.GetComponent<PlayerHealth>().applyDamage(damage);//a variavel serve para retira o damage
            }
        }
        FireTime = 0;
        efect.Play();
        PlayShoot();

    }

    public void applyDamage(int damage) //perde vida
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            anim.SetTrigger("die");
            anim.SetBool("shoot", false);
            anim.SetBool("run", false);
            agent.enabled = false;
            isDead = true;

            playerHealth.IncrementPoints();
        }
    }
    public void PlayShoot() 
    {
        audioSource.PlayOneShot(shooterSounds);
    }
}

