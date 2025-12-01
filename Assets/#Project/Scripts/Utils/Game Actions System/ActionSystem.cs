using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IMPORTANT CONCEPTS:
//
// A GameAction is something that can be "performed" in steps:
//
// 1. PreReactions     → actions that run before the main performer
// 2. Performer         → the main logic of this action
// 3. PostReactions    → actions that run after the main performer
//
// The ActionSystem executes these actions IN ORDER using a coroutine pipeline
//
// A GameAction can trigger more GameActions → this creates action chains.
// The system ensures everything runs in the correct order, without overlap.

public class ActionSystem : Singleton<ActionSystem>
{
    // STATE :
    public bool IsPerforming { get; private set; } = false; // True when an action is currently running through the Flow() pipeline
    public event Action OnFlowFinished; // Triggered when the entire Flow(action) finishes
    private List<GameAction> currentReactions = null; // Temporary container that points to the current list to process (value adapts during Flow())


    // SUBSCRIBERS :
    // NB : Subscribers are NOT part of the GameAction but plain functions called before/after the action (listeners)
    private static Dictionary<Type, List<Action<GameAction>>> preSubscribers = new(); // All functions that trigger before a game action
    private static Dictionary<Type, List<Action<GameAction>>> postSubscribers = new(); // All functions that trigger after a game action


    // PERFORMERS :
    // The performers dictionary maps each GameAction type to the function that executes it.
    // Example: performers[MoveAction] = MyMovePerformer
    // When ActionSystem needs to run a MoveAction, it looks up the MoveAction performer and executes it.
    private static Dictionary<Type, Func<GameAction, IEnumerator>> performers = new(); // Performers hold the logic for their game action
    // NB : Each action type has exactly one performer that defines how it executes

    public enum ReactionTiming
    {
        Pre,
        Post
    }


    // FLOW = the complete lifecycle (flow) of a GameAction:
        // - Runs pre-subscribers (global listeners)
        // - Runs pre-reactions (GameAction-defined actions)
        // - Runs performer (main logic)
        // - Runs perform-reactions (GameAction-defined actions)
        // - Runs post-subscribers (global listeners)
        // - Runs post-reactions (GameAction-defined actions)
    private IEnumerator Flow(GameAction action) // Core of the ActionSystem
    {
        // Pre-effects & external listeners phase (pre-reactions) : 
        currentReactions = action.PreReactions; // Must be set correctly for PerformReactions() to use the correct list
        InvokeSubscribers(action, preSubscribers); // Trigger all pre-subscribers
        yield return RunReactions();

        // Main action :
        currentReactions = action.PerformActions; // PerformActions are optional (Main performer defines the core logic)
        yield return PerformPerformer(action); // Run the performer
        yield return RunReactions(); // Run any perform reactions

        // After-effects & cleanup phase (post-reactions) :
        currentReactions = action.PostReactions;
        InvokeSubscribers(action, postSubscribers); // Trigger all post-subscribers
        yield return RunReactions();

        IsPerforming = false;

        OnFlowFinished?.Invoke(); // Optional callback execution, after the Flow is done
    }

    private void InvokeSubscribers(GameAction action, Dictionary<Type, List<Action<GameAction>>> subs) // Runs all subscribers for a given action
    {
        Type actionType = action.GetType(); // Type of the action (MovementAction, AttackAction, ...)

        if (subs.ContainsKey(actionType)) // Check if the dictionary contains any subscribers for this type
        {
            // Loop trough each one and call it :
            foreach (Action<GameAction> sub in subs[actionType])
            {
                sub(action);
            }
        }
    }

    private IEnumerator RunReactions() // Coroutine that runs all reactions in currentReactions 
    {
        // Run each reaction in the reactions list and wait (yield return) until each reaction is finished before starting the next :
        foreach (GameAction reaction in currentReactions)
        {
            yield return Flow(reaction); // A reaction is a full action itself and can have its own pre/perf/post reactions → Chain of actions
        }
    }

    private IEnumerator PerformPerformer(GameAction action) // Runs the main effect of an action, if any
    {
        Type actionType = action.GetType();

        if (performers.ContainsKey(actionType))
        {
            yield return performers[actionType](action);
        }
    }


    public Coroutine Perform(GameAction action, Action OnPerformFinished = null) // Requests an action to be performed
    {
        if (IsPerforming) return null; // Do nothing if already performing

        return StartCoroutine(Flow(action)); // Start Flow(action) as Unity Coroutine
    }

    public void AddReaction(GameAction reaction) // Allows new reactions to be inserted dynamically while running an action
    {
        currentReactions?.Add(reaction); // Inserted into the list being processed
    }


    public static void AttachPerformer<T>(Func<T, IEnumerator> typedPerformer) where T : GameAction // Defines how to execute a specific action type
    {
        Type actionType = typeof(T); // Get action type

        // Wrap typedPerformer into a function that accepts GameAction :
        IEnumerator WrappedPerformer(GameAction action) => typedPerformer((T)action);

        // Register in dictionary :
        performers[actionType] = WrappedPerformer;
    }

    public static void DetachPerformer<T>() where T : GameAction // Removes the performer previously attached for that action type
    {
        Type type = typeof(T);

        if (performers.ContainsKey(type)) performers.Remove(type);
    }

    public static void SubscribeReaction<T>(Action<T> reactionFunction, ReactionTiming reactionTiming) where T : GameAction // Allows registering a method to be run before or after actions of type T
    {
        Dictionary<Type, List<Action<GameAction>>> targetDict = (reactionTiming == ReactionTiming.Pre) ? preSubscribers : postSubscribers; // Choose correct dictionary 

        Type actionType = typeof(T);

        // Wrap Action<T> into Action<GameAction> :
        void Wrapped(GameAction action) => reactionFunction((T)action);

        if (!targetDict.ContainsKey(actionType)) targetDict[actionType] = new List<Action<GameAction>>(); // New dict entry

        targetDict[actionType].Add(Wrapped); // Adds subscriber to dict key
    }

    public static void UnsubscribeReaction<T>(Action<T> reactionFunction, ReactionTiming reactionTiming) where T : GameAction // Removes the reaction from the appropriate dictionary
    {
        Dictionary<Type, List<Action<GameAction>>> targetDict = (reactionTiming == ReactionTiming.Pre) ? preSubscribers : postSubscribers;

        Type actionType = typeof(T);

        if (!targetDict.ContainsKey(actionType)) return; // Do nothing if not in dictionary

        // NOTE: wrapped delegate will not match the original → removal probably won't work
        void Wrapped(GameAction action) => reactionFunction((T)action);

        targetDict[actionType].Remove(Wrapped);
    }
}
