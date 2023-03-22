using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISwitcher : MonoBehaviour
{
    [SerializeField]
    private Button toSettingsButton;
    [SerializeField]
    private Button toMainMenuButton;

    [SerializeField]
    private GameObject[] screens;

    private void Awake()
    {
        toSettingsButton.onClick.AddListener(() => {
            SwitchToScreen(1);
            Debug.Log("Switched to settings screen");
        });
        toMainMenuButton.onClick.AddListener(() => {
            SwitchToScreen(0);
            Debug.Log("Switched to main menu screen");
        });
    }





    public void SwitchToScreen(int screenIndex)
    {
        for (int i = 0; i < screens.Length; i++)
        {
            screens[i].SetActive(i == screenIndex);
        }
    }
}
