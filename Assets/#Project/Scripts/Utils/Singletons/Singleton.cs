using UnityEngine;


// Singleton pattern adapted to Unity lifecycle methods :
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour // Constraint the singleton use to Monobehaviour to be able to be able to use Unity lifecycle methods 
{
    public static T Instance { get; private set; } // Single instance 

    protected virtual void Awake() // Can be overridden by children if needed
    {
        if (Instance != null) // Is there already an instance of the object ?
        {
            Destroy(gameObject); // Remove this object -> no duplicate
            return; 
        }

        Instance = this as T; // Assign current object to Instance if no existing instance
        // NB : this as T casts the current object to the specific type (T) that inherits from Singleton<T>
    }

    protected virtual void OnApplicationQuit()
    {
        // Reset :
        Instance = null; // Clear Instance

        Destroy(gameObject); // Ensure gameObject associated with Singleton is also destroyed
    }
}
