#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

public class StateMachineEditor : EditorWindow
{
    const string PATHTOCORE = "Base/#SCRIPTNAME#/#SCRIPTNAME#Core.txt";
    const string PATHTOSTATES = "Base/#SCRIPTNAME#/#SCRIPTNAME#States.txt";
    const string PATHTOIDLESTATE = "Base/#SCRIPTNAME#/States/#SCRIPTNAME#IdleState.txt";
    const string PATHTOJUMPSTATE = "Base/#SCRIPTNAME#/States/#SCRIPTNAME#JumpState.txt";
    const string PATHTOTEMPLATESTATE = "Base/#SCRIPTNAME#/States/#SCRIPTNAME##STATENAME#State.txt";

    const int INDENTWIDTH = 8;


    string _path = "";


    string _coreName = "Enemy";

    
    [System.Serializable] class StateMachineProperties
    {
        public bool Toggle = false;
        public string NewStateName = "";
    }
    List<StateMachineProperties> _newStateName = new List<StateMachineProperties>();

    Vector2 _scrollPos;

    GUIStyle foldoutStyle;
        
    int _toolbarInt = 0;
    string[] _toolbarStrings = {"Add New", "Update Existing", "Initialization"};


    [MenuItem("Window/State Machine Editor")]
    static void Init()
    {
        StateMachineEditor window = (StateMachineEditor)EditorWindow.GetWindow(typeof(StateMachineEditor));
        window.Show();
    }

    void OnGUI()
    {
        foldoutStyle = EditorStyles.foldout;
        foldoutStyle.fontStyle = FontStyle.Bold;

        _toolbarInt = GUILayout.Toolbar(_toolbarInt, _toolbarStrings);

        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

        if(_path == "")
        {
            _path = GetCurrentScriptFolderPath();
        }

        // Check if directory exist
        if (!Directory.Exists(_path))
        {
            // EditorGUILayout.HelpBox("Please select a valid directory", MessageType.Error);
            _toolbarInt = 2;
            // return;
        }

        switch(_toolbarInt)
        {
            case 0:
                AddNewStateMachine();
                break;
            case 1:
                UpdateExistingStateMachine();
                break;
            case 2:
                Initialization();
                break;
        }
        EditorGUILayout.EndScrollView();
    }

    private void Initialization()
    {
        GUIStyle bold = EditorStyles.boldLabel;
        bold.alignment = TextAnchor.MiddleCenter;
        EditorGUILayout.LabelField("Initialization", bold);

        GUIStyle style = EditorStyles.textArea;
        style.wordWrap = true;
        // _path = EditorGUILayout.TextField("Path: ", _path, style);
        EditorGUILayout.LabelField("Path: ");
        
        _path = EditorGUILayout.TextArea(_path, style);

        Rect rect = EditorGUILayout.GetControlRect();
        if(GUI.Button(rect, "Select Folder"))
        {
            string defaultPath = GetCurrentScriptFolderPath();
            _path = EditorUtility.OpenFolderPanel("Select Folder", "", defaultPath);
        }

    }

