using UnityEngine;

public class Orbita : MonoBehaviour
{
    public Vector2 position;
    private float PositionMove = 0.5f;
    public float speed = 0.3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float emY = position.y + Mathf.Sin(Time.time * speed) * PositionMove;

        transform.position = new Vector2(position.x, emY);
    }
}
