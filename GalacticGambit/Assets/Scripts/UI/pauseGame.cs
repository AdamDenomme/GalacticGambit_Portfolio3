using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseGame : MonoBehaviour
{
    bool _paused = false;

    public void pauseGameFromSettings()
    {
        if (_paused == false)
        {
            _paused = true;
            Time.timeScale = 0;
        }
        else if (_paused == true) 
        {
            _paused = false;
            Time.timeScale = 1;
        }

    }
}
