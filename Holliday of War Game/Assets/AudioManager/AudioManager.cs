using UnityEngine.Audio;
using System;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour 
{
    [Header("Change size number to add new clips")]
    public Sound[] music; //Play a song using : FindObjectOfType<AudioManager>().PlayMusic("SongName");
    public Sound[] sounds;//Play a sound using : FindObjectOfType<AudioManager>().PlaySound("SongName");

    public static AudioManager instance;

    private Sound currentSong;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (var m in music)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;
            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
        }

        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // Use this for initialization
    void Start()
    {
        PlayMusic("OhComeAllYeHaunted");
        //SongSelect //Un-comment to randomly select a song to play on Start
    }

    // Update is called once per frame
    void Update()
    {
        //PlayNextSong(); //Un-comment to play a new song at the end of the last song.
    }

    public void PlayMusic(string name)
    {
        Sound m = Array.Find(music, music => music.name == name);
        if (m == null)
        {
            Debug.LogWarning("Song with " + name + " not found!");
            return;
        }
        m.source.Play();
        currentSong = m;
    }

    public void StopMusic(string name)
    {
        Sound m = Array.Find(music, music => music.name == name);
        if (m == null)
        {
            Debug.LogWarning("Song with " + name + " not found!");
            return;
        }
        m.source.Stop();
    }


    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound with " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    //Play a random song add or remove songs as needed
    void SongSelect() // randomly select a song to play
    {
        System.Random rnd = new System.Random();
        int num = rnd.Next(1, 4);
        if (num == 1)
        {
            PlayMusic("Track01");
        }

        if (num == 2)
        {
            PlayMusic("Track02");
        }

        if (num == 3)
        {
            PlayMusic("Track03");
        }

    }

    //Play a random song after the end of the last song.
    public void PlayNextSong ()
    {
        if (currentSong.source.isPlaying == false)
        {
            SongSelect();
        }
    }
}
