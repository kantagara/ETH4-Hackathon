using System;
using System.Collections.Generic;


/// <summary>
///     Represents a generic event system handling events of type <typeparamref name="T" />,
/// </summary>
/// <remarks>
///     This class uses a static <see cref="HashSet{T}" /> to store unique event handler methods
///     and a static event delegate to invoke these methods. It provides functionality to
///     subscribe to, unsubscribe from, and invoke events.
/// </remarks>
/// <typeparam name="T">The type of the event arguments. Must be a subclass of <see cref="EventArgs" />.</typeparam>
public static class EventSystem<T> where T : EventArgs
{
    /// <summary>
    ///     Stores unique event handler methods.
    /// </summary>
    private static readonly HashSet<Action<T>> _events = new();

    /// <summary>
    ///     Represents the event that gets invoked.
    /// </summary>
    private static Action<T> _event;

    /// <summary>
    ///     Subscribes a method to the event system.
    /// </summary>
    /// <param name="method">The method to subscribe as an event handler.</param>
    /// <exception cref="InvalidOperationException">Thrown when trying to subscribe a method that is already subscribed.</exception>
    public static void Subscribe(Action<T> method)
    {
        if (_events.Contains(method))
            throw new InvalidOperationException("Attempting to subscribe a method that is already subscribed.");
        _event += method;
    }

    /// <summary>
    ///     Unsubscribes a method from the event system.
    /// </summary>
    /// <param name="method">The method to unsubscribe.</param>
    /// <remarks>
    ///     There is no effect if the method being unsubscribed was not previously subscribed.
    /// </remarks>
    public static void Unsubscribe(Action<T> method)
    {
        _event -= method;
    }

    /// <summary>
    ///     Invokes the event, calling all subscribed methods with the provided arguments.
    /// </summary>
    /// <param name="args">The event arguments to pass to the subscribed methods.</param>
    public static void Invoke(T args)
    {
        _event?.Invoke(args);
    }
}