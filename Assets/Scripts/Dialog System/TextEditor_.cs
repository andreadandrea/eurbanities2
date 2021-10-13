using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO; 
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
public class TextEditor_ : EditorWindow
{
    [MenuItem("CustomWindows/TextEditor")]
    public static void ShowWindow()
    {
        GetWindow<TextEditor_>("TextEditor");
    }
    public EditorCoroutine coroutine;
    string txt = "";
    string previousAutoSaveText = "";
    string explenationTxt = "";
    string intInsertion = "0";
    string colorHTML;
    Color color = Color.white;
    bool plus;
    string path = "";
    int saveFileInt = 0;
    int saveFrequency = 60;
    string saveFrequencystring = "60";
    int saveFileCount = 10;
    string saveFileCountString = "10";
    private void OnFocus()
    {
        if (coroutine == null)
        {
            coroutine = EditorCoroutineUtility.StartCoroutine(Autosave(), this);
        }
    }
    void OnGUI()
    {
        path = GUI.TextField(new Rect(8, 830, 450, 20), path);
        GUI.Label(new Rect(8, 855, 600, 60), string.Format("^ Insert your path to whatever textfile you wana edit ^\n" +
            $"example => {Application.dataPath}/textFile.txt\n" +
            "OBS! you need \"/\" and not \"\\\", it's really important"));

        txt = GUI.TextArea(new Rect(8, 38, 350, 400), txt);

        if (!int.TryParse(saveFileCountString, out saveFileCount)) saveFileCount = 10;
        saveFileCountString = GUI.TextField(new Rect(476, 810, 30, 20), saveFileCountString, 2);
        GUI.Label(new Rect(510, 805, 1000, 30), string.Format("/-- Insert how many savefiles you want \n\\-- before it starts overwriting old ones"));
        
        if (!int.TryParse(saveFrequencystring, out saveFrequency)) saveFrequency = 10;
        saveFrequencystring = GUI.TextField(new Rect(476, 840, 30, 20), saveFrequencystring, 2);
        GUI.Label(new Rect(510, 840, 1000, 20), string.Format("<= Insert how often you want the text to save(seconds)"));

        GUI.TextArea(new Rect(366, 38, 350, 400), WithoutCode(txt));

        TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);

        color = EditorGUILayout.ColorField("Color picker", color);
        colorHTML = ColorUtility.ToHtmlStringRGB(color);

        plus = GUI.Toggle(new Rect(366, 340 + 250, 100, 40), plus, "Alternative");
        GUI.Label(new Rect(476, 340 + 250, 300, 40), string.Format("Click the checkbox to toggel functions"));

        GUI.Label(new Rect(476, 640, 300, 150), string.Format("There is a known issue that don't know if i can fix\n" +
            "and that issue is that the system don't change\n" +
            "exactly what you have choosen, but instead it\n" +
            "changes everything that fits the selected text\n" +
            "so consider doing some maualy, you have all of the\n" +
            "functions and how they are supose to look above\n"));

        intInsertion = GUI.TextField(new Rect(366, 520, 20, 20), intInsertion, 1);
        GUI.Label(new Rect(396, 260 + 250, 400, 40), string.Format("Only Insert one number in here \n(is used when you need a number)"));

        explenationTxt = GUI.TextArea(new Rect(8, 675, 450, 100), explenationTxt);
        GUI.Label(new Rect(8, 775, 450, 40), string.Format("^ Insert your explenation here ^"));

        GUI.Label(new Rect(150, 430, 450, 40), string.Format("^ Text editor ^"));
        GUI.Label(new Rect(440, 430, 450, 40), string.Format("^ Raw text (the text you will get out) ^"));


        GUI.Label(new Rect(158, 220 + 250, 500, 40), string.Format("Looks like => {0}",  plus ? $"{{#{colorHTML} {editor.SelectedText}}}" : $"[{explenationTxt}\"{editor.SelectedText}]"));
        GUI.Label(new Rect(158, 260 + 250, 500, 40), string.Format("Looks like => {0}",  plus ? $"+{intInsertion}{editor.SelectedText}" : $"-{intInsertion}{editor.SelectedText}"));
        GUI.Label(new Rect(158, 300 + 250, 500, 40), string.Format("Looks like => {0}",  plus ? $"<s{editor.SelectedText}>" : $"<w{editor.SelectedText}>"));
        GUI.Label(new Rect(158, 340 + 250, 500, 40), string.Format("Looks like => {0}",  plus ? $"#{editor.SelectedText}" : $"{editor.SelectedText}&"));
        GUI.Label(new Rect(158, 380 + 250, 500, 40), string.Format("Looks like => {0}",  plus ? $"*{intInsertion}{editor.SelectedText}" : $"/{intInsertion}{editor.SelectedText}"));


