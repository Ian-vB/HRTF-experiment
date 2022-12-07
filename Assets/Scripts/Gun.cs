using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] XRGrabInteractable grabInteractable;
    [SerializeField] Transform raycastOrigin;
    [SerializeField] LayerMask targetLayer;
    AudioSource gunAudio;
    [SerializeField] AudioClip laserSFX;
    public Vector3 hitloc;


    private void Awake()
    {
        if(TryGetComponent(out AudioSource audio))
        {
            gunAudio = audio;
        }
        else
        {
            gunAudio = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        }
    }


    private void OnEnable() => grabInteractable.activated.AddListener(TriggerPulled);
    
    private void OnDisable() => grabInteractable.activated.RemoveListener(TriggerPulled);

    private void TriggerPulled(ActivateEventArgs arg0)
    {
        arg0.interactor.GetComponent<XRBaseController>().SendHapticImpulse(.5f, .25f);
        //grabInteractable.GetComponent<XRBaseController>().SendHapticImpulse(.5f, .25f);
        gunAudio.Play();
        FireRaycast();

    }

    private void FireRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, targetLayer))
        {
           Debug.Log($"hit target {hit.transform.name} at location {hit.point}");
           hit.transform.SendMessage("HitByRay");
           hitloc = hit.point; 

        }
    }
}
