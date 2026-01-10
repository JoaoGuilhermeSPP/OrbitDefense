using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [Header("Movimento")]
    public GameObject target;
    public float speed = 30f;
    public Animator anim;
    private SpriteRenderer spriteRenderer;
    private bool Pmoves;

    [Header("Disparo")]
    public Transform point;
    public GameObject PrefBullet;
    public float fireRate = 0.5f;
    public float FireTime = 0.2f;
    public float bulletForce = 20f;

    [Header("Shield")]
    public GameObject Shield;
    public bool Ativo;
    public Image iconShield;

    void Start()
    {
        Shield.SetActive(false);
        Ativo = false;
        Pmoves = true;
        
    }
  
    // Update is called once per frame
    void Update()
    {
        Move();
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }
    public void Move()
    {

      
        if (Input.GetMouseButton(0))
        {
            
            if (Pmoves)
            transform.RotateAround(target.transform.position, Vector3.forward, speed * Time.deltaTime);
            Shoot();
            Shield.SetActive(false);
            iconShield.enabled = false;
            Ativo = false;
            anim.SetBool("move", true);
            spriteRenderer.flipX = false;
        }
        else if (Input.GetMouseButton(1))
        {
            
           if(Pmoves)
            transform.RotateAround(target.transform.position, Vector3.back, speed * Time.deltaTime);
            Shoot();
            Shield.SetActive(false);
            iconShield.enabled = false;
            Ativo = false;
            anim.SetBool("move", true);
            spriteRenderer.flipX = true;
        }
        else if (Input.GetMouseButton(2))
        { 
           
            Pmoves = false;
            Shoot();
            Shield.SetActive(false);
            iconShield.enabled = false;
            Ativo = false;
            anim.SetBool("move", true);
            spriteRenderer.flipX = true;
         
        }
        else
        {
            Ativo=true;
            Shield.SetActive(true);
            iconShield.enabled = true;
            anim.SetBool("move", false);

        }
        //volta o movimento
        if (Input.GetMouseButtonUp(2))
        {
            Pmoves=true;
        }
    }
    public void Shoot()
    {
        if (Time.time >= FireTime)
        {
            GameObject bullet = Instantiate(PrefBullet, point.position,point.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(point.up * bulletForce, ForceMode2D.Impulse);
            FireTime = Time.time + fireRate;
            StartCoroutine(DestroyBullet(bullet, 2f));
        }
    }
    IEnumerator DestroyBullet(GameObject bullet, float times)
    {
        yield return new WaitForSeconds(times);
        Destroy(bullet, 4f);
    }
}
