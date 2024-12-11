using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    public static bool Online = true;
    public static int Seed;

    public static float Brightness;
    public static float Contrast;
    public static float Saturation;

    public static float GeneralVolume;
    public static float SoundVolume;
    public static float MusicVolume;

    public static bool AutoSave = true;

    public static bool HelpMessages = true;

    public static bool Tutorial = true;
    public static bool FrameTutorial = true;
    public static bool ItemTutorial = true;
    public static bool RemainingMagicTutorial = true;

    public static bool LoadedWorld = false;
    public static bool CanRevive = true;
    public static bool LoadedControls = false;
    public static bool ExistsPreviousGame = false;

    //Items
    public static bool Owl = false;
    public static bool ElectricFingers = false;
}