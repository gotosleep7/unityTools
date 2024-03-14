using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SongListUI : MonoBehaviour
{
    CanvasGroup canvasGroup;

    public PointerEventData OnPointerUp { get; private set; }
    private SongListPanelCtrl songListPanelCtrl;

    private void Awake()
    {
        songListPanelCtrl = GetComponent<SongListPanelCtrl>();
    }

    private void Start()
    {

        MusicCenter.Instance.OnStateChange += MusicCenter_OnStateChange;
        songListPanelCtrl.OnCompleteAnimationExit.AddListener(() => { Hide(); });
        Hide();
    }

    private void MusicCenter_OnStateChange(object sender, MusicCenter.OnStateChangeEventArgs e)
    {
        if (e.musicCenterState == MusicCenter.MusicCenterState.SONG_LIST || e.musicCenterState == MusicCenter.MusicCenterState.SONG_DETAIL)
        {
            Show();
            songListPanelCtrl.OnEnter();
        }
        else
        {

        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    public void ForDebug()
    {

        songListPanelCtrl.OnExit();
        MusicCenter.Instance.ChangeState(MusicCenter.MusicCenterState.GROUP_LIST);

    }
}
