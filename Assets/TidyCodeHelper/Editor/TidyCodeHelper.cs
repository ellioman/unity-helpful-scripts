using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;

public class TidyCodeHelper : ScriptableObject {

    [MenuItem("Tidy Code Helper/Find Empty functions")]
    private static void FindEmptyFunctions()
    {
        FindAndDisplayUsingRegex(
            @"(?<!override\s)(?<!override\sprivate\s)(?<!override\sprotected\s)(?<!override\spublic\s)(?<!virtual\s)(?<!virtual\sprivate\s)(?<!virtual\sprotected\s)(?<!virtual\spublic\s)((private\svoid\s?|public\svoid\s?|void))\s+(Awake|Start|OnEnable|Update|FixedUpdate|LateUpdate|OnGUI|OnPostRender|OnPreCull|OnPreRender|OnPostRender|OnRenderObject|OnTriggerEnter|OnTriggerEnter2D|OnTriggerExit|OnTriggerExit2D|OnTriggerStay|OnTriggerStay2D|OnCollisionEnter|OnCollisionEnter2D|OnCollisionExit|OnCollisionExit2D|OnCollisionStay|OnCollisionStay2D|OnMouseDown|OnMouseDrag|OnMouseEnter|OnMouseOver|OnMouseExit|OnMouseUp|OnBecameVisible|OnBecameInvisible|OnDisable|OnDestroy|OnApplicationQuit|OnApplicationPause|OnApplicationFocus)\s*?\(.*\)\s*\{\n*?\s*?\}"
        );
    }

    [MenuItem("Tidy Code Helper/Find Heavy Function Calls")]
    private static void FindHeavyFunctionCalls()
    {
        FindAndDisplayUsingRegex(
            @"(Update|FixedUpdate|LateUpdate|OnGUI|OnPostRender|OnPreCull|OnPreRender|OnPostRender|OnRenderObject|OnTriggerStay|OnTriggerStay2D|OnCollisionStay|OnCollisionStay2D|OnMouseDown|OnMouseDrag|OnMouseEnter|OnMouseOver|OnMouseExit|OnMouseUp|OnBecameVisible|OnBecameInvisible)+\s*\(.*\)\s*\{\s*(\w*.*\s*(GetComponents|GetComponentInChildren|GetComponentsInChildren|GetComponentInParent|GetComponentsInParent|GetComponent|FindObjectOfType|FindObjectsOfType|FindObjectsOfTypeAll|FindObjectsOfTypeIncludingAssets|FindSceneObjectsOfType|Find|SendMessage|SendMessageUpwards)+)\s*(\(|\<)+(\s*\w.*\s*)*\s*\}"
        );
    }

    private static void FindAndDisplayUsingRegex(string regexPattern)
    {
        const string removeLineBreaksPattern = @"/(\r\n)+|\r+|\n+|\t+";
        const int lineNumber = 0;
        const int lineColumn = 0;

        Dictionary<string, string> scripts = GetAssetsOfType("cs");
        Regex rgx = new Regex(regexPattern);
        MethodBase mUnityLog = typeof(UnityEngine.Debug).GetMethod("LogPlayerBuildError", BindingFlags.NonPublic | BindingFlags.Static);
        foreach (KeyValuePair<string, string> kv in scripts)
        {
            MatchCollection matches = rgx.Matches(kv.Value);
            if (matches.Count > 0)
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    string text = "[" + kv.Key + "]\n";
                    text += Regex.Replace(matches[i].Value, removeLineBreaksPattern, ""); // Removes line breaks, tabs, etc.
                    mUnityLog.Invoke(null, new object[] { text, kv.Key, lineNumber, lineColumn } );
                }
            }
        }
    }

    private static Dictionary<string, string> GetAssetsOfType(string fileEnding)
    {
        Dictionary<string, string> scriptsDict = new Dictionary<string, string>();
        DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
        FileInfo[] goFileInfo = directory.GetFiles("*" + fileEnding, SearchOption.AllDirectories);

        int i = 0; 
        int goFileInfoLength = goFileInfo.Length;
        FileInfo tempGoFileInfo; 
        string tempFilePath;
        string codeString;
        for (; i < goFileInfoLength; i++)
        {
            tempGoFileInfo = goFileInfo[i];
            if (tempGoFileInfo == null)
            {
                continue;
            }

            tempFilePath = tempGoFileInfo.FullName;
            tempFilePath = tempFilePath.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
            codeString = System.IO.File.ReadAllText(@tempFilePath);
            scriptsDict.Add(tempFilePath, codeString);
        }

        return scriptsDict;
    }
}
