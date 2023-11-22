using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Haptics;

public class HapticManager : MonoBehaviour
{
    private static HapticManager _instance = null;

    public static HapticManager Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject obj = new GameObject("HapticManager");
                obj.AddComponent<HapticManager>();
                _instance = obj.GetComponent<HapticManager>();
            }
            return _instance;
        }
        private set { _instance = value; }
    }

    public HapticClip clip1;
    public HapticClip clip2;
    private HapticClipPlayer player;
    public float debugAmp, debugFreq;
    public int hapticCnt;
    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
        hapticCnt = 0;
        player = new HapticClipPlayer(clip1);
        player.isLooping = false;
    }

    public void PlayHapticClip1()
    {
        player.clip = clip1;
        player.Play(Controller.Right);
    }
    public void PlayHapticClip2()
    {
        player.clip = clip2;
        player.Play(Controller.Right);
    }
    public void StopHaptics()
    {
        player.Stop();
    }
    public void HapticLoopOn()
    {
        player.isLooping = true;
    }
    public void HapticLoopOff()
    {
        player.isLooping = false;
    }

    private void OnDestroy()
    {
        player.Dispose();
        _instance = null;
    }

    private void OnApplicationQuit()
    {
        Haptics.Instance.Dispose();
    }

    private void Update()
    {
        player.amplitude = debugAmp;
        player.frequencyShift = debugFreq;
    }
}
