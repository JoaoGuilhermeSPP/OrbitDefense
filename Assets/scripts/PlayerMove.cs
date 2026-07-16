using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [Header("Movimento")]
    public GameObject target;
    public float speed = 200f;
    public Animator anim;
    public SpriteRenderer spriteRenderer;
   // private bool Pmoves;
    public SkinShop[] item;

    [Header("Disparo")]
    public static PlayerMove instance;
    public Transform point;
    public GameObject PrefBullet;
    [Header("Bullet")]
    public Sprite defaultBulletSprite;
    public Sprite currentBulletSprite;
    // public GameObject Bulleteffect;
    public float fireRate = 0.5f;
    public float FireTime = 0.2f;
    public float bulletForce = 20f;


    [Header("Shield")]
    public GameObject Shield;
    public bool Ativo;
    public Image iconShield;

    [Header("Visual")]
    public Transform visualChild;

    private float currentRotationSpeed;
    private float rotationVelocity;

    [Header("Smooth")]
    public float smoothTime = 0.15f;
    public float maxRotationSpeed = 200f;


   

    private void Awake()
    {
        instance = this;

        if (visualChild == null && transform.childCount > 0)
        {
            visualChild = transform.GetChild(0);
        }
    }

    void Start()
    {
        currentBulletSprite = defaultBulletSprite;
        Shield.SetActive(false);
        Ativo = false;
      //  Pmoves = true;

        // VERIFICAÇÃO DE SEGURANÇA
        if (SaveManager.instance != null && SaveManager.instance.data != null)
        {
            string equippedId = SaveManager.instance.data.equipedItem;

            if (!string.IsNullOrEmpty(equippedId))
            {
                foreach (var itens in item)
                {
                    if (itens.ItemId == equippedId)
                    {
                        EquipShip(itens);
                        break;
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("SaveManager não disponível no Start do PlayerMove");
        }
    }
    void Update()
    {
        if (Time.timeScale == 0) return;
        Move();
    }

    void LateUpdate()
    {
        if (visualChild != null)
        {
            visualChild.rotation = Quaternion.identity;
        }
    }

    public void Move()
    {
        bool left = false;
        bool right = false;
        bool center = false;

        bool mobile =
            MobileInput.Instance != null &&
            MobileInput.Instance.IsMobileControlsEnabled;

        if (mobile)
        {
            // SOMENTE UI MOBILE
            left = MobileInput.Instance.leftPressed;
            right = MobileInput.Instance.rightPressed;
            center = MobileInput.Instance.centerPressed;
        }
        else
        {
            // SOMENTE DESKTOP
            left =
                Input.GetMouseButton(0) ||
                Input.GetKey(KeyCode.LeftArrow) ||
                Input.GetKey(KeyCode.A);

            right =
                Input.GetMouseButton(1) ||
                Input.GetKey(KeyCode.RightArrow) ||
                Input.GetKey(KeyCode.D);

            center =
                Input.GetMouseButton(2) ||
                Input.GetKey(KeyCode.UpArrow) ||
                Input.GetKey(KeyCode.W);
        }

        if (left)
        {
            HandleRotation(speed);

            Shoot();

            Shield.SetActive(false);
            iconShield.enabled = false;
            Ativo = false;

            anim.SetBool("move", true);

            if (spriteRenderer != null)
                spriteRenderer.flipX = false;
        }
        else if (right)
        {
            HandleRotation(-speed);

            Shoot();

            Shield.SetActive(false);
            iconShield.enabled = false;
            Ativo = false;

            anim.SetBool("move", true);

            if (spriteRenderer != null)
                spriteRenderer.flipX = true;
        }
        else if (center)
        {
            // Atira parado
            HandleRotation(0);

            Shoot();

            Shield.SetActive(false);
            iconShield.enabled = false;
            Ativo = false;

            anim.SetBool("move", true);
        }
        else
        {
            // Escudo ativo quando não faz nada
            HandleRotation(0);

            Ativo = true;

            Shield.SetActive(true);
            iconShield.enabled = true;

            anim.SetBool("move", false);
        }
    }

    public void Shoot()
    {
        if (Time.time < FireTime)
            return;

        SFXManager.current.PlayMusic(
            SFXManager.current.bulletPlayer
        );

        if (PowerUp.instance != null &&
            PowerUp.instance.IsBulletBuffActive())
        {
            ShootDouble(point);
        }
        else
        {
            ShootSingle(point);
        }

        FireTime = Time.time + fireRate;
    }

    void ShootSingle(Transform point)
    {
        GameObject bullet =
            Instantiate(
                PrefBullet,
                point.position,
                point.rotation
            );

        Bullet bulletScript =
            bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.spriteRenderer.sprite =
                currentBulletSprite;
        }

        Rigidbody2D rb =
            bullet.GetComponent<Rigidbody2D>();

        rb.AddForce(
            point.up * bulletForce,
            ForceMode2D.Impulse
        );

        Destroy(bullet, 2f);
    }

    void ShootDouble(Transform point)
    {
        float offSetX = PowerUp.instance.offSetX;

        GameObject leftBullet = Instantiate(
            PowerUp.instance.bulletDouble,
            point.position - point.right * offSetX,
            point.rotation


        );

       
        GameObject rightBullet = Instantiate(
            PowerUp.instance.bulletDouble,
            point.position + point.right * offSetX,
            point.rotation
        );

        Rigidbody2D rbLeft = leftBullet.GetComponent<Rigidbody2D>();
        Rigidbody2D rbRight = rightBullet.GetComponent<Rigidbody2D>();

        rbLeft.AddForce(point.up * bulletForce, ForceMode2D.Impulse);
        rbRight.AddForce(point.up * bulletForce, ForceMode2D.Impulse);

        Destroy(leftBullet, 2f);
        Destroy(rightBullet, 2f);
    }
    void HandleRotation(float targetSpeed)
    {
        if (target == null) return;
        currentRotationSpeed = Mathf.SmoothDamp(
     currentRotationSpeed,
     targetSpeed,
     ref rotationVelocity,
     Mathf.Max(0.01f, smoothTime)
 );

        transform.RotateAround(
            target.transform.position,
            Vector3.forward,
            currentRotationSpeed * Time.deltaTime
        );
    }
    public void EquipShip(SkinShop item)
    {
        SpriteRenderer sr =
            visualChild.GetComponent<SpriteRenderer>();

        Animator animator =
            visualChild.GetComponent<Animator>();

        sr.sprite = item.skin;

        animator.runtimeAnimatorController =
            item.controller;

        currentBulletSprite =
            item.BulletSkin;
    }
    IEnumerator DestroyBullet(GameObject bullet, float times)
    {
        yield return new WaitForSeconds(times);
        Destroy(bullet, 4f);
    }

}
