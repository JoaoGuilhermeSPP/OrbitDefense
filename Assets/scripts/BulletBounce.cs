using UnityEngine;

public class BulletBounce : MonoBehaviour
{
    public float maxY;
    public float minY;
    public float speed = 2f;
    public float waitTime = 5f;
    public float bulletForce = 20f;
    public GameObject effect;
    public Transform Target;
    public GameObject BulletDmg;

    private Vector3 pos;
    private bool goingUp = false;
    private bool waiting = false;
    private float timer = 0f;

    void Start()
    {
        pos = transform.position;
        pos.y = maxY; // começa em cima
        effect.SetActive(false);
        transform.position = pos;
    }

    void Update()
    {
        if (waiting)
        {
            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                timer = 0f;
                waiting = false;

                // troca a direção após esperar
                goingUp = !goingUp;
            }

            return;

        }

        Move();
        pos.z = 0f;
        transform.position = pos;
    }

    void Move()
    {
        if (goingUp)
        {
            // sobe
            pos.y += speed * Time.deltaTime;

            if (pos.y >= maxY)
            {
                pos.y = maxY;
                waiting = true; // espera 5s no topo
            }
        }
        else
        {
            // desce
            pos.y -= speed * Time.deltaTime;

            if (pos.y <= minY)
            {
                pos.y = minY;
                effect.SetActive(true);
                Invoke("Effect", 0.5f);
                Shooter(); // atira ao chegar embaixo
                waiting = true; // espera 5s embaixo
            }
        }
    }

    void Shooter()
    {
        GameObject bullet = Instantiate(BulletDmg, Target.position, Target.rotation);
        SFXManager.current.PlayMusic(SFXManager.current.BulletB);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(Target.up * bulletForce, ForceMode2D.Impulse);

        Destroy(bullet, 5f);
    }
    void Effect()
    {
        effect.SetActive(false);
    }
}