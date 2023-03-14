using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPause : MonoBehaviour
{
    public enum State { Resume = 0, Quit = 1 }

    public Texture[] MenuPauseTexture;
    private RawImage _image;

    public bool Pause = false;
    public State MenuPauseState = State.Resume;

    public SetValueControls _controls;
    private Launcher _launcher;

    private void Start()
    {
        _image = GetComponent<RawImage>();
        if (_image == null) Debug.LogWarning("Raw image pas récup.");
        _controls = GameObject.Find("[UI] Controls").GetComponent<SetValueControls>();
        _launcher = GameObject.Find("[UI] Launcher").GetComponent<Launcher>();
        if (_launcher == null) Debug.LogWarning("Pas de launcher dans la scène.");
        DisactivePause();
    }

    public void Update()
    {
        if (Pause)
        {
            _image.texture = MenuPauseTexture[(int)MenuPauseState];
        }
    }

    public void SelectionUp()
    {
        if (Pause)
            MenuPauseState = State.Resume;
    }

    public void SelectionDown()
    {
        if (Pause)
            MenuPauseState = State.Quit;
    }

    public void ValidateSelection()
    {
        if (Pause)
        {
            if (MenuPauseState == State.Resume)
                DisactivePause();
            else if (MenuPauseState == State.Quit)
                _launcher.ChargerLauncherGame();
        }
    }

    public void EnablePause()
    {
        _image.enabled = true;
        Time.timeScale = 0.0f;
        Pause = true;
        MenuPauseState = State.Resume;

    }
    public void DisactivePause()
    {
        _image.enabled = false;
        Time.timeScale = 1.0f;
        Pause = false;
    }


}
