using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UImanager : MonoBehaviour
{
    public bool inMenu;
    public GameObject HotkeyUI;
    public Transform HotkeysArea;
    List<GameObject> ExistingHotkeys = new List<GameObject>();
    HomieCamera homieCamera;
    HomieMovement movement;
    public enum UIstates{menu,ingame,mounted}

    private void Start()
    {
        homieCamera = FindObjectOfType<HomieCamera>();
        movement = FindObjectOfType<HomieMovement>();

    }
    public void HideUI(CanvasGroup canvas)
    {
        canvas.alpha = 0;
        canvas.blocksRaycasts = false;
    }
    public void ShowUI(CanvasGroup canvas)
    {
        canvas.alpha = 1;
        canvas.blocksRaycasts = true;
    }
    public void ChangeUIState(UIstates state)
    {
        switch (state)
        {
            case UIstates.ingame:
                movement.canmove = true;
                homieCamera.canRotate = true;

                break;
            case UIstates.menu:
                movement.canmove = false;
                homieCamera.canRotate = false;


                break;
            case UIstates.mounted:
                movement.canmove = false;
                homieCamera.canRotate = true;




                break;
        }


    }
    public void CreateHotkeyUI(string hotkey,string desc)
    {
        bool duplicate = false;

        foreach(GameObject i in ExistingHotkeys)
        {
            if(i.GetComponent<HotkeyIcon>().Desc.text == desc && i.GetComponent<HotkeyIcon>().buttonText.text == hotkey)
            {
                duplicate = true;

            }
        }
        if (!duplicate)
        {
            GameObject h = Instantiate(HotkeyUI, HotkeysArea);
            ExistingHotkeys.Add(h);
            h.GetComponent<HotkeyIcon>().UpdateInfo(hotkey, desc);
        }
      
    }
    public void DeleteHotkeyUI(string hotkey, string desc)
    {
        GameObject todelete = null;
        if (ExistingHotkeys.Count > 0)
        {
            foreach (GameObject i in ExistingHotkeys)
            {
                if (i.GetComponent<HotkeyIcon>().Desc.text == desc && i.GetComponent<HotkeyIcon>().buttonText.text == hotkey)
                {
                    todelete = i;
                }
            }
                    ExistingHotkeys.Remove(todelete);
                    Destroy(todelete);
        }

    }


}
