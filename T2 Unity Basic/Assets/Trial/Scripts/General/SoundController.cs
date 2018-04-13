using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour {

    [SerializeField]protected AudioSource speaker;

    public AudioClip[] attackSound;
    //public AudioClip[] attackVocal;

    //public AudioClip[] attackSound;
    //public AudioClip[] attackSound;
    //public AudioClip[] attackSound;
    //public AudioClip[] attackSound;
    //public AudioClip[] attackSound;

    protected void Awake()
    {
        if (null == speaker)
            speaker = GetComponent<AudioSource>();
    }



    public virtual void Attack(float delay = 0f)
    {
        PlayOfAGroup(attackSound, delay);

        
    }

    public virtual void PlayOfAGroup(AudioClip[] group, float delay = 0)
    {
        if (0 == group.Length)
            return;


        //speaker.clip = group[Random.Range(0, group.Length)];
        //speaker.PlayDelayed(delay);
        speaker.PlayOneShot(group[Random.Range(0, group.Length)]);
    }


}
