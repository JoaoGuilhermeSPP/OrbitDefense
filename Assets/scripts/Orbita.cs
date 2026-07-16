using DG.Tweening;
using UnityEngine;

public class Orbita : MonoBehaviour
{
  
    public float time = 0.5f;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.DOMoveY(transform.position.y + 0.2f, time)
           .SetLoops(-1, LoopType.Yoyo)
           .SetEase(Ease.InOutSine);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
