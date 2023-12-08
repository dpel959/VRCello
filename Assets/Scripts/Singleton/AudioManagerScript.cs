using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManagerScript : Singleton<AudioManagerScript>
{
    [SerializeField] Sound[] sfxSounds = null;
    [SerializeField] Sound[] bgmSounds = null;

    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] AudioSource[] sfxPlayer = null;

    public bool isMusicStart = false;
    public void PlayBGM(string p_bgmName)
    {
        for(int i =0; i < bgmSounds.Length; i++)
        {
            if(p_bgmName == bgmSounds[i].name)
            {
                bgmPlayer.clip = bgmSounds[i].clip;
                bgmPlayer.Play();
            }
        }
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX(string p_sfxName)
    {
        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if (p_sfxName == sfxSounds[i].name)
            {
                for(int x = 0; x < sfxPlayer.Length; x++)
                {
                    if (!sfxPlayer[x].isPlaying)
                    {
                        sfxPlayer[x].clip = sfxSounds[i].clip;
                        sfxPlayer[x].Play();
                        return; // 굳이 다 찾았으면 돌 필요가 없음
                    }
                }
                Debug.LogWarning("all sfxPlayer is Playing");
                return;
            }
        }

        Debug.LogWarning("sfx param name isn't exist");
    }
}
