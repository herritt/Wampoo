using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject soloMarbleSelect;
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

        GameManager.Instance.player = colour;

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
