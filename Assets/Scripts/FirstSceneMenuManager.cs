using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceneMenuManager : MonoBehaviour {

    public FirstSceneMenu CurrentMenu;

	public void Start()
	{
        ShowMenu(CurrentMenu);
	}

    public void ShowMenu(FirstSceneMenu menu){
        if (CurrentMenu != null){
            CurrentMenu.IsOpen = false;
        }

        CurrentMenu = menu;
        CurrentMenu.IsOpen = true;
    }

}
