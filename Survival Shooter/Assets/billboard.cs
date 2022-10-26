using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billboard : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer m_SpriteRenderer;
    private Camera camera;
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.LookAt(camera.transform);
        if (Input.GetKey("space"))
        {
            m_SpriteRenderer.color = Color.blue;
        }
        else
        {
            m_SpriteRenderer.color = Color.red;
        }
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}
