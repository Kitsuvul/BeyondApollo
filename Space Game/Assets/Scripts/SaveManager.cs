using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;

    private static SaveManager Instance {  get { return instance; } }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else { instance = this; }

        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolumeInPlayerPrefs(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetVolumeInPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            return PlayerPrefs.GetFloat("Volume");
        }
        else { return 0.1f; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="volume"></param>
    public void SetSFXVolumeInPlayerPrefs(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetSFXVolumeInPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            return PlayerPrefs.GetFloat("SFXVolume");
        }
        else { return 0.1f; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isMuted"></param>
    public void SetIsMutedInPlayerPrefs(bool isMuted)
    {
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool GetIsMutedInPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("IsMuted"))
        {
            return PlayerPrefs.GetInt("IsMuted") != 0;
        }
        else { return false; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isMuted"></param>
    public void SetIsTutorialCompleteInPlayerPrefs(bool hasCompleted)
    {
        PlayerPrefs.SetInt("TutorialComplete", hasCompleted ? 1 : 0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isFlickering"></param>
    public void SetUIFlickerInPlayerPrefs(bool isFlickering)
    {
        PlayerPrefs.SetInt("IsFlickering", isFlickering ? 1 : 0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool GetIsTutorialCompleteInPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("TutorialComplete"))
        {
            return PlayerPrefs.GetInt("TutorialComplete") != 0;
        }
        else { return false; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isMuted"></param>
    public void SetIsEndlessTutorialCompleteInPlayerPrefs(bool hasCompleted)
    {
        PlayerPrefs.SetInt("EndlessTutorialComplete", hasCompleted ? 1 : 0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool GetIsEndlessTutorialCompleteInPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("EndlessTutorialComplete"))
        {
            return PlayerPrefs.GetInt("EndlessTutorialComplete") != 0;
        }
        else { return false; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="level"></param>
    public void SetHighestLevelInPlayerPrefs(int level)
    {
        PlayerPrefs.SetInt("Level", level);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetHighestLevelInPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            return PlayerPrefs.GetInt("Level");
        }
        else { return 1; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="level"></param>
    public void SetEndlessHighScoreInPlayerPrefs(int score)
    {
        PlayerPrefs.SetInt("EndlessScore", score);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetEndlessHighScoreInPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("EndlessScore"))
        {
            return PlayerPrefs.GetInt("EndlessScore");
        }
        else { return 0; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isFlickering"></param>
    public bool GetUIFlickerInPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("IsFlickering"))
        {
            return PlayerPrefs.GetInt("IsFlickering") != 0;
        }
        else { return true; }
    }
}