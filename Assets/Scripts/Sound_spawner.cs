using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;



public class Sound_spawner : MonoBehaviour
{
    [SerializeField] GameObject sphere;
    [SerializeField] XRGrabInteractable grabInteractable;
    [SerializeField] GameObject gunobj;
    Gun gun;
    Vector3[] soundLocations;


    GameObject soundObj;
    AudioSource sound;
    Vector3 hitloc;
    float radius;
    Vector3 randPos;
    int timesFired;


    void Start()
    {
        gun = gunobj.GetComponent<Gun>();
        sound = gameObject.GetComponent<AudioSource>();
        radius = 2.9F;
        randPos = gameObject.transform.position;
        timesFired = 0;
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

            StartCoroutine(waiter(timesFired));
            timesFired++;
        }
    }

    IEnumerator waiter(int timesFired)
    {
        int index = timesFired;
        yield return new WaitForSeconds(1);
        if (timesFired >= 18 && timesFired <= 36)
        {
            Shuffle();
            index = timesFired - 18;
        }
        if(timesFired >= 36)
        {
            Shuffle();
            index = timesFired - 36;
        }
        Debug.Log($"array index: {index}");
        hitloc = gun.hitloc;
        float dist = Vector3.Distance(hitloc, soundLocations[0]);
        Debug.Log($"Distance between sound and hit is {dist}");
        gameObject.transform.position = soundLocations[index];
        sound.Play();
    }

    public void Shuffle()
    {
        Vector3 tempGO;
        for (int i = 0; i < soundLocations.Length - 1; i++)
        {
            int rnd = Random.Range(i, soundLocations.Length);
            tempGO = soundLocations[rnd];
            soundLocations[rnd] = soundLocations[i];
            soundLocations[i] = tempGO;
        }
    }








}
