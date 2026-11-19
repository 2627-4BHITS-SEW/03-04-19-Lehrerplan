using UnityEngine;

public class SchoolHelp : MonoBehaviour
{
    [SerializeField] GameObject helpPanel;

    void Start() { helpPanel.SetActive(false); }

    public void ToggleHelp() { helpPanel.SetActive(!helpPanel.activeSelf); }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            ToggleHelp();
        if (Input.GetKeyDown(KeyCode.Escape))
            helpPanel.SetActive(false);
    }
}
