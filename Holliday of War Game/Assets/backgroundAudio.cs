using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class backgroundAudio : MonoBehaviour {

    // Use this for initialization
    AudioClip HolyFright;
    AudioClip OhHaunted;

    AudioClip Hallolose;
    AudioClip Hallowin;
    AudioClip Christmas_Lose;
    AudioClip Christmas_Win;

    private PlayerSelection player;
    private Team playerTeam;

    AudioSource me;
    public int currentBeat;
    public int bps;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        player = GameObject.FindGameObjectWithTag("PlayerSelection").GetComponent<PlayerSelection>();
        playerTeam = player.myTeam();
        OhHaunted = Resources.Load<AudioClip>("Oh Come all Ye Haunted");
        HolyFright = Resources.Load<AudioClip>("O Holy Fright");
        Hallolose = Resources.Load<AudioClip>("Hallolose");
        Hallowin = Resources.Load<AudioClip>("Hallowin");
        Christmas_Lose = Resources.Load<AudioClip>("Christmas Lose");
        Christmas_Win = Resources.Load<AudioClip>("Christmas Win");
        me = GetComponent<AudioSource>();

        if (System.DateTime.Now.Second % 2 == 1)
        {
            me.clip = OhHaunted;
            bps = 4;
            StartCoroutine(keepTrackOfBeat( bps, 0.46153846153f));
        }
        else
        {
            me.clip = HolyFright;
            bps = 3;
            StartCoroutine(keepTrackOfBeat(bps, 0.4f));
        }

        me.Play();
        
    }

    public void playCorrectFanfar()
    {
        me = GetComponent<AudioSource>();
        me.Stop();
        if (SceneManager.GetActiveScene().buildIndex == 2) //2 should be lose
        {
            if (playerTeam.Equals(Team.Merry))
            {
                me.clip = Christmas_Lose;
            }
            else
            {
                me.clip = Hallolose;
            }
        }
        else
        {
            if (playerTeam.Equals(Team.Merry))
            {
                me.clip = Christmas_Win;
            }
            else
            {
                me.clip = Hallowin;
            }
        }
        me.Play();
    }

    private IEnumerator keepTrackOfBeat(int numberBeats, float bps)
    {
        yield return new WaitUntil(() => me.isPlaying);
        currentBeat = 0;
        while(true)
        {
            yield return new WaitForSeconds(bps);
            currentBeat = (currentBeat + 1) % numberBeats;
        }
    }

}
