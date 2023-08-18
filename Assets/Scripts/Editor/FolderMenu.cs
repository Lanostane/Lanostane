using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public static class FolderMenu
{
    [MenuItem("Folder/Open Persistent Path")]
    public static void OpenPersistentDataPath()
    {
        Process.Start(Application.persistentDataPath);
    }
}
