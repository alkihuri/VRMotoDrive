using UnityEngine;

public static class PathFinder
{

    public static string ConfigsPath
    {
        get
        {
            var path = Application.streamingAssetsPath;
            switch (Application.platform)
            {

                case RuntimePlatform.Android:
                    path = Application.persistentDataPath;
                    break;
                case RuntimePlatform.WindowsEditor:
                    path = Application.streamingAssetsPath;
                    break;
                default:
                    path = Application.streamingAssetsPath;
                    break;   
            }
            return path;    
        }
    }


}