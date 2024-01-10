using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ToonBabies
{
   [CustomEditor(typeof(TBabyPrefabMaker))]

    public class EditorTBabyPrefabMaker : Editor
    {
        public override void OnInspectorGUI()
        {

            TBabyPrefabMaker myPrefabMaker = (TBabyPrefabMaker)target;

            if (!myPrefabMaker.allOptions)
            {
                if (GUILayout.Button("LET'S GET DRESS", GUILayout.Width(250), GUILayout.Height(75)))
                {
                    myPrefabMaker.Menu();
                    myPrefabMaker.Getready();
                }
            }
            else
            {
                if (GUILayout.Button("RANDOMIZE", GUILayout.Width(250), GUILayout.Height(75)))
                {
                    myPrefabMaker.Randomize();
                }

                EditorGUILayout.Space();                

                GUILayout.BeginHorizontal("box");
                if (GUILayout.Button("<", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Prevhair(); }
                EditorGUILayout.LabelField("  HAIR", GUILayout.Width(65), GUILayout.Height(20));
                if (GUILayout.Button(">", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nexthair(); }
                if (GUILayout.Button("<", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nexthaircolor(1); }
                EditorGUILayout.LabelField("  material", GUILayout.Width(65), GUILayout.Height(20));
                if (GUILayout.Button(">", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nexthaircolor(0); }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal("box");
                if (GUILayout.Button("<", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nextskincolor(1); }
                EditorGUILayout.LabelField("  SKIN", GUILayout.Width(65), GUILayout.Height(20));
                if (GUILayout.Button(">", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nextskincolor(0); }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal("box");
                if (GUILayout.Button("<", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nexteyescolor(1); }
                EditorGUILayout.LabelField("  EYES", GUILayout.Width(65), GUILayout.Height(20));
                if (GUILayout.Button(">", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nexteyescolor(0); }
                GUILayout.EndHorizontal();

                

                GUILayout.BeginHorizontal("box");
                if (GUILayout.Button("DIAPERS", GUILayout.Width(115), GUILayout.Height(20))) { myPrefabMaker.Diaperson(); }
                if (myPrefabMaker.diaperactive)
                {
                    if (GUILayout.Button("<", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nextdiapers(1); }
                    EditorGUILayout.LabelField("  material", GUILayout.Width(65), GUILayout.Height(20));
                    if (GUILayout.Button(">", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nextdiapers(0); }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal("box");
                if (GUILayout.Button("PIYAMAS", GUILayout.Width(115), GUILayout.Height(20))) { myPrefabMaker.Nextbody(); }
                if (myPrefabMaker.pyjamasactive)
                {
                    if (GUILayout.Button("<", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nextbodycolor(1); }
                    EditorGUILayout.LabelField("  material", GUILayout.Width(65), GUILayout.Height(20));
                    if (GUILayout.Button(">", GUILayout.Width(20), GUILayout.Height(20))) { myPrefabMaker.Nextbodycolor(0); }
                }
                GUILayout.EndHorizontal();

                

                //if (GUILayout.Button("NUDE", GUILayout.Width(115), GUILayout.Height(20))) { myPrefabMaker.Nude(); }

                GUILayout.BeginHorizontal("box");
                if (GUILayout.Button("CREATE COPY", GUILayout.Width(100), GUILayout.Height(50)))
                {
                    myPrefabMaker.CreateCopy();
                }
                if (GUILayout.Button("DONE", GUILayout.Width(100), GUILayout.Height(50)))
                {
                    myPrefabMaker.FIX();
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button("RESET", GUILayout.Width(100), GUILayout.Height(50)))
                {
                    myPrefabMaker.Resetmodel();
                }
            }
        }
    }
}
