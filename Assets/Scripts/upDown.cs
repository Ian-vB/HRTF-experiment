using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upDown : MonoBehaviour
{
    // Start is called before the first frame update
    bool up;

    // Audio vars
    AudioSource m_Heli;
    bool m_Play;
    void Start()
    {

        m_Heli = gameObject.GetComponent<AudioSource>();
        m_Play = false;
        m_Heli.Stop();
        up = false;
    }

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

    // Update is called once per frame
    void Update()
    {
        if (m_Play == true)
        {
            if (transform.position.y >= 5)
            {
                up = false;
            }
            if (transform.position.y <= -0.5)
            {
                up = true;
            }


            if(up == true)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.008F, transform.position.z);

            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.008F, transform.position.z);
            }
        }
    }
}
