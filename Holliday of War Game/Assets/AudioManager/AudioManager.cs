using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private PlayerSelection player;
    private Team playerTeam;

    public int currentBeat;
    public int bps; //beats per second

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
        playKringle();

        //SongSelect //Un-comment to randomly select a song to play on Start
    }

    // Update is called once per frame
    void Update()
    {
        //PlayNextSong(); //Un-comment to play a new song at the end of the last song.
    }
    //_______________Cease Songs Immediately_________________________________________
    public void stopAnyMusic()
    {
        currentSong.source.Stop();
    }
       
    //_______________Custom Function To Play Kringle Bells Correctly__________________
    public void playKringle()
    {
        PlayMusic("KringleBellsIntro");
        StartCoroutine(playRightAfter("KringleBellsMain"));
    }
    private IEnumerator playRightAfter(String next)
    {
        yield return new WaitUntil(() => currentSong.source.isPlaying == false);
        PlayMusic(next);
    }
    //_______________________________________________________________________________|

    //_______________Custom Function To Play Right Takeover___________________________
    public void playTakeoverSound(Team overtakerTeam)
    {
        if (overtakerTeam.Equals(Team.Spooky))
        {
            if (System.DateTime.Now.Second % 2 == 1)
            {
                PlaySound("HalloweenTakeover1");
            }
            else
            {
                PlaySound("HalloweenTakeover2");
            }
        }
        else
        {
            if (System.DateTime.Now.Second % 2 == 1)
            {
                PlaySound("ChristmasTakeover1");
            }
            else
            {
                PlaySound("ChristmasTakeover2");
            }
        }
    }

    //_______________________________________________________________________________|

    public void PlayMusic(string name)
    {
        if (currentSong != null && currentSong.source.isPlaying == true)
        {
            return;
        }

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
    //________________________Also Wanna Stop Sound_________________
    public void StopSound(String name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound with " + name + " not found!");
            return;
        }
        s.source.Stop();
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
    //____________________Functions to assure things happen in new scene______
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex > 0)
        {
            StopMusic("KringleBellsMain");
            player = GameObject.FindGameObjectWithTag("PlayerSelection").GetComponent<PlayerSelection>();
            playerTeam = player.myTeam();

            if (System.DateTime.Now.Second % 2 == 1)
            {
                bps = 4;
                StartCoroutine(keepTrackOfBeat(bps, 0.46153846153f));
                PlayMusic("OhComeAllYeHaunted");
            }
            else
            {
                bps = 3;
                StartCoroutine(keepTrackOfBeat(bps, 0.4f));
                PlayMusic("OHolyFright");
            }
        }
    }
    //________________________________________________________________________|

    //_____________________Coroutine To Allow Syncing To Beat____
    private IEnumerator keepTrackOfBeat(int numberBeats, float bps)
    {
        yield return new WaitUntil(() => currentSong.source.isPlaying);
        currentBeat = 0;
        while (true)
        {
            yield return new WaitForSeconds(bps);
            currentBeat = (currentBeat + 1) % numberBeats;
        }
    }
    //________________________________________________________________________|

    //Play a random song after the end of the last song.
    public void PlayNextSong ()
    {
        if (currentSong.source.isPlaying == false)
        {
            SongSelect();
        }
    }
}
