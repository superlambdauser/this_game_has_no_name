using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Card), true)] // Custom Editor for Card objects, true = Card + Card children (defautl = false)
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

        // Draw all default properties except the custom ones :
        DrawPropertiesExcluding(serializedObject, "m_Script", "attackBehaviour", "movementBehaviour", "specialAbilities", "cardTypes");

        Card inspectedCard = (Card)target; // Current instance of Card type inspected

        // Determine allowed types :
        string[] allowedTypes = inspectedCard switch
        {
            AttackCard => new string[] { "Attack", "Special" }, // No movement
            MovementCard => new string[] { "Movement", "Special" }, // No attack
            SpecialCard => new string[] { "Attack", "Movement", "Special" }, // All allowed
            _ => Array.Empty<string>() // Empty array otherwise
        };
        
        // Determine mandatory (non-removable) type :
        string mandatoryType = inspectedCard switch
        {
            AttackCard => "Attack",
            MovementCard => "Movement",
            SpecialCard => null, // All types optional
            _ => null
        };


        // Remove invalid card types :
        for (int i = inspectedCard.CardTypes.Count - 1; i >= 0; i--)
        {
            string typeToString = inspectedCard.CardTypes[i].ToString();
            
            if (typeToString != mandatoryType && !allowedTypes.Contains(typeToString))
            {
                inspectedCard.CardTypes.RemoveAt(i);
            }
        }

        // Ensure mandatory type is present :
        if (mandatoryType != null && !inspectedCard.CardTypes.Contains(Enum.Parse<Card.CardType>(mandatoryType)))
        {
            inspectedCard.CardTypes.Insert(0, Enum.Parse<Card.CardType>(mandatoryType));
        }

        // Draw card types :
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Card Types:", EditorStyles.boldLabel);

        if (mandatoryType != null) // Mandatory type (only for attacks and movements)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField($"{mandatoryType} (Mandatory)");
            EditorGUI.EndDisabledGroup();
        }

        // Display "Special" type dynamically :
        bool hasSpecialAbility = specialAbilities != null && specialAbilities.arraySize > 0;

        if (hasSpecialAbility) EditorGUILayout.LabelField("Special");

        // Draw non-mandatory types :
        foreach (Card.CardType type in inspectedCard.CardTypes)
        {
            string typeToString = type.ToString();

            if (typeToString == mandatoryType) continue; // Skip mandatory

            if (typeToString == "Special" && hasSpecialAbility) continue; // Already drawn dynamically above 

            EditorGUILayout.LabelField(typeToString);
        }

        // At least one visible, assigned type for special cards :
        if (inspectedCard is SpecialCard)
        {
            bool hasAnyType = 
                hasSpecialAbility || // "Special" type check via specialAbilities list
                inspectedCard.CardTypes.Count > 0; // "Attack" and/or "Movement" type check via CardTypes

            if (!hasAnyType)
            {
                EditorGUILayout.HelpBox("Special cards must have at least one Card Type assigned.", MessageType.Error);
            }
        }

        // Draw traits section :
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Type(s) Traits :", EditorStyles.boldLabel);

        // Draw behaviours based on allowed types safely
        bool hasAttackType = inspectedCard.CardTypes.Contains(Card.CardType.Attack);
        bool hasMovementType = inspectedCard.CardTypes.Contains(Card.CardType.Movement);

        if (hasAttackType)
        {
            if (attackBehaviour.managedReferenceValue == null) EditorGUILayout.HelpBox("Attack Behaviour is required when Attack type is assigned.", MessageType.Error);

            EditorGUILayout.PropertyField(attackBehaviour);
        }

        if (hasMovementType)
        {
            if (movementBehaviour.managedReferenceValue == null) EditorGUILayout.HelpBox("Movement Behaviour is required when Movement type is assigned.", MessageType.Error);

            EditorGUILayout.PropertyField(movementBehaviour);
        }

        if (allowedTypes.Contains("Special") && specialAbilities != null)
        {
            // No warning if empty :
            EditorGUILayout.PropertyField(specialAbilities, new GUIContent("Special Abilities"), true);

            // Prevent & remove duplicates :
            HashSet<Type> alreadyAssignedAbilities = new HashSet<Type>(); // Hashset<Type> = list of Type elements where duplicates are not allowed.

            for (int i = specialAbilities.arraySize - 1; i >= 0; i--) // Always work backwards when deleting elements of a list
            {
                SerializedProperty element = specialAbilities.GetArrayElementAtIndex(i);

                // Null-check + warning :
                if (element.managedReferenceValue == null)
                {
                    EditorGUILayout.HelpBox($"Special Ability #{i} is empty. Select an ability or remove the entry.", MessageType.Error);
                }

                object obj = element.managedReferenceValue; // Comparing objects (in memory), not values

                if (obj == null) continue; // Skip and go to next iteration 

                Type type = obj.GetType();

                // Delete element if already assigned, add it to already assigned types otherwise :
                if (alreadyAssignedAbilities.Contains(type))
                {
                    Debug.Log($"{type.Name} is already assigned on {inspectedCard.CardName}. Special abilities can only be assigned once.");
                    specialAbilities.DeleteArrayElementAtIndex(i);
                }
                else alreadyAssignedAbilities.Add(type);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
