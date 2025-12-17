using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;



// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
// !!!!!DON'T TOUCH THIS CODE ANYMORE AND FOCUS ON THE GAME !!!!!
// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


[CustomEditor(typeof(CardData), true)] // Custom Editor for Card objects, true = Card + Card children (default = false)
public class CardsCustomEditor : Editor
{
    private SerializedProperty typeFlags; // Flags field (Card.types)
    private int previousFlags;
    private SerializedProperty attackSpecs;
    private SerializedProperty movementSpecs;
    private SerializedProperty specialAbilities;


    private void OnEnable()
    {
        if (target == null) return; // Safeguard
        
        typeFlags = serializedObject.FindProperty("typeFlags");
        attackSpecs = serializedObject.FindProperty("attackSpecs");
        movementSpecs = serializedObject.FindProperty("movementSpecs");
        specialAbilities = serializedObject.FindProperty("specialAbilities");

        if (typeFlags != null) previousFlags = typeFlags.intValue;
    }

    public override void OnInspectorGUI() // NB : don't forget to override the method
    {
        // Pull current values :
        serializedObject.Update(); // Always start with the Update() inside OnInspectorGUI()

        // Draw all default properties except the custom ones :
        DrawPropertiesExcluding(serializedObject, "m_Script", "typeFlags", "attackSpecs", "movementSpecs", "specialAbilities");

        CardData inspectedCard = (CardData)target; // Current instance of Card type inspected

        // Determine allowed types based on flags :
        List<string> allowedTypesList = new List<string>();
        if (inspectedCard.TypeFlags.HasFlag(CardData.CardType.Attack)) allowedTypesList.Add("Attack");
        if (inspectedCard.TypeFlags.HasFlag(CardData.CardType.Movement)) allowedTypesList.Add("Movement");
        if (inspectedCard.TypeFlags.HasFlag(CardData.CardType.Special)) allowedTypesList.Add("Special");
        string[] allowedTypes = allowedTypesList.ToArray();

        // Determine mandatory (non-removable) type :
        string mandatoryType = null;
        if (inspectedCard.TypeFlags.HasFlag(CardData.CardType.Attack) &&
            !inspectedCard.TypeFlags.HasFlag(CardData.CardType.Movement) &&
            !inspectedCard.TypeFlags.HasFlag(CardData.CardType.Special))
        {
            mandatoryType = "Attack";
        }
        else if (inspectedCard.TypeFlags.HasFlag(CardData.CardType.Movement) &&
            !inspectedCard.TypeFlags.HasFlag(CardData.CardType.Attack) &&
            !inspectedCard.TypeFlags.HasFlag(CardData.CardType.Special))
        {
            mandatoryType = "Movement";
        }

        // Ensure mandartory type flag is present :
        if (mandatoryType != null && typeFlags != null)
        {
            CardData.CardType mandatoryEnum = (CardData.CardType)Enum.Parse(typeof(CardData.CardType), mandatoryType);

            if ((typeFlags.intValue & (int)mandatoryEnum) == 0)
            {
                // Set mandatory bit :
                typeFlags.intValue |= (int)mandatoryEnum; // Where x |= y means x = x | y (|= is the bitwise +=)

                // Apply immediately for rest of the UI sees it :
                serializedObject.ApplyModifiedProperties(); // Push
                serializedObject.Update(); // Pull
            }
        }

        // Draw flags field :
        if (typeFlags != null) EditorGUILayout.PropertyField(typeFlags, new GUIContent("Card Type(s) :"));

        // Commit user edits to the flags/list :
        bool changed = serializedObject.ApplyModifiedProperties();
        serializedObject.Update();

        int currentFlags = (typeFlags != null) ? typeFlags.intValue : 0;
        CardData.CardType flags = (CardData.CardType)currentFlags;

        // First, update serializedObject
        serializedObject.Update();

        // Store previous flags
        int previousFlagsCopy = previousFlags;  

        // Detect if only flags changed
        bool flagsChanged = currentFlags != previousFlagsCopy;

        if (flagsChanged && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            // Handle specialAbilities list creation/removal
            bool hadSpecial = (previousFlagsCopy & (int)CardData.CardType.Special) != 0;
            bool hasSpecial = (currentFlags & (int)CardData.CardType.Special) != 0;

            if (!hadSpecial && hasSpecial && specialAbilities != null && specialAbilities.arraySize == 0)
            {
                specialAbilities.arraySize++;
                SerializedProperty newElement = specialAbilities.GetArrayElementAtIndex(0);
                newElement.managedReferenceValue = null;
            }
            else if (hadSpecial && !hasSpecial && specialAbilities != null)
            {
                specialAbilities.ClearArray();
            }

            previousFlags = currentFlags;

            // Only reset focus for flags changes
            GUI.FocusControl(null);
            serializedObject.ApplyModifiedProperties();
            return;
        }

        if (mandatoryType != null) // Mandatory type (only for attacks and movements)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField($"{mandatoryType} (Mandatory)");
            EditorGUI.EndDisabledGroup();
        }

