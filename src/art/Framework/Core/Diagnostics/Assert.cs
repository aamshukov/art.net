//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace UILab.Art.Framework.Core.Diagnostics;

public static class Assert
{

    public static void NonNullReference([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
    {
        if(string.IsNullOrWhiteSpace(argumentName))
        {
            StackFrame frame = new(1, true);
            throw new ArgumentNullException($"Null reference or empty 'argName' is passed to the method '{frame.GetMethod()}: {frame.GetFileName()}, {frame.GetFileLineNumber()}.");
        }

        if(argument is null)
        {
            StackFrame frame = new(1, true);
            throw new ArgumentNullException($"Null reference or empty '{argumentName}' is passed to the method '{frame.GetMethod()}: {frame.GetFileName()}, {frame.GetFileLineNumber()}.");
        }
    }

    public static void NonEmptyString(string argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
    {
        NonNullReference(argument);
        NonNullReference(argumentName);

        if(string.IsNullOrWhiteSpace(argument))
        {
            StackFrame frame = new(1, true);
            throw new ArgumentNullException($"The string '{argumentName}' provided to the method '{frame.GetMethod()} is not valid (null or empty): {frame.GetFileName()}, {frame.GetFileLineNumber()}.");
        }
    }

    public static void NonEmptyCollection<T>(IEnumerable<T> collection, [CallerArgumentExpression(nameof(collection))] string? argumentName = null)
    {
        NonNullReference(collection);
        NonNullReference(argumentName);

        if(!collection.Any())
        {
            StackFrame frame = new(1, true);
            throw new ArgumentException($"The collection '{collection}' provided to the method '{frame.GetMethod()} is empty: {frame.GetFileName()}, {frame.GetFileLineNumber()}.");
        }
    }

    public static void NotImplemented()
    {
        StackFrame frame = new(1, true);
        throw new NotImplementedException($"The required method or operation '{frame.GetMethod()}' is not implemented: {frame.GetFileName()}, {frame.GetFileLineNumber()}.");
    }

    public static void NonDisposed(bool disposed)
    {
        if(disposed)
        {
            StackFrame frame = new(1, true);
            throw new ObjectDisposedException($"Operation is performed on disposed object in '{frame.GetMethod()}': {frame.GetFileName()}, {frame.GetFileLineNumber()}.");
        }
    }

    public static void NonDisposed(object obj)
    {
        NonNullReference(obj);

        if(obj is Disposable disposableObject && disposableObject.Disposed)
        {
            StackFrame frame = new(1, true);
            throw new ObjectDisposedException($"Operation is performed on disposed object '{disposableObject.GetType().FullName ?? string.Empty}' in '{frame.GetMethod()}': {frame.GetFileName()}, {frame.GetFileLineNumber()}.");
        }
    }

    public static void Ensure(bool condition, string? message, Type? exceptionType = default, Exception? internalException = default)
    {
        if(!condition)
        {
            StackFrame frame = new(1, true);

            var composedMessage = $"{message}{Environment.NewLine}Condition failed in '{frame.GetMethod()}': {frame.GetFileName()}, {frame.GetFileLineNumber()}.";

            if(exceptionType != default)
            {
                var exception = Activator.CreateInstance(exceptionType, composedMessage, internalException);

                if(exception != default)
                {
                    throw (Exception)exception;
                }
                else
                {
                    throw new InvalidOperationException(composedMessage, internalException);
                }
            }
            else
            {
                throw new InvalidOperationException(composedMessage, internalException);
            }
        }
    }

    public static void ThrowKeyNotFoundException(string message)
    {
        StackFrame frame = new(1, true);
        throw new KeyNotFoundException($"{message}{Environment.NewLine}Occured in '{frame.GetMethod()}': {frame.GetFileName()}, {frame.GetFileLineNumber()}.");
    }

    public static void ExpectedType(object obj, Type type)
    {
        Assert.NonNullReference(obj);
        Assert.NonNullReference(type);

        if(!type.IsInstanceOfType(obj))
        {
            StackFrame frame = new(1, true);
            throw new ArgumentException($"Invalid expected type in '{frame.GetMethod()}': {frame.GetFileName()}, {frame.GetFileLineNumber()}.");
        }
    }
}
