using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTest : MonoBehaviour
{
    public PlayableAsset testCutscene;
    public bool played = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!played)
        {
            played = true;
            
                CutsceneManager.S.PlayCutscene(testCutscene);
        }
    }
}
