using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public enum PlayerColour { Red, Blue, Green, Yellow };

    public GameObject mainMenu;
    public GameObject soloMarbleSelect;
    private PlayerColour colour;

    // Start is called before the first frame update
    void Start()
    {
        soloMarbleSelect.SetActive(false);
    }

    public void OnLocalGameSelect()
    {
        mainMenu.SetActive(false);
        soloMarbleSelect.SetActive(true);
    }

    public void OnLocalMarbleColourSelect(int colour)
    {
        soloMarbleSelect.SetActive(false);

        switch ((PlayerColour) colour)
        {
            case (PlayerColour.Red):
                Debug.Log("red");
                break;
            case (PlayerColour.Blue):
                Debug.Log("blue");
                break;
            case (PlayerColour.Green):
                Debug.Log("green");
                break;
            case (PlayerColour.Yellow):
                Debug.Log("yellow");
                break;
        }
    }
}
