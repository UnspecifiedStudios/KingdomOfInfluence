using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public Button playButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playButton.onClick.AddListener(() => {
            SceneManagerScript.Instance.LoadScene("WorldMap");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
