using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicCenter : MonoBehaviour
{
    public enum MusicCenterState
    {
        GROUP_LIST, SONG_LIST, SONG_DETAIL, SLIDING
    }
    public event EventHandler<OnStateChangeEventArgs> OnStateChange;
    public class OnStateChangeEventArgs : EventArgs
    {
        public MusicCenterState musicCenterState;
    }
    public static MusicCenter Instance { get; private set; }

    [SerializeField] MusicCenterState state;
    private void Awake()
    {
        Instance = this;
        state = MusicCenterState.GROUP_LIST;
    }

    public void ChangeState(MusicCenterState newState)
    {
        state = newState;
        OnStateChange?.Invoke(this, new OnStateChangeEventArgs { musicCenterState = newState });
    }
    public MusicCenterState GetMusicCenterState()
    {
        return state;
    }
    internal bool IsGROUP_LIST()
    {
        return state == MusicCenterState.GROUP_LIST;
    }
}
