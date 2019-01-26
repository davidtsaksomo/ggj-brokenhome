using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicControl : MonoBehaviour
{
    public AudioMixerSnapshot mainOst;
    public AudioMixerSnapshot harmony;
    public AudioMixerSnapshot sad;
    public AudioMixerSnapshot doubt;
    public AudioMixerSnapshot happy;

    public float bpm = 128;
    private float m_TransitionIn;
    private float m_TransitionOut;
    private float m_QuarterNote;

    
    // Start is called before the first frame update
    void Start()
    {
        m_QuarterNote = 60/bpm;
        m_TransitionIn = m_QuarterNote;
        m_TransitionOut = m_QuarterNote * 16;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("mainOst")){
            mainOst.TransitionTo(m_TransitionOut);
        }
        if(other.CompareTag("harmony")){
            harmony.TransitionTo(m_TransitionOut);
        }
        if(other.CompareTag("sad")){
            sad.TransitionTo(m_TransitionOut);
        }
        if(other.CompareTag("doubt")){
            doubt.TransitionTo(m_TransitionOut);
        }
        if(other.CompareTag("happy")){
            happy.TransitionTo(m_TransitionOut);
        }
    }
}
