using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private int maxHealth = 100;
    public int health;
    public int recoveryFactor = 20;
    public Image bloodscreen;//efeito de perder vida
    public Image redImage;
    
    public Text Points;
    public int points;
    private Color alphaamount;

    public float recoveryRate;
    private float rrecoveryTimer;
  
    public bool isDead;
    private void Start()
    {
        health = maxHealth;
        Points.text = points.ToString();
    }
    private void Update()
    { 
        rrecoveryTimer += Time.deltaTime;
        if(rrecoveryTimer > recoveryRate) 
        {
            StartCoroutine(RecoveryHealth());
        }
    }
    public void IncrementPoints() // Incrementa pontos quando um soldado morre
    {
        points++; // Incrementa a pontuação
        Points.text = points.ToString(); // Atualiza o texto de pontos

        // Verifica se a condição de vitória foi atingida
        if (points >= 4)
        {
            Controller.gc.ShowWin(); // Exibe a tela de vitória
        }
    }
    public void applyDamage(int damage) //perde vida
    {
        health -= damage;
        alphaamount = bloodscreen.color;
        alphaamount.a += ((float)damage / 100);//realiza alteracao de cor em alpha, casta para float o damage

        bloodscreen.color = alphaamount;
        if (redImage.color.a < 0.4f)
        {
            redImage.color = new Color(255f, 0, 0, alphaamount.a);
        }
        if (health <= 0)
        {
            Controller.gc.ShowGameOver();
            isDead = true;
        }
        rrecoveryTimer = 0f;
    }
   IEnumerator RecoveryHealth()//recarregar vida quando sair do dano
    {

        while(health < maxHealth) 
        {
            health += recoveryFactor;
            alphaamount.a -= ((float)recoveryFactor / 100);
            bloodscreen.color = alphaamount;
            redImage.color = new Color(255f, 0, 0, alphaamount.a);
            yield return new WaitForSeconds(2f);
        }
       
    }
}
