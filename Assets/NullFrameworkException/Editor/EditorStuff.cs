using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Junk))]
public class EditorStuff : Editor
{
    string playerName = "Player 1";
    string playerLevel = "1";
    string playerElo = "5";
    string playerScore = "100";
    
    float thumbnailWidth = 70;
    float thumbnailHeight = 70;
    float labelWidth = 150f;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Junk junk = (Junk) target;
        GUILayout.Space(20f); //2
        GUILayout.Label("Custom Editor Elements", EditorStyles.boldLabel); //3

        GUILayout.Space(10f);
        GUILayout.Label("Player Preferences");

        GUILayout.BeginHorizontal(); //4
        GUILayout.Label("Player Name", GUILayout.Width(labelWidth)); //5
        playerName = GUILayout.TextField(playerName); //6
        GUILayout.EndHorizontal(); //7

        GUILayout.BeginHorizontal();
        GUILayout.Label("Player Level", GUILayout.Width(labelWidth));
        playerLevel = GUILayout.TextField(playerLevel);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Player Elo", GUILayout.Width(labelWidth));
        playerElo = GUILayout.TextField(playerElo);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Player Score", GUILayout.Width(labelWidth)); 
        playerScore = GUILayout.TextField(playerScore);
        GUILayout.EndHorizontal();
        
        EditorGUILayout.HelpBox("This is a help box", MessageType.Info);
    }
    
}
