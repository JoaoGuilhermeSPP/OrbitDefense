using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class shootelaser : MonoBehaviour
{
    [Header("Referências")]
    public Transform target;
    public Transform laserTarget;
    public Transform PointFire;
    public GameObject effect;

    private LineRenderer lineRenderer;
    private NavMeshAgent agent;

    [Header("Laser")]
    public float MaxDistance = 100f;
    public float danoSecond = 2;
    public int danoSecondP = 2;
    public Material laserMaterial;
    public LayerMask Mundo;

    [Header("Movimento")]
    public float CurrentAngle = 0f;
    public float rotateSpeed = 20f;

    private int direction = 1;

    [Header("Pulso")]
    public float pulseSpeed = 6f;
    public float pulseAmount = 0.15f;
    private float baseWidth = 0.5f;

    private bool laserActive;

    void Start()
    {
        if (effect != null)
            effect.SetActive(false);

        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        GameObject mundo = GameObject.FindGameObjectWithTag("Mundo");

        if (mundo != null)
        {
            target = mundo.transform;

           
        }
        GameObject child = GameObject.FindGameObjectWithTag("CentroDoLaser");
        if(child != null)
        {
            laserTarget = child.transform;
        }
        SetupLineRenderer();
    }

    void SetupLineRenderer()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.positionCount = 2;

        // Shader compatível com URP/WebGL
        if (laserMaterial != null)
            lineRenderer.material = laserMaterial;

        Gradient gradient = new Gradient();

        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(Color.green, 0f),
                new GradientColorKey(Color.white, 0.5f),
                new GradientColorKey(Color.red, 1f)
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        );

        lineRenderer.colorGradient = gradient;

        lineRenderer.startWidth = baseWidth;
        lineRenderer.endWidth = baseWidth;

        lineRenderer.useWorldSpace = true;

        lineRenderer.numCapVertices = 10;
        lineRenderer.numCornerVertices = 10;

        lineRenderer.sortingLayerName = "Default";
        lineRenderer.sortingOrder = 50;

        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (target == null)
        {
            StopLaser();
            StopAgentSafe();
            return;
        }

        if (WordLife.instance != null &&
            WordLife.instance.Life <= 0)
        {
            StopLaser();
            return;
        }

        if (agent != null &&
            agent.isOnNavMesh &&
            !agent.isStopped)
        {
            agent.SetDestination(target.position);
        }

        Move();

        PulseLaser();

        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            0f
        );
    }

    void Move()
    {
        if (target == null)
            return;

        float step =
            rotateSpeed *
            Time.deltaTime *
            direction;

        transform.RotateAround(
            target.position,
            Vector3.forward,
            step
        );

        CurrentAngle += step;

        if (direction == 1)
        {
            Shooter();
        }
        else
        {
            StopLaser();
        }

        if (CurrentAngle >= 180f)
        {
            direction = -1;
        }
        else if (CurrentAngle <= -180f)
        {
            direction = 1;
        }
    }

    void Shooter()
    {
        if (PointFire == null || laserTarget == null)
            return;

        if (!laserActive)
        {
            laserActive = true;

            lineRenderer.enabled = true;

            if (effect != null)
                effect.SetActive(true);

            if (SFXManager.current != null)
            {
                SFXManager.current.PlayMusic(
                    SFXManager.current.Laser
                );
            }
        }

        Vector3 start = PointFire.position;
        Vector3 end = laserTarget.position;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        WordLife damage =
            target.GetComponent<WordLife>();

        if (damage != null)
        {
            damage.TakeDamage(
                danoSecond * Time.deltaTime
            );
        }
    }
    void StopLaser()
    {
        laserActive = false;

        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }

        if (effect != null)
        {
            effect.SetActive(false);
        }
    }

    void StopAgentSafe()
    {
        if (agent != null &&
            agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
    }

    void PulseLaser()
    {
        if (lineRenderer == null ||
            !lineRenderer.enabled)
        {
            return;
        }

        float pulse =
            Mathf.Sin(
                Time.time * pulseSpeed
            ) * pulseAmount;

        float width =
            baseWidth + pulse;

        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }
}