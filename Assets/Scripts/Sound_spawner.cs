using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;



public class Sound_spawner : MonoBehaviour
{
    [SerializeField] GameObject sphere;
    [SerializeField] XRGrabInteractable grabInteractable;
    [SerializeField] GameObject gun;

    GameObject soundObj;
    AudioSource sound;
    RaycastHit hit;
    float radius;
    Vector3 randPos;


    void Start()
    {
        //soundObj = new GameObject("soundObj");
        //soundObj.AddComponent<AudioSource>();
        //soundObj.GetComponent<AudioSource>().spatialBlend = 1.0F;
        //soundObj.GetComponent<AudioSource>().clip = test;
        //hit = gun.GetComponent<RaycastHit>();
        Mesh mesh = sphere.GetComponent<MeshFilter>().mesh;
        sound = gameObject.GetComponent<AudioSource>();
        Vector3[] verts = mesh.vertices;
        radius = 3F;
        randPos = gameObject.transform.position;
    }

    private void OnEnable() => grabInteractable.activated.AddListener(TriggerPulled);

    private void OnDisable() => grabInteractable.activated.RemoveListener(TriggerPulled);


    private void TriggerPulled(ActivateEventArgs arg0)
    {
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(3);
        //hit = gun.GetComponent<RaycastHit>();
        //float dist = Vector3.Distance(hit.point, randPos);
        //Debug.Log($"Distance is {dist}");
        randPos = UnityEngine.Random.onUnitSphere * radius;
        Debug.Log($"Sound at position {randPos}, radius is {radius}, randpos = {UnityEngine.Random.onUnitSphere}");
        gameObject.transform.position = randPos;
        sound.Play();
    }


        // Start is called before the first frame update
   



    

}
