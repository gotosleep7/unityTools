using UnityEngine;

[CreateAssetMenu(fileName = "KeyDataSO", menuName = "BMW/KeyDataSO", order = 0)]
public class KeyDataSO : ScriptableObject
{
    public Vector3 position;
    public Vector3 roatation;
    public Vector3 scale;
    public Vector3 titlePosition;
    public Vector3 subTitlePosition;
    public float textAlpha;
    public Sprite backGround;
}