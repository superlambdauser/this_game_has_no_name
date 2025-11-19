using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Card), true)]
public class CardsCustomEditor : Editor
{
    private SerializedProperty cardTypes;
    private SerializedProperty attackBehaviour;
    private SerializedProperty movementBehaviour;
    private SerializedProperty specialAbilities;

    private void OnEnable()
    {
        cardTypes = serializedObject.FindProperty("cardTypes");
        attackBehaviour = serializedObject.FindProperty("attackBehaviour");
        movementBehaviour = serializedObject.FindProperty("movementBehaviour");
        specialAbilities = serializedObject.FindProperty("specialAbilities");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw all default properties except the custom ones
        DrawPropertiesExcluding(serializedObject, "m_Script", "attackBehaviour", "movementBehaviour", "specialAbilities", "cardTypes");

        Card inspectedCard = (Card)target;

        // Determine allowed types
        string[] allowedTypes = inspectedCard switch
        {
            AttackCard => new string[] { "Attack", "Special" },
            MovementCard => new string[] { "Movement", "Special" },
            SpecialCard => new string[] { "Attack", "Movement", "Special" },
            _ => Array.Empty<string>()
        };

        string mandatoryType = inspectedCard switch
        {
            AttackCard => "Attack",
            MovementCard => "Movement",
            SpecialCard => null,
            _ => null
        };

        // Clean up invalid card types
        for (int i = inspectedCard.CardTypes.Count - 1; i >= 0; i--)
        {
            string typeString = inspectedCard.CardTypes[i].ToString();
            if (typeString != mandatoryType && !allowedTypes.Contains(typeString))
            {
                inspectedCard.CardTypes.RemoveAt(i);
            }
        }

        // Ensure mandatory type is present
        if (mandatoryType != null && !inspectedCard.CardTypes.Contains(Enum.Parse<Card.CardType>(mandatoryType)))
        {
            inspectedCard.CardTypes.Insert(0, Enum.Parse<Card.CardType>(mandatoryType));
        }

        // Draw card types
        EditorGUILayout.LabelField("Card Types:", EditorStyles.boldLabel);
        if (mandatoryType != null)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField($"{mandatoryType} (Mandatory)");
            EditorGUI.EndDisabledGroup();
        }

        foreach (var type in inspectedCard.CardTypes)
        {
            string typeStr = type.ToString();
            if (typeStr == mandatoryType) continue;
            EditorGUILayout.LabelField(typeStr);
        }

        // Draw behaviours based on allowed types safely
        if (allowedTypes.Contains("Attack") && attackBehaviour != null)
        {
            if (attackBehaviour.managedReferenceValue == null)
                EditorGUILayout.HelpBox("Attack Behaviour is required when Attack type is assigned.", MessageType.Error);

            EditorGUILayout.PropertyField(attackBehaviour);
        }

        if (allowedTypes.Contains("Movement") && movementBehaviour != null)
        {
            if (movementBehaviour.managedReferenceValue == null)
                EditorGUILayout.HelpBox("Movement Behaviour is required when Movement type is assigned.", MessageType.Error);

            EditorGUILayout.PropertyField(movementBehaviour);
        }

        if (allowedTypes.Contains("Special") && specialAbilities != null)
        {
            if (specialAbilities.arraySize == 0)
                EditorGUILayout.HelpBox("Special Ability might be required when Special type is assigned.", MessageType.Warning);

            EditorGUILayout.PropertyField(specialAbilities, new GUIContent("Special Abilities"), true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
