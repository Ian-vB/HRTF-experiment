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
    // Get gun object to get raycast location
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

        // Create evenly spaced soundlocations on sphere with Fibonacci agorithm
        soundLocations = new Vector3[20];
        soundLocations = fibonacciShpere(40, radius);
        Debug.Log($"amount of locations: {soundLocations.Length}");
        printarray(soundLocations);
        soundlocRandom = new Vector3[20];
        Array.Copy(soundLocations, soundlocRandom, soundLocations.Length);
        Shuffle();
    }

    public static void printarray(Vector3[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            Debug.Log($"{arr[i]}");

        }
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

    public static Vector3[] fibonacciShpere(float samples, float radius)
    {

        List<Vector3> points = new List<Vector3>();
        for (int j = 0; j < samples; j++) {
            float i = j;
            float k = i + .5f;

            float phi = Mathf.Acos(1f - 2f * k / samples);
            float theta = Mathf.PI * (1 + Mathf.Sqrt(5)) * k;

            float x = Mathf.Cos(theta) * Mathf.Sin(phi) * radius;
            float y = Mathf.Sin(theta) * Mathf.Sin(phi) * radius;
            float z = Mathf.Cos(phi) * radius;
            
            // Only keep points above 0 for half sphere
            if (y > 0)
            {
                Debug.Log($"vars: {j} {x} {y} {z}");
                points.Add(new Vector3(x, y, z));
                //Debug.Log($"in arr: {points[j]}");
            }
        }
        return points.ToArray();
    }

    private void OnEnable() => grabInteractable.activated.AddListener(TriggerPulled);

    private void OnDisable() => grabInteractable.activated.RemoveListener(TriggerPulled);


    private void TriggerPulled(ActivateEventArgs arg0)
    {
        

        if (timesFired <= 59)
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

        // ignore first shot fired to start experiment
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
            //int soundNumber = Array.IndexOf(soundLocations, soundlocRandom[index]);
            File.AppendAllText($"D:/User Projects/Ian/HRTF-experiment-data/{subjectName}.txt", $"{soundlocRandom[index]} {hitloc} \n");

            // Reshuffle when all locations have been played
            if (timesFired == 19 || timesFired == 39)
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
    // Shuffle the random sound locations in place
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
