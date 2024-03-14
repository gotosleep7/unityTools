using System;
using UnityEngine;

public class ThreeDScrollControllerForMusic : ThreeDScrollController
{

    public static ThreeDScrollControllerForMusic Instance { get; private set; }
    public override void Awake()
    {
        Instance = this;
        base.Awake();
        OnAutoAlignComplateEvent += OnAutoAlignComplate;

    }
    private void Start()
    {
        MusicCenter.Instance.OnStateChange += MusicCenter_OnStateChange;
    }

    private void MusicCenter_OnStateChange(object sender, MusicCenter.OnStateChangeEventArgs e)
    {
        if (e.musicCenterState == MusicCenter.MusicCenterState.GROUP_LIST)
        {
            EnableDrag();
        }
        else
        {
            DisableDrag();
        }
    }

    private void OnAutoAlignComplate(object sender, ThreeDScrollItem e)
    {
        // MusicCenter.Instance.ChangeState(MusicCenter.MusicCenterState.SONG_LIST);
    }

    public override void OnMouseButtonUp()
    {
        // Input.mousePosition
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, int.MaxValue))
        {
            if (hitInfo.transform.gameObject.TryGetComponent<ThreeDButton>(out var btn))
            {
                if (ThreeDScroll.Instance.IsMoveing()) return;
                MusicCenter.Instance.ChangeState(MusicCenter.MusicCenterState.SONG_LIST);
                btn.Click();
            }
        }
    }
}
