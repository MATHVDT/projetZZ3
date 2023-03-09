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
        _launcher = GameObject.Find("[UI] Launcher").GetComponent<Launcher>();
        if (_launcher == null) Debug.LogWarning("Pas de launcher dans la scène.");
    }

    public void Update()
    {
        if (_controls.buttonUp) { MenuPauseState = State.Resume; }
        else if (_controls.buttonDown) { MenuPauseState = State.Quit; }

        if (_controls.buttonA)
        {
            if (MenuPauseState == State.Resume) DisactivePause();
            else if (MenuPauseState == State.Quit) _launcher.ChargerLauncherGame();
        }

        _image.enabled = Pause;
        if (Pause)
        {
            _image.texture = MenuPauseTexture[(int)MenuPauseState];
        }
    }


    public void EnablePause()
    {
        Time.timeScale = 0.0f;
        Pause = true;
        MenuPauseState = State.Resume;

    }
    public void DisactivePause()
    {
        Time.timeScale = 1.0f;
        Pause = false;
    }


}