        // Display "Special" header dynamically :
        bool hasSpecialAbility = specialAbilities != null && specialAbilities.arraySize > 0;

        if (hasSpecialAbility) EditorGUILayout.LabelField("Special");

        // Draw non-mandatory types :
        foreach (CardData.CardType type in Enum.GetValues(typeof(CardData.CardType)))
        {
            if (type == CardData.CardType.None) continue;

            string typeName = type.ToString();

            if (typeName == mandatoryType) continue; // Skip mandatory

            if (type == CardData.CardType.Special && hasSpecialAbility) continue; // Already drawn dynamically above 

            if ((flags & type) != 0) EditorGUILayout.LabelField(typeName); // Draw any other type selected
        }

        // At least one visible, assigned type for special cards :
        if (inspectedCard.TypeFlags.HasFlag(CardData.CardType.Special))
        {
            bool hasAnyType =
                (inspectedCard.Abilities != null && inspectedCard.Abilities.Count > 0) || // Special abilities
                inspectedCard.TypeFlags.HasFlag(CardData.CardType.Attack) ||
                inspectedCard.TypeFlags.HasFlag(CardData.CardType.Movement);

            if (!hasAnyType)
            {
                EditorGUILayout.HelpBox(
                    "Special cards must have at least one type assigned (Attack, Movement, or a Special Ability).",
                    MessageType.Error
                );
            }
        }

        // Draw traits section :
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Type(s) Traits :", EditorStyles.boldLabel);

        // Draw behaviours types safely :
        bool hasAttackType = (flags & CardData.CardType.Attack) != 0;
        bool hasMovementType = (flags & CardData.CardType.Movement) != 0;
        bool hasSpecialType = (flags & CardData.CardType.Special) != 0;

        if (hasAttackType)
        {
            EditorGUILayout.PropertyField(attackSpecs);

            if (attackSpecs.managedReferenceValue == null) EditorGUILayout.HelpBox("Attack Behaviour is required when Attack type is assigned.", MessageType.Error); // Null-check
        }

        if (hasMovementType)
        {
            EditorGUILayout.PropertyField(movementSpecs);

            if (movementSpecs.managedReferenceValue == null) EditorGUILayout.HelpBox("Movement Behaviour is required when Movement type is assigned.", MessageType.Error); // Null-check
        }

        if (hasSpecialType && specialAbilities != null)
        {
            // Draw special properties list :
            EditorGUILayout.PropertyField(specialAbilities, new GUIContent("Special Abilities"), true);

            // Error if list is empty but Special type selected :
            if (specialAbilities.arraySize == 0) EditorGUILayout.HelpBox("At least one special ability must be added when Special type is selected.", MessageType.Error);

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

                Type abilityType = element.managedReferenceValue.GetType();

                // Delete element if already assigned, add it to already assigned types otherwise :
                if (alreadyAssignedAbilities.Contains(abilityType))
                {
                    Debug.Log($"{abilityType.Name} is already assigned on {inspectedCard.CardName}. Special abilities can only be assigned once.");
                    specialAbilities.DeleteArrayElementAtIndex(i);
                }
                else alreadyAssignedAbilities.Add(abilityType);
            }
        }

        // If specialAbilities got emptied by the user, auto-remove Special flag :
        if (!EditorApplication.isPlayingOrWillChangePlaymode && specialAbilities != null) // Edit-mode only
        {
            if (specialAbilities.arraySize == 0 && (flags & CardData.CardType.Special) != 0)
            {
                // remove Special bit from flags
                flags &= ~CardData.CardType.Special; // Where ~ is the bitwise NOT operator

                if (typeFlags != null)
                {
                    typeFlags.intValue = (int)currentFlags;
                    serializedObject.ApplyModifiedProperties();
                    GUI.FocusControl(null);
                    return;
                }
            }
        }

        // Write any remaining changes :
        serializedObject.ApplyModifiedProperties();
    }
}