        if (GUI.Button(new Rect(8, 220 + 250, 150, 40), plus? "Add colour"          : "Add explenation"))          txt = txt.Replace(editor.SelectedText,  plus? $"{{#{colorHTML} {editor.SelectedText}}}" : $"[{explenationTxt}\"{editor.SelectedText}]");
        if (GUI.Button(new Rect(8, 260 + 250, 150, 40), plus? "Bigger text"         : "Smaller Text"))             txt = txt.Replace(editor.SelectedText,  plus? $"+{intInsertion}{editor.SelectedText}" : $"-{intInsertion}{editor.SelectedText}");
        if (GUI.Button(new Rect(8, 300 + 250, 150, 40), plus? "Add Shaking"         : "Add wiggeling"))            txt = txt.Replace(editor.SelectedText,  plus? $"<s{editor.SelectedText}>" : $"<w{editor.SelectedText}>");
        if (GUI.Button(new Rect(8, 340 + 250, 150, 40), plus? "Add the word whole"  : "Add \"wait for input\""))   txt = txt.Replace(editor.SelectedText,  plus? $"#{editor.SelectedText}" : $"{editor.SelectedText}&");
        if (GUI.Button(new Rect(8, 380 + 250, 150, 40), plus? "Change Text Speed"   : "Add a pause"))              txt = txt.Replace(editor.SelectedText,  plus? $"*{intInsertion}{editor.SelectedText}" : $"/{intInsertion}{editor.SelectedText}");

        if (GUI.Button(new Rect(198, 780, 130, 40), "Load TextFile"))
        {
            StreamReader stream;
            try
            {
                stream = new StreamReader(path);
                txt = stream.ReadToEnd();
            }
            catch (System.Exception e)
            {
                Debug.Log("if you did not enter a path, dont worry about the warning");
                Debug.Log(e);
                Debug.Log("your path was not valid, look through it and make sure that its structured properly. Using Main.Speech.txt");
                stream = new StreamReader($"{Application.dataPath}/Main.Speech.txt");
                
                txt = stream.ReadToEnd();
            }
            stream.Close();
        }
        if (GUI.Button(new Rect(328, 780, 130, 40), "Save TextFile"))
        {
            try
            {
                using (StreamWriter sr = new StreamWriter(path))
                {
                    sr.WriteLine(txt);
                }
            }
            catch
            {
                Debug.Log("if you did not enter a path, dont worry about the warning");
                Debug.Log("your path was not valid, look through it and make sure that its structured properly. Using Main.Speech.txt");
                using (StreamWriter sr = new StreamWriter($"{Application.dataPath}/Main.Speech.txt"))
                {
                    sr.WriteLine(txt);
                }
            }
        }    
        
    }
    IEnumerator Autosave()
    {
        yield return new WaitForSecondsRealtime(saveFrequency);
        if (txt == "" || txt == previousAutoSaveText) Debug.Log("there was nothing to save");
        else if (File.Exists($"{Application.dataPath}/AutoSaves/AutoSaveNO{saveFileInt}.txt"))
            using (StreamWriter sr = new StreamWriter($"{Application.dataPath}/AutoSaves/AutoSaveNO{saveFileInt}.txt"))
            {
                sr.WriteLine(txt);
                Debug.Log($"Makin the {saveFileInt} save");
                saveFileInt++;
            }
        else
        {
            File.Create($"{Application.dataPath}/AutoSaves/AutoSaveNO{saveFileInt}.txt").Close();
            using (StreamWriter sr = new StreamWriter($"{Application.dataPath}/AutoSaves/AutoSaveNO{saveFileInt}.txt"))
            {
                sr.WriteLine(txt);
                Debug.Log($"Makin the {saveFileInt} save");
                saveFileInt++;
            }
        }
        previousAutoSaveText = txt;
        if (saveFileInt >= saveFileCount) saveFileInt = 0;
        if (focusedWindow == this) EditorCoroutineUtility.StartCoroutine(Autosave(), this);
        else coroutine = null;
    }
    string WithoutCode(string text)
    {
        char functionChar = 'a';
        string returnText = "";

        bool activlyCheckingChar = true;
        bool skipNextChar = false;
        
        string charsWithFunctions = "{}<>[]&+-*\\#/";
        string singleSkipChars = "\\+-/*<";
        string noSkipChars = "}>&]#";
        foreach (char character in text)
        {
            if (skipNextChar)
            {
                skipNextChar = false;
                continue;
            } 
            if (charsWithFunctions.Contains(character.ToString()) && activlyCheckingChar)
            {
                if (singleSkipChars.Contains(character.ToString()))
                {
                    skipNextChar = true;
                    continue;
                }
                else if (noSkipChars.Contains(character.ToString()))
                {
                    continue;
                }
                else
                {
                    functionChar = character;
                    activlyCheckingChar = false;
                }
            }
            if (!activlyCheckingChar)
                switch (functionChar)
                {
                    case '{':
                        if (character == ' ') activlyCheckingChar = true;
                        continue;
                    case '[':
                        if (character == '"') activlyCheckingChar = true;
                        continue;
                }
            returnText += character;
        }
        return returnText;
    }

    /*private void OnGUI()
    {
        GUILayout.Space(100);
        if (GUILayout.Button("Set the textfile"))
        {
            string path = $"{Application.dataPath}/Main.Speech.txt";
            if (!File.Exists(path)) File.Create(path).Close();
            File.OpenWrite(text).Close();
        }
        TextEditor textEditor = new TextEditor();
        text = GUILayout.TextArea(text, GUILayout.MaxHeight(500));
        if (GUILayout.Button("Add color to the text"))
        {
            string newText = textEditor.SelectedText;
            string color = ColorUtility.ToHtmlStringRGB(Color.white);
            textEditor.SelectedText.Replace("YEET", $"<{color} {newText}>");
            //textEditor.ReplaceSelection($"<{color} {newText}>");
            Debug.Log(newText);
            Debug.Log(textEditor.SelectedText);
        }
    }*/
