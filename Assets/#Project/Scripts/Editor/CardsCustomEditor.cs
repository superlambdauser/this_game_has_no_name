using UnityEditor;

// Draw a custom Editor for Card objects letting you enter data depending on the type(s) assigned to the card :
[CustomEditor(typeof(Card), true)] // Draws a custom Editor for Card objects letting you enter data depending on the type(s) assigned to the card. NB : Default value of the bool overload is FALSE which doesn't show the custom inspector for children of the inspected type (in this case, Card is an abstract class so we must set it to true !)
public class CardsCustomEditor : Editor // !!! Script must be place in a directory "Editor" anywhere inside the "Assets" folder.
{
    private SerializedProperty cardTypes;
    private SerializedProperty attackBehaviour;
    private SerializedProperty movementBehaviour;
    private SerializedProperty specialAbility;

    private void OnEnable() // Called every time the inspector opens
    {
        // NB : serializedObject = Unity's wrapper around selected Card instance

        cardTypes = serializedObject.FindProperty("cardTypes"); // Returns an array that contains all the card types entered for the card
        attackBehaviour = serializedObject.FindProperty("attackBehaviour");
        movementBehaviour = serializedObject.FindProperty("movementBehaviour");
        specialAbility = serializedObject.FindProperty("specialAbility");
    }

    public override void OnInspectorGUI() // Called every time Unity draws the inspector -> Updates every frame while the inspector is visible + every time a value is changed or the layout updates
    {
        serializedObject.Update(); // Sync SerializedObject with current values

        // Draw everything normally except for behaviours :
        DrawPropertiesExcluding(serializedObject, "attackBehaviour", "movementBehaviour", "specialAbility");

        // Determine which card types are attributed :
        bool hasAttack = HasCardType("Attack");
        bool hasMovement = HasCardType("Movement");
        bool hasSpecial = HasCardType("Special");

        // Editor visual settings for clarity :
        EditorGUILayout.Space(); // Make space in the inspector
        EditorGUILayout.LabelField("Custom card traits :", EditorStyles.boldLabel); // LabelField() is required here because [Header("")] is ignored in custom inspectors

        // Draw behaviour fields only if ther are in the card types list :
        if (hasAttack)
        {
            EditorGUILayout.PropertyField(attackBehaviour);

            if (attackBehaviour.objectReferenceValue == null) EditorGUILayout.HelpBox("Attack Behaviour is required when Attack type is assigned.", MessageType.Error);
        }

        if (hasMovement)
        {
            EditorGUILayout.PropertyField(movementBehaviour);

            if (movementBehaviour.objectReferenceValue == null) EditorGUILayout.HelpBox("Movement Behaviour is required when Movement type is assigned.", MessageType.Error);

        }

        if (hasSpecial) 
        {
            EditorGUILayout.PropertyField(specialAbility);

            if (specialAbility.objectReferenceValue == null) EditorGUILayout.HelpBox("Special Ability is required when Movement type is assigned.", MessageType.Error);

        }

        // Apply changes to the SerializedObject
        serializedObject.ApplyModifiedProperties();
    }

    private bool HasCardType(string typeName) // Where typeName must exactly match the enum name
    {
        for (int i = 0; i < cardTypes.arraySize; i++)
        {
            SerializedProperty element = cardTypes.GetArrayElementAtIndex(i);

            // Compare element to enum name :
            if (element.enumDisplayNames[element.enumValueIndex] == typeName) return true;
        }

        return false;
    }
}
