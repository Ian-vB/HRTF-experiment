using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circular_movement : MonoBehaviour
{
    // circle vars
    float time = 0;
    float speed;
    float width;
    float height;
    float x;
    float y;
    float z;
    bool z_toggle;

    // Audio vars
    AudioSource m_Heli;
    bool m_Play;
    
    void Start()
    {
        speed = 1;
        width = 3;
        height = 3;
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;


       //Fetch the AudioSource from the GameObject
       //Ensure the toggle is set to true for the music to play at start-up
        m_Heli = gameObject.GetComponent<AudioSource>();
        m_Play = false;
        m_Heli.Stop();
    }

    // Update is called once per frame
    void HitByRay()
    {
        Debug.Log("I was hit by a Ray");
        if (m_Play == true)
        {
            m_Heli.Stop();
            m_Play = false;
        }
        else if (m_Play == false)
        {
            m_Heli.Play();
            m_Play = true;
        }

    }
    void Update()
    {
      
        if (m_Play == true)
        {
            time += Time.deltaTime * speed;
            x = Mathf.Cos(time) * width;
            z = Mathf.Sin(time) * height;
            transform.position = new Vector3(x, transform.position.y, z);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            transform.position = new Vector3(transform.position.x, -2, transform.position.z);
        }



    }
}
