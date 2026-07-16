using UnityEngine;

public class MeteoroBuff : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 40f; // Agora configurável

    private bool isActive = true;

    void Start()
    {
        // Evita FindGameObjectWithTag repetido
        if (target == null)
        {
            GameObject mundo = GameObject.FindGameObjectWithTag("Mundo");
            if (mundo != null)
            {
                target = mundo.transform;
            }
            else
            {
                Debug.LogWarning($"MeteoroBuff {gameObject.name}: Mundo não encontrado!");
                isActive = false;
            }
        }
    }

    void Update()
    {
        if (!isActive || target == null) return;

        // Rotação mais suave e com deltaTime
        transform.RotateAround(target.position, Vector3.back, rotationSpeed * Time.deltaTime);
    }

    // Para a rotação quando o objeto for desativado
    private void OnDisable()
    {
        isActive = false;
    }

    private void OnDestroy()
    {
        isActive = false;
        target = null;
    }
}