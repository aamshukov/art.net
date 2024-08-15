//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Diagnostics;

namespace UILab.Art.Framework.Core.Diagnostics;

public static class Assert
{
    public static void NonNullReference(object arg, string argName)
    {
        if(string.IsNullOrWhiteSpace(argName))
        {
            StackFrame frame = new(1, true);
            throw new ArgumentNullException($"Null reference or empty 'argName' is passed to the method '{frame.GetMethod()}: {frame.GetFileName()}, {frame.GetFileLineNumber()}.");
        }

        if(arg == null)
        {
            StackFrame frame = new(1, true);
            throw new ArgumentNullException($"Null reference or empty '{argName}' is passed to the method '{frame.GetMethod()}: {frame.GetFileName()}, {frame.GetFileLineNumber()}.");
        }
    }

    public static void NonEmptyString(string arg, string argName)
    {
        NonNullReference(arg, nameof(arg));
        NonNullReference(argName, nameof(argName));

        if(string.IsNullOrWhiteSpace(arg))
        {
            StackFrame frame = new(1, true);
            throw new ArgumentNullException($"The string '{argName}' provided to the method '{frame.GetMethod()} is not valid (null or empty): {frame.GetFileName()}, {frame.GetFileLineNumber()}.");
        }
    }

    public static void NonEmptyCollection<T>(IEnumerable<T> collection, string argName)
    {
        NonNullReference(collection, nameof(collection));
        NonNullReference(argName, nameof(argName));

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

    public static void Ensure(bool condition, string message, Type? exceptionType = default)
    {
        if(!condition)
        {
            StackFrame frame = new(1, true);

            var composedMessage = $"{message}{Environment.NewLine}Condition failed in '{frame.GetMethod()}': {frame.GetFileName()}, {frame.GetFileLineNumber()}.";

            if(exceptionType != default)
            {
                var exception = Activator.CreateInstance(exceptionType, composedMessage);

                if(exception != default)
                {
                    throw (Exception)exception;
                }
                else
                {
                    throw new InvalidOperationException(composedMessage);
                }
            }
            else
            {
                throw new InvalidOperationException(composedMessage);
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
        Assert.NonNullReference(obj, nameof(obj));
        Assert.NonNullReference(type, nameof(type));

        if(!type.IsInstanceOfType(obj))
        {
            StackFrame frame = new(1, true);
            throw new ArgumentException($"Invalid expected type in '{frame.GetMethod()}': {frame.GetFileName()}, {frame.GetFileLineNumber()}.");
        }
    }
}
