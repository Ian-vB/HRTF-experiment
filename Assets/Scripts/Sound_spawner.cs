using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

public class Sound_spawner : MonoBehaviour
{
    [SerializeField] GameObject sphere;
    [SerializeField] XRGrabInteractable grabInteractable;
    [SerializeField] GameObject gunobj;
    [SerializeField] string subjectName;
    // Get gun object to get racast location
    Gun gun;
    Vector3[] soundLocations;
    Vector3[] soundlocRandom;

    GameObject soundObj;
    AudioSource sound;
    Vector3 hitloc;
    float radius;
    Vector3 randPos;
    int timesFired;
    bool start;
    int index;
    float totalDist;


    void Start()
    {
        // Instantiate starting variables
        gun = gunobj.GetComponent<Gun>();
        sound = gameObject.GetComponent<AudioSource>();
        radius = 2.9F;
        randPos = gameObject.transform.position;
        timesFired = -1;
        index = 0;
        totalDist = 0;

        start = false;
        //soundLocations = new Vector3[18];
        SphericalToCartesian(radius, 0.125F*Mathf.PI, 0.6F);
        soundLocations = new[] { SphericalToCartesian(radius, 0.125F * Mathf.PI, 0.6F), // Elevation 1, angles in increments of 1/8 pi radian.
                                 SphericalToCartesian(radius, 0.375F * Mathf.PI, 0.6F),
                                 SphericalToCartesian(radius, 0.625F * Mathf.PI, 0.6F),
                                 SphericalToCartesian(radius, 0.875F * Mathf.PI, 0.6F),
                                 SphericalToCartesian(radius, 1.125F * Mathf.PI, 0.6F),
                                 SphericalToCartesian(radius, 1.375F * Mathf.PI, 0.6F),
                                 SphericalToCartesian(radius, 1.625F * Mathf.PI, 0.6F),
                                 SphericalToCartesian(radius, 1.875F * Mathf.PI, 0.6F),

                                 SphericalToCartesian(radius, 0.333F * Mathf.PI, 0.9F), // Elevation 2
                                 SphericalToCartesian(radius, 0.666F * Mathf.PI, 0.9F),
                                 SphericalToCartesian(radius, Mathf.PI, 0.9F),
                                 SphericalToCartesian(radius, 1.333F * Mathf.PI, 0.9F),
                                 SphericalToCartesian(radius, 1.666F * Mathf.PI, 0.9F),
                                 SphericalToCartesian(radius, 2 * Mathf.PI, 0.9F),

                                 SphericalToCartesian(radius, 0.25F * Mathf.PI, 1.2F), // Elevation 3
                                 SphericalToCartesian(radius, 0.75F * Mathf.PI, 1.2F),
                                 SphericalToCartesian(radius, 1.25F * Mathf.PI, 1.2F),
                                 SphericalToCartesian(radius, 1.75F * Mathf.PI, 1.2F)
                                                                                      };
        soundlocRandom = new Vector3[18];
        Array.Copy(soundLocations, soundlocRandom, soundLocations.Length);
        Shuffle();
    }




    public static Vector3 SphericalToCartesian(float radius, float polar, float elevation)
    {
        Vector3 loc = new Vector3();
        float a = radius * Mathf.Cos(elevation);
        loc.x = a * Mathf.Cos(polar);
        loc.y = radius * Mathf.Sin(elevation);
        loc.z = a * Mathf.Sin(polar);
        return loc;
    }

    private void OnEnable() => grabInteractable.activated.AddListener(TriggerPulled);

    private void OnDisable() => grabInteractable.activated.RemoveListener(TriggerPulled);


    private void TriggerPulled(ActivateEventArgs arg0)
    {
        

        if (timesFired <= 48)
        {
            StartCoroutine(waiter());  
        }
        else
        {
            float avg = totalDist / 48;
            File.AppendAllText($"D:/User Projects/Ian/HRTF-experiment-data/{subjectName}.txt", $"{totalDist} {avg} \n");
        }
        
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(1);


        if (timesFired != -1)
        {
            
            Debug.Log($"index: {index}");

            // Get raycast from gun and calculate distance to soundsource
            hitloc = gun.hitloc;
            float dist = Vector3.Distance(hitloc, soundlocRandom[index]);
            Debug.Log($"Soundlocation: {soundlocRandom[index]}");

            Debug.Log($"Distance between sound and hit is {dist}");
            totalDist += dist;
            // Reference back to original soundlocations array to get original index of shuffled locations and save to file.
            int soundNumber = Array.IndexOf(soundLocations, soundlocRandom[index]);
            File.AppendAllText($"D:/User Projects/Ian/HRTF-experiment-data/{subjectName}.txt", $"{soundNumber} {dist} \n");

            // Reshuffle when all locations have been played
            if (timesFired == 17 || timesFired == 35)
            {
                Shuffle();
                index = 0;
            }
            else
            {
                index++;
            }
            gameObject.transform.position = soundlocRandom[index];
            sound.Play();
        }
        else
        {
            gameObject.transform.position = soundlocRandom[0];
            sound.Play();
        }
        timesFired++;
        

    }

    public void Shuffle()
    {
        Vector3 tempGO;
        for (int i = 0; i < soundlocRandom.Length - 1; i++)
        {
            int rnd = UnityEngine.Random.Range(i, soundlocRandom.Length);
            tempGO = soundlocRandom[rnd];
            soundlocRandom[rnd] = soundlocRandom[i];
            soundlocRandom[i] = tempGO;
        }
    }








}