    private void AddNewStateMachine()
    {
        GUIStyle bold = EditorStyles.boldLabel;
        bold.alignment = TextAnchor.MiddleCenter;
        EditorGUILayout.LabelField("Add New StateMachine", bold);

        GUILayout.BeginHorizontal();
        
        
        float originalWidth = EditorGUIUtility.labelWidth;
        string label = "Core Name: ";
        Vector2 textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
        EditorGUIUtility.labelWidth = textDimensions.x + INDENTWIDTH*2;
        _coreName = EditorGUILayout.TextField(label, _coreName, GUILayout.ExpandWidth(false));
        EditorGUIUtility.labelWidth = originalWidth;
        
        
        // Remove all spaces
        _coreName = _coreName.Replace(" ", "");

        // GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent("", "Please don't include the 'Core' suffix in this field"));
        // GUILayout.Label(GUI.tooltip);
        EditorGUILayout.LabelField("Core", GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent(label)).x));
        GUILayout.EndHorizontal();

        if(_coreName == "")
            GUI.enabled = false;
        if(GUILayout.Button("Generate "+_coreName+"Core"))
        {
            GenerateStateMachine(_coreName);
        }
        GUI.enabled = true;
    }

    private void UpdateExistingStateMachine()
    {
        GUIStyle bold = EditorStyles.boldLabel;
        bold.alignment = TextAnchor.MiddleCenter;
        EditorGUILayout.LabelField("Update StateMachine", bold);

        // path to current folder including the name of the core, ...StateMachine\Player
        List<string> paths = Directory.GetDirectories(_path).ToList();
        // name of current core
        List<string> coreNames = Directory.GetDirectories(_path).Select(d => new DirectoryInfo(d).Name).ToList();

        // Set the size of the list to the number of paths
        if(_newStateName.Count() != paths.Count())
        {
            _newStateName.Clear();
            for(int i = 0; i < paths.Count(); i++)
            {
                _newStateName.Add(new StateMachineProperties());
            }
        }

        for(int i = 0; i < paths.Count(); i++)
        {
            if(coreNames[i] == "Base")continue;
            EditorGUILayout.Space(5);

            _newStateName[i].Toggle = EditorGUILayout.Foldout(_newStateName[i].Toggle, coreNames[i]+"Core", foldoutStyle);
            if(!_newStateName[i].Toggle)continue;

            EditorGUI.indentLevel++;

            // Available States
            List<string> states = GetAvailableStates(paths[i], coreNames[i]);
            // string statesText = "";
            
            // for(int j = 0; j < states.Count(); j++)
            // {
            //     if(j == states.Count()-1)
            //         statesText += states[j];
            //     else
            //         statesText += states[j] + ", ";
            // }
            // EditorGUILayout.LabelField("States("+states.Count()+"): "+ statesText);
            
            EditorGUI.LabelField(EditorGUILayout.GetControlRect() ,"States("+states.Count()+"): ");

            GUILayout.BeginHorizontal();
            GUILayout.Space(INDENTWIDTH*3);
            
            foreach(string state in states)
            {   
                if(EditorGUILayout.LinkButton(state))
                {
                    Application.OpenURL(paths[i] + "/States/" + (coreNames[i]+state+"State.cs"));
                }
            }
            GUILayout.EndHorizontal();
            

            // New State Name
            EditorGUILayout.LabelField("New State Name:");

            // Field
            GUILayout.BeginHorizontal();
            GUILayout.Space(INDENTWIDTH);
            // _newStateName[i].NewStateName = EditorGUILayout.TextField(coreNames[i], _newStateName[i].NewStateName);
            float originalWidth = EditorGUIUtility.labelWidth;
            Vector2 textDimensions = GUI.skin.label.CalcSize(new GUIContent(coreNames[i]));
            EditorGUIUtility.labelWidth = textDimensions.x + INDENTWIDTH*2;
            _newStateName[i].NewStateName = EditorGUILayout.TextField(coreNames[i], _newStateName[i].NewStateName, GUILayout.ExpandWidth(false));
            EditorGUIUtility.labelWidth = originalWidth;


            // Remove all spaces
            _newStateName[i].NewStateName = _newStateName[i].NewStateName.Replace(" ", "");

            // Tight label
            GUIContent stateLabel = new GUIContent("State");
            GUILayoutOption option = GUILayout.Width(GUI.skin.label.CalcSize(stateLabel).x + INDENTWIDTH*2);
            // move label to the left a bit
            GUILayout.Space(-INDENTWIDTH*2);
            EditorGUILayout.LabelField(stateLabel, option);
            GUILayout.EndHorizontal();

            Rect rect = EditorGUILayout.GetControlRect();
            rect.x += INDENTWIDTH*2.5f;
            rect.width -= INDENTWIDTH*2.5f;

            if(_newStateName[i].NewStateName == "")
                GUI.enabled = false;
            // Generate State
            if(GUI.Button(rect, "Add "+coreNames[i]+_newStateName[i].NewStateName+"State"))
            {
                GenerateState(paths[i], coreNames[i], _newStateName[i].NewStateName);
            }
            GUI.enabled = true;

            rect = EditorGUILayout.GetControlRect();
            rect.x += INDENTWIDTH*2.5f;
            rect.width -= INDENTWIDTH*2.5f;

            // Update States
            if(GUI.Button(rect, "Refresh "+coreNames[i]+"States"))
            {
                UpdateStateMachine(paths[i], coreNames[i]);
            }

            EditorGUI.indentLevel--;
        }
            
    }

    void GenerateStateMachine(string _coreName)
    {
        string coreTemplate = File.ReadAllText(_path+"/"+PATHTOCORE).Replace("#SCRIPTNAME#", _coreName);
        string statesTemplate = File.ReadAllText(_path+"/"+PATHTOSTATES).Replace("#SCRIPTNAME#", _coreName);
        string idleStateTemplate = File.ReadAllText(_path+"/"+PATHTOIDLESTATE).Replace("#SCRIPTNAME#", _coreName);
        string jumpStateTemplate = File.ReadAllText(_path+"/"+PATHTOJUMPSTATE).Replace("#SCRIPTNAME#", _coreName);

        DirectoryInfo folder = Directory.CreateDirectory(_path+"/"+_coreName);
        File.WriteAllText(_path+"/"+_coreName+"/"+_coreName+"Core.cs", coreTemplate);
        File.WriteAllText(_path+"/"+_coreName+"/"+_coreName+"States.cs", statesTemplate);

        DirectoryInfo statesFolder = Directory.CreateDirectory(_path+"/"+_coreName+"/States");
        File.WriteAllText(_path+"/"+_coreName+"/States/"+_coreName+"IdleState.cs", idleStateTemplate);
        File.WriteAllText(_path+"/"+_coreName+"/States/"+_coreName+"JumpState.cs", jumpStateTemplate);
        

        UpdateStateMachine(_path+"/"+_coreName, _coreName);
        AssetDatabase.Refresh();
        Debug.Log("Successfully generating " + _coreName + "Core");
    }

    void UpdateStateMachine(string path, string _coreName)
    {
        Debug.Log("Updating " + _coreName + "Core" + " in " + path);

        List<string> states = GetAvailableStates(path, _coreName);

        string statesTemplate = File.ReadAllText(_path+"/"+PATHTOSTATES).Replace("#SCRIPTNAME#", _coreName);
        
        string statesEnumText = "";
        string statesConstructorText = "";
        string statesSwitchMethods = "";

        for(int i = 0; i < states.Count(); i++)
        {
            statesEnumText += states[i] + ", ";
            statesConstructorText += $"        _states[State.{states[i]}] = new {_coreName}{states[i]}State(Core, this);\n";
            statesSwitchMethods += $"    public BaseState<{_coreName}Core, {_coreName}States> {states[i]}() => _states[State.{states[i]}];\n";
        }

        statesTemplate = statesTemplate.Replace("#STATESENUM#", statesEnumText);
        statesTemplate = statesTemplate.Replace("#STATESCONSTRUCTOR#", statesConstructorText);
        statesTemplate = statesTemplate.Replace("#STATESSWITCHMETHODS#", statesSwitchMethods);

        File.WriteAllText(_path+"/"+_coreName+"/"+_coreName+"States.cs", statesTemplate);
        AssetDatabase.Refresh();
    }

    void GenerateState(string path, string _coreName, string _newStateName)
    {
        Debug.Log("Generating " + _coreName + _newStateName + "State" + " in " + path);

        string stateTemplate = File.ReadAllText(_path+"/"+PATHTOTEMPLATESTATE).Replace("#SCRIPTNAME#", _coreName);
        stateTemplate = stateTemplate.Replace("#STATENAME#", _newStateName);

        DirectoryInfo folder = Directory.CreateDirectory(_path+"/"+_coreName);
        File.WriteAllText(_path+"/"+_coreName+"/States/"+_coreName+_newStateName+"State.cs", stateTemplate);

        UpdateStateMachine(_path+"/"+_coreName, _coreName);
        AssetDatabase.Refresh();
        Debug.Log("Successfully generating " + _coreName + _newStateName + "State");
    }

    List<string> GetAvailableStates(string path, string coreName)
    {
        List<string> states = new List<string>();
        string[] files = Directory.GetFiles(path+"/States");
        for(int i = 0; i < files.Length; i++)
        {
            string fileExtension = Path.GetExtension(files[i]);
            if(fileExtension == ".cs")
            {
                string fileNameNoExtension = Path.GetFileNameWithoutExtension(files[i]);
                fileNameNoExtension = fileNameNoExtension.Replace(".cs", "");
                states.Add(fileNameNoExtension.Replace(coreName, "").Replace("State", ""));
            }
        }
        return states;
    }

    string GetCurrentScriptFolderPath()
    {
        MonoScript script = MonoScript.FromScriptableObject(this);
        string scriptPath = AssetDatabase.GetAssetPath(script);
        scriptPath = scriptPath.Replace("Assets", Application.dataPath);

        return Path.GetDirectoryName(scriptPath);
    }
}
#endif