using System;
using System.Collections.Generic;
using System.Linq;
using MenuScreens;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This is a simple solution to managing multiple UI screens. It is used in Devastate, Die Up, and Starship Architect.
///
/// It allows you to:
///     Change screen -- SetScreen(MenuScreen)
///     Go back one screen -- BackOne()
///
/// In addition, it allows you to spread out your screens wherever you want in the scene,
/// so they can all be active and visible at once, making it much easier to work on them.
///
/// EXAMPLE USAGE:
/// The main menu of my game has a Home screen, a matchmaking screen, and a store screen.
///
/// I'd make one class for each screen, each extending MenuScreen.
///     public class HomeScreen : MenuScreen
///     public class MatchmakingScreen : MenuScreen
///     public class StoreScreen : MenuScreen
///
/// I'd then place my MenuScreenManager script onto any gameobject in my scene.
///
/// Anywhere in my scene (though probably as a direct child of the canvas, or some organizing object) I'd
/// create three empty gameobjects (one for each screen), and apply one Screen script to each.
///
/// Then I'd drag the home screen into the MenuScreenManager's startScreen field.
///
/// To transfer between screens, you can either reference the menuscreen manager to call SetScreen, or on MenuScreen,
/// You'll find a utility method called SetMeActive() 
/// 
/// </summary>
public class MenuScreenManager : MonoBehaviour
{
    public static MenuScreenManager Instance { get; private set; }
    
    [SerializeField] private bool initializeInAwake = true;
    public Canvas canvas;
    public List<MenuScreen> menuScreens = new List<MenuScreen>();
    public MenuScreen currentScreen = null;
    public Stack<MenuScreen> screenHistory = new Stack<MenuScreen>();

    private bool _initialized;
    
    [SerializeField] private MenuScreen startScreen;

    

    private void Awake()
    {
        if(initializeInAwake)
            Initialize();
    }

    public void Initialize()
    {
        if (!_initialized)
        {
            _initialized = true;
            Instance = this;
            menuScreens = FindObjectsOfType<MenuScreen>().ToList();
            menuScreens.ForEach((screen => screen.gameObject.SetActive(false)));
            SetScreen(startScreen);
        }
    }

    private void Update()
    {
        
    }

    public void SetScreen(MenuScreen screenToBe)
    {
        if (currentScreen is null)
        {
            screenToBe.transform.position = canvas.transform.position;
        }
        else
        {
            currentScreen.transform.position = currentScreen.StartPosition;
            currentScreen.gameObject.SetActive(false);
            
            screenToBe.transform.position = canvas.transform.position;
        }
        
        screenHistory.Push(currentScreen);
        currentScreen = screenToBe;
        currentScreen.gameObject.SetActive(true);
    }

    public void BackOne()
    {
        SetScreen(screenHistory.Pop());
        screenHistory.Pop();
    }
    
}