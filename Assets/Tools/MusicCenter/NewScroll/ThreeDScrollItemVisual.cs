using System;
using TMPro;
using UnityEngine;

public class ThreeDScrollItemVisual : MonoBehaviour
{

    [SerializeField] ThreeDScrollItem scrollItem;
    [SerializeField] GameObject box;
    [SerializeField] GameObject activeVisual;
    [SerializeField] Material material;
    [SerializeField] SpriteRenderer cover;
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text subTitle;
    public Material GetBgMaterial()
    {
        return material;
    }
    private void Start()
    {
        MusicCenter.Instance.OnStateChange += MusicCenter_OnstateChange;
        scrollItem.OnCurrentProgressChange += ThreeDScrollItem_OnCurrentProgressChange;
        HideVisual();
    }

    private void MusicCenter_OnstateChange(object sender, MusicCenter.OnStateChangeEventArgs e)
    {

        if (e.musicCenterState == MusicCenter.MusicCenterState.GROUP_LIST)
        {
        }
    }

    private void ThreeDScrollItem_OnCurrentProgressChange(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    // Update is called once per frame
    void Update()
    {
        if (scrollItem.CurrentProgress == 0 && MusicCenter.Instance.IsGROUP_LIST())
        {
            ShowVisual();
        }
        else if (scrollItem.CurrentProgress == 0)
        {
            box.SetActive(false);
            activeVisual.SetActive(true);
        }
        else
        {
            HideVisual();
        }
    }

    public void HideVisual()
    {
        box.SetActive(false);
        activeVisual.SetActive(false);
    }
    public void ShowVisual()
    {
        box.SetActive(true);
        activeVisual.SetActive(true);
    }

    public void UpdateVisual()
    {
        ThreeDScrollControllerForMusic.Instance.GetDataByProgress(scrollItem.CurrentProgress, out KeyDataSO currentData, out KeyDataSO nextData, out float lerpValue);
        transform.localPosition = Vector3.Lerp(currentData.position, nextData.position, lerpValue);
        transform.rotation = Quaternion.Euler(Vector3.Lerp(currentData.roatation, nextData.roatation, lerpValue));
        transform.localScale = Vector3.Lerp(currentData.scale, nextData.scale, lerpValue);
        // if (keyDataSO.textAlpha == 0) transform.localScale = Vector3.zero;


        // transform.localPosition = keyDataSO.position;
        //         transform.localScale = keyDataSO.scale;
        title.transform.localPosition = Vector3.Lerp(currentData.titlePosition, nextData.titlePosition, lerpValue);
        subTitle.transform.localPosition = Vector3.Lerp(currentData.subTitlePosition, nextData.subTitlePosition, lerpValue);
        float alpha = Mathf.Lerp(currentData.textAlpha, nextData.textAlpha, lerpValue);
        title.color = new Color(title.color.r, title.color.b, title.color.b, alpha);
        subTitle.color = new Color(subTitle.color.r, subTitle.color.b, subTitle.color.b, alpha);
        //         cover.color = new Color(subTitle.color.r, subTitle.color.b, subTitle.color.b, keyDataSO.textAlpha);
        //         if (keyDataSO.textAlpha == 0) transform.localScale = Vector3.zero;
    }
}
