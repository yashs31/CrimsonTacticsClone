using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Unity.VisualScripting;

[CustomPropertyDrawer(typeof(ObstacleGridLayout))]
public class CustPropertyDrawer:PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PrefixLabel(position, label);
        Rect newPosition = position;
        Rect labelPos = position;
        newPosition.y += 40f;
        SerializedProperty data=property.FindPropertyRelative("X");
        //data.row[0][]

        labelPos.x += 30;
        for (int i=0;i<10;i++)
        {
            EditorGUI.LabelField(new Rect(labelPos.x, 20, 50, 20), "x" + i);
            labelPos.width = 70;
            labelPos.x += labelPos.width;
        }
        for(int i=0;i<10;i++)
        {
            SerializedProperty row = data.GetArrayElementAtIndex(i).FindPropertyRelative("Y");
            newPosition.height = 20;
            if(row.arraySize!=10)
                row.arraySize = 10;

            newPosition.width = 70;
            newPosition.x += 30; //for adding coordinate labels
            
            for (int j = 0; j < 10; j++)
            {
                EditorGUI.PropertyField(newPosition, row.GetArrayElementAtIndex(j),GUIContent.none);
                newPosition.x += newPosition.width;
            }
            EditorGUI.LabelField(new Rect(0, newPosition.y, 50,20), "y" + i);
            newPosition.x = position.x;
            newPosition.y += 20f;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 20*12;
    }

    public bool GetBool(SerializedProperty property, GUIContent label,int x,int y)
    {
        SerializedProperty data = property.FindPropertyRelative("rows");
        //data.row[0][]
        SerializedProperty row = data.GetArrayElementAtIndex(x).FindPropertyRelative("row");
        return row.GetArrayElementAtIndex(y).boolValue;
    }
}
