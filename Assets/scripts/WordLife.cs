using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NUnit.Framework.Internal;

public class WordLife : MonoBehaviour
{
  
    
    public Image BarUI;
    public int MaxHealth = 100;
    public int Life;

    private SpriteRenderer Sr;
    private Color OrigColor;

    void Start()
    {
        Life = MaxHealth;
      
        Sr = GetComponent<SpriteRenderer>();
        OrigColor = Sr.color;
        UpdateLife();
    }

    void Update()
    {
       
    }
    public void takeDamage(int damage)
    {
        Life -= damage;

        StartCoroutine(Effect());
        UpdateLife();

        if (Life <= 0)
        {
            Die();
        }

    }
    public void UpdateLife()
    {
        float Fill = (float)Life / MaxHealth;
        BarUI.fillAmount = Fill;
    }
    void Die()
    {
        Destroy(gameObject);
    }
    public IEnumerator Effect()
    {
        Sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        Sr.color = OrigColor;

    }
    
}
