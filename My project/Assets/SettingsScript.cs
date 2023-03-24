using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//textmeshpro
using TMPro;

public class SettingsScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float BottomClampFov = 40f;
    public float TopClampFov = 120f;

    public float sensitivitytop = 10f;
    public float sensitivitybottom = 0f;


    [SerializeField]
    private Slider sensitivitySlidery;
    [SerializeField]
    //textmeshpro
    private TMP_Text sensitivityText;
  
    [SerializeField]
    private Slider fovSlider;
    [SerializeField]
    //textmeshpro
    private TMP_Text fovText;
    [SerializeField]
    private Button saveButton;
    void Awake()
    {
        SettingsFile.ReadFile();
        fovSlider.value = (convertString(SettingsFile.getVariable("FOV"))-BottomClampFov)/(TopClampFov-BottomClampFov);
        sensitivitySlidery.value = (convertString(SettingsFile.getVariable("sensitivity"))-sensitivitybottom)/(sensitivitytop-sensitivitybottom);
        fovText.text = SettingsFile.getVariable("FOV");
        sensitivityText.text = SettingsFile.getVariable("sensitivity");
        saveButton.onClick.AddListener(() => {
            SettingsFile.setVariable("FOV", fovText.text);
            SettingsFile.setVariable("sensitivity", sensitivityText.text);
            SettingsFile.saveSettings();
            Debug.Log("Saved settings");
        });
        fovSlider.onValueChanged.AddListener((float value) => {
            string temp = (value*(TopClampFov-BottomClampFov)+BottomClampFov).ToString();
            //limit to first 4 characters
            temp=temp.Substring(0,Mathf.Min(temp.Length,4));
            fovText.text = temp;
        });
        sensitivitySlidery.onValueChanged.AddListener((float value) => {
            string temp = (value*(sensitivitytop-sensitivitybottom)+sensitivitybottom).ToString();
            //limit to first 4 characters
            temp=temp.Substring(0,Mathf.Min(temp.Length,4));
            sensitivityText.text = temp;
        });


    }

    public float convertString(string s){
        float f = 0f;
        float.TryParse(s, out f);
        return f;
    }

}
