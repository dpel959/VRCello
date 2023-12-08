using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Haptics;

public class HapticManager : Singleton<HapticManager>
{
    public HapticClip clip1;
    public HapticClip clip2;
    private HapticClipPlayer player;
    public float debugAmp, debugFreq;
    public int hapticCnt;
    // Start is called before the first frame update
    void Awake()
    {
        hapticCnt = 0;
        player = new HapticClipPlayer(clip1);
        player.isLooping = false;
    }

    public void SetHapticClip1()
    {
        player.clip = clip1;
        player.Play(Controller.Both);
    }
    public void SetHapticClip2()
    {
        player.clip = clip2;
        player.Play(Controller.Both);
    }

    public void PlayHapticBoth()
    {
        player.Play(Controller.Both);
    }
    public void PlayHapticLeft()
    {
        player.Play(Controller.Left);
    }

    public void PlayHapticRight()
    {
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
    }

    private void OnApplicationQuit()
    {
        Haptics.Instance.Dispose();
    }

    private void Update()
    {
        if (CelloHand.Instance.isStringed)
        {
            if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
            {
                player.Play(Controller.Left);
            }
            if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
            {
                player.Play(Controller.Left);
            }
            if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
            {
                player.Play(Controller.Left);
            }
            if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger))
            {
                player.Play(Controller.Left);
            }
        }
    }
}
