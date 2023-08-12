using UnityEngine;
using UnityEngine.Scripting;
using qin_makeface;

[Preserve]
public class InitController
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        BaseArchitecture.InitArchitecture();
    }
}