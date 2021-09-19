using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public enum PlayerColour { Red, Blue, Green, Yellow };

    public GameObject mainMenu;
    public GameObject soloMarbleSelect;
    private PlayerColour colour;
    public GameObject popUp;
    public Text popUpText;

    // Start is called before the first frame update
    void Start()
    {
        soloMarbleSelect.SetActive(false);
        popUp.SetActive(false);
    }

    public void OnLocalGameSelect()
    {
        mainMenu.SetActive(false);
        soloMarbleSelect.SetActive(true);
    }

    public void OnLocalMarbleColourSelect(int colour)
    {
        soloMarbleSelect.SetActive(false);
        popUp.SetActive(true);

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

    public void OnPopUpOkButton()
    {
        popUp.SetActive(false);

    }

    public void ShowPopUpWithMessage(string message)
    {
        popUp.SetActive(true);
        popUpText.text = message;

    }
}
