using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocalController : SoundController
{

    public float speakingChance = 0.3f;
    //prevent the character from repeating same vocal
    int lastIndex = 99;
    
    public override void Attack(float delay = 0f)
    {
        if (Random.Range(0, 1f) < speakingChance)
            PlayOfAGroup(attackSound);
    }

    public override void PlayOfAGroup(AudioClip[] group, float delay = 0)
    {
        if (0 == group.Length)
            return;

        int index = Random.Range(0, group.Length);

        if (1 != group.Length)
        {//repeating prevention
            if (lastIndex == index)
            {
                index++;

                index %= group.Length;
            }

            lastIndex = index;
        }


        speaker.Stop();
        speaker.PlayOneShot(group[index]);

        
    }
}
