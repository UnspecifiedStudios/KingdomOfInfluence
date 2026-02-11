using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager S;
    [SerializeField]
    private PlayableDirector director;

    void Awake()
    {
        director.stopped += OnCutsceneEnded;
    }

    // play custscene
    public void PlayCutscene(PlayableAsset cutscene)
    {
        // can enable/disable other systems here as needed
        director.playableAsset = cutscene;
        director.Play();
    }

    private void OnCutsceneEnded(PlayableDirector d)
    {
        EndCutscene();
    }

    private void EndCutscene()
    {
        // Re-enable player control
        // Enable UI
        // Trigger next gameplay event
        Debug.Log("Cutscene finished");
    }
}
