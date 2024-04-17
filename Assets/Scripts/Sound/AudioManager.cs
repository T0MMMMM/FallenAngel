using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    private PlayerManagerScript _player;
    public Sound[] sounds;

    private int i = 1;


    void Awake()
    {
        foreach (Sound s in sounds) 
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
        _player = PlayerManagerScript.instance;
    }

    void Start()
    {
        /*Play("Forest");
        Play("Birds");*/
    }


    public void Play(string name)
    {
       Sound s = Array.Find(sounds, sound => sound.name == name);
       s.source.Play();
    }

    public void PlayDelayed(string name, float delay)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.PlayDelayed(delay);
    }


    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }


    public void Update()
    {
        playSounds();
    }
    

    public void playSounds()
    {
        if (_player._data.dashing)
        {
            Play("Dash Cartoon");
        }

        if (_player._data.horizontalInput != 0 && !_player._data.isOnWall && _player._data.isGrounded && !_player._data.dashing && !_player._data.isSliding) 
        {
            if (!Array.Find(sounds, sound => sound.name == "walk1").source.isPlaying && !Array.Find(sounds, sound => sound.name == "walk2").source.isPlaying && !Array.Find(sounds, sound => sound.name == "walk3").source.isPlaying && !Array.Find(sounds, sound => sound.name == "walk4").source.isPlaying)//(!Array.Find(sounds, sound => sound.name.Contains("walk")).source.isPlaying)
            {
                Play("walk" + i.ToString());
                i++;
                if (i > 4)
                {
                    i = 1;
                }
            }
            //PlayDelayed("walk1", Array.Find(sounds, sound => sound.name == "walk1").source);

        } else
        {
            Stop("walk1");
            Stop("walk2");
            Stop("walk3");
            Stop("walk4");
        }

    }
}
