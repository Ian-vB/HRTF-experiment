using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadHRTF : MonoBehaviour
{

    [SerializeField] int HRTF_index;
    // Start is called before the first frame update
    void Start()
    {
        GameObject manager = GameObject.Find("Steam Audio Manager");
        manager.GetComponent<SteamAudio.SteamAudioManager>().currentHRTF = HRTF_index;
    }

}