/*private static bool checkIfItsValid(ref string path, string RelativePath = "",string Extension = "")
    {
        if (path.IndexOfAny(Path.GetInvalidPathChars()) == -1)
        {
            try
            {
                // If path is relative take %IGXLROOT% as the base directory
                if (!Path.IsPathRooted(path))
                {
                    if (string.IsNullOrEmpty(RelativePath))
                    {
                        // Exceptions handled by Path.GetFullPath
                        // ArgumentException path is a zero-length string, contains only white space, or contains one or more of the invalid characters defined in GetInvalidPathChars. -or- The system could not retrieve the absolute path.
                        // 
                        // SecurityException The caller does not have the required permissions.
                        // 
                        // ArgumentNullException path is null.
                        // 
                        // NotSupportedException path contains a colon (":") that is not part of a volume identifier (for example, "c:\"). 
                        // PathTooLongException The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.

                        // RelativePath is not passed so we would take the project path 
                        path = Path.GetFullPath(RelativePath);

                    }
                    else
                    {
                        // Make sure the path is relative to the RelativePath and not our project directory
                        path = Path.Combine(RelativePath, path);
                    }
                }

                // Exceptions from FileInfo Constructor:
                //   System.ArgumentNullException:
                //     fileName is null.
                //
                //   System.Security.SecurityException:
                //     The caller does not have the required permission.
                //
                //   System.ArgumentException:
                //     The file name is empty, contains only white spaces, or contains invalid characters.
                //
                //   System.IO.PathTooLongException:
                //     The specified path, file name, or both exceed the system-defined maximum
                //     length. For example, on Windows-based platforms, paths must be less than
                //     248 characters, and file names must be less than 260 characters.
                //
                //   System.NotSupportedException:
                //     fileName contains a colon (:) in the middle of the string.
                FileInfo fileInfo = new FileInfo(path);

                // Exceptions using FileInfo.Length:
                //   System.IO.IOException:
                //     System.IO.FileSystemInfo.Refresh() cannot update the state of the file or
                //     directory.
                //
                //   System.IO.FileNotFoundException:
                //     The file does not exist.-or- The Length property is called for a directory.
                bool throwEx = fileInfo.Length == -1;

                // Exceptions using FileInfo.IsReadOnly:
                //   System.UnauthorizedAccessException:
                //     Access to fileName is denied.
                //     The file described by the current System.IO.FileInfo object is read-only.-or-
                //     This operation is not supported on the current platform.-or- The caller does
                //     not have the required permission.
                throwEx = fileInfo.IsReadOnly;

                if (!string.IsNullOrEmpty(Extension))
                {
                    // Validate the Extension of the file.
                    if (Path.GetExtension(path).Equals(Extension, StringComparison.InvariantCultureIgnoreCase))
                    {
                        // Trim the Library Path
                        path = path.Trim();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;

                }
            }
            catch (ArgumentNullException)
            {
                //   System.ArgumentNullException:
                //     fileName is null.
            }
            catch (System.Security.SecurityException)
            {
                //   System.Security.SecurityException:
                //     The caller does not have the required permission.
            }
            catch (ArgumentException)
            {
                //   System.ArgumentException:
                //     The file name is empty, contains only white spaces, or contains invalid characters.
            }
            catch (UnauthorizedAccessException)
            {
                //   System.UnauthorizedAccessException:
                //     Access to fileName is denied.
            }
            catch (PathTooLongException)
            {
                //   System.IO.PathTooLongException:
                //     The specified path, file name, or both exceed the system-defined maximum
                //     length. For example, on Windows-based platforms, paths must be less than
                //     248 characters, and file names must be less than 260 characters.
            }
            catch (NotSupportedException)
            {
                //   System.NotSupportedException:
                //     fileName contains a colon (:) in the middle of the string.
            }
            catch (FileNotFoundException)
            {
                // System.FileNotFoundException
                //  The exception that is thrown when an attempt to access a file that does not
                //  exist on disk fails.
            }
            catch (IOException)
            {
                //   System.IO.IOException:
                //     An I/O error occurred while opening the file.
            }
            catch (Exception)
            {
                // Unknown Exception. Might be due to wrong case or nulll checks.
            }
        }
        else
        {
            // Path contains invalid characters
        }
        return false;
    }*/
}

#endif