using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioClip[] _audio;
   public AudioSource[] _source;

    // Start is called before the first frame update


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _source[0].enabled = true; 
        _source[1].enabled = true;
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
