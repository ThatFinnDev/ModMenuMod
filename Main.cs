using System;
using System.Reflection;
using Il2CppMonomiPark.SlimeRancher.UI.MainMenu;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Object = UnityEngine.Object;


namespace ModMenuMod
{
    public static class BuildInfo
    {
        public const string Name = "ModMenu"; // Name of the Mod.  (MUST BE SET)
        public const string Description = "A ModMenu for Slime Rancher 2"; // Description for the Mod.  (Set as null if none)
        public const string Author = "ThatFinn"; // Author of the Mod.  (MUST BE SET)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }


    public class ModMenuMod : MelonMod
    {
        bool mainMenuLoaded = false;
        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            if (sceneName == "MainMenuUI")
                mainMenuLoaded = false;
        }
        public override void OnUpdate()
        {
            if (mainMenuLoaded)
            {
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    if (modMenuUI != null)
                    {
                        Object.Destroy(modMenuUI.gameObject);
                        MainMenuLandingRootUI rootUI = Object.FindObjectOfType<MainMenuLandingRootUI>();
                        rootUI.transform.GetChild(0).gameObject.SetActive(true);
                        rootUI.enabled = true;
                        modMenuUI = null;
                        OnSceneWasInitialized(-1, "MainMenuUI");
                    }
                }
            }
        }
        public static GameObject modMenuUI;
        public override void OnSceneWasInitialized(int buildindex, string sceneName) // Runs when a Scene has Initialized and is passed the Scene's Build Index and Name.
        {
            MelonLogger.Msg("OnSceneWasInitialized: " + buildindex.ToString() + " | " + sceneName);
            if (sceneName == "MainMenuUI")
            {
                mainMenuLoaded = true;
                MainMenuLandingRootUI rootUI = Object.FindObjectOfType<MainMenuLandingRootUI>();
                Transform buttonHolder = rootUI.transform.GetChild(0).GetChild(3);
                for (int i = 0; i < buttonHolder.childCount; i++)
                {
                    if (buttonHolder.GetChild(i).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text == "Mods")
                        Object.Destroy(buttonHolder.GetChild(i).gameObject);
                }
                //Create Button
                GameObject newButton = Object.Instantiate(buttonHolder.GetChild(0).gameObject, buttonHolder);
                newButton.transform.SetSiblingIndex(3);
                newButton.transform.GetChild(0).GetComponent<CanvasGroup>().enabled = false;
                newButton.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Mods";
                //Fix RedButton SpaceBar Icon Dupe
                for (int i = 0; i < newButton.transform.GetChild(0).GetChild(0).GetChild(0).childCount; i++)
                    newButton.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(false);
                newButton.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);

                Button button = newButton.GetComponent<Button>();
                button.onClick = new Button.ButtonClickedEvent();
                button.onClick.AddListener((Action)(() =>
                {
                    MelonLogger.Msg("E");
                    //Create ModMenu
                    if (modMenuUI != null)
                        return;
                    rootUI.transform.GetChild(0).gameObject.SetActive(false);
                    rootUI.enabled = false;
                    GameObject modMenuRoot = new GameObject();
                    modMenuUI = modMenuRoot;
                    modMenuRoot.transform.SetParent(rootUI.transform);
                    modMenuRoot.name = "ModMenuRoot";
                    modMenuRoot.transform.localScale = new Vector3(1, 1, 1);
                    modMenuRoot.transform.localPosition = new Vector3(0, 0, 0);
                    modMenuRoot.transform.localRotation = Quaternion.identity;
                    modMenuRoot.AddComponent<RectTransform>();
                    modMenuRoot.AddComponent<Image>();
                    Color backgroundColor;
                    ColorUtility.TryParseHtmlString("#f0e1c8",out backgroundColor);
                    modMenuRoot.GetComponent<Image>().color = backgroundColor;
                    RectTransform rectTransform = modMenuRoot.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(Screen.currentResolution.width/1.23f, Screen.currentResolution.height/1.23f);
                    
                    GameObject modEntryHolder = Object.Instantiate(buttonHolder.gameObject, modMenuRoot.transform);
                    modEntryHolder.GetComponent<RectTransform>().pivot = Vector2.zero;
                    modEntryHolder.GetComponent<RectTransform>().anchorMax = Vector2.zero;
                    modEntryHolder.GetComponent<RectTransform>().anchorMin = Vector2.zero;
                    Vector2 leftLayoutGroupPosition = rootUI.transform.GetChild(0).GetChild(1).transform.localPosition;
                    modEntryHolder.transform.localPosition = new Vector3(leftLayoutGroupPosition.x*0.78f, leftLayoutGroupPosition.y*0.6f);
                    
                    TextMeshProUGUI text = Object.Instantiate(rootUI.transform.GetChild(0).GetChild(1).GetChild(1).gameObject, modMenuRoot.transform).GetComponent<TextMeshProUGUI>();
                    text.GetComponent<RectTransform>().pivot = Vector2.zero;
                    text.GetComponent<RectTransform>().anchorMax = Vector2.zero;
                    text.GetComponent<RectTransform>().anchorMin = Vector2.zero;
                    text.transform.localPosition = Vector3.zero;
                    Object.Destroy(text.GetComponent<LocalizedVersionText>());
                    
                    for (int i = 0; i < modEntryHolder.transform.childCount; i++)
                            Object.Destroy(modEntryHolder.transform.GetChild(i).gameObject);
                    
                    foreach (MelonBase melonBase in MelonBase.RegisteredMelons)
                    {
                        //Create ModMenuEntry
                        GameObject modMenuEntry = Object.Instantiate(buttonHolder.GetChild(0).gameObject, modEntryHolder.transform);
                        modMenuEntry.transform.GetChild(0).GetComponent<CanvasGroup>().enabled = false;
                        modMenuEntry.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = melonBase.Info.Name;
                        modMenuEntry.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().fontSizeMin = 0.1f;
                        modMenuEntry.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                        Vector2 oldSize = modMenuEntry.GetComponent<RectTransform>().sizeDelta;
                        modMenuEntry.GetComponent<RectTransform>().sizeDelta = new Vector2(oldSize.x * 1.5f, oldSize.y);
                        
                        //Fix RedButton SpaceBar Icon Dupe
                        for (int i = 0; i < modMenuEntry.transform.GetChild(0).GetChild(0).GetChild(0).childCount; i++)
                            modMenuEntry.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(false);
                        modMenuEntry.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
                        
                        //Add Onclick
                        button.onClick = new Button.ButtonClickedEvent();
                        modMenuEntry.GetComponent<Button>().onClick.AddListener((Action)(() =>
                        {
                            
                            text.GetComponent<TextMeshProUGUI>().SetText(melonBase.Info.Name+"\nby: "+melonBase.Info.Author+"\nVersion: "+melonBase.Info.Version+"\n");
                            
                            if (melonBase.Info.DownloadLink != null)
                                text.GetComponent<TextMeshProUGUI>().text += "DownloadLink: "+melonBase.Info.DownloadLink.Replace("\n","")+"\n";
                            
                            text.text += "\n";
                            var universalMod = melonBase.MelonAssembly.Assembly.GetCustomAttribute<MelonGameAttribute>();
                            if(universalMod!=null)
                                text.text += "IsUniversalMod: " + universalMod.Universal+"\n";
                            else
                                text.text += "IsUniversalMod: Unknown";
                            
                            
                            
                            var desc = melonBase.MelonAssembly.Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
                            if(desc!=null)
                                if (!String.IsNullOrEmpty(desc.Description))
                                    text.text += "Description: " + desc.Description+"\n";
                        }));
                    }
                }));
            }
        }
    }
}