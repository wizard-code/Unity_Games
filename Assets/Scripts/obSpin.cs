using UnityEngine;

public class obSpin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 300f;
    

    // Update is called once per frame
        void Update()
        {
            transform.Rotate(0f, speed * Time.deltaTime, 0f);
        }
    }

