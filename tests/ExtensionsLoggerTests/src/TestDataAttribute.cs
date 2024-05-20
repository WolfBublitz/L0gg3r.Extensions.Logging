using System;
using System.Collections.Generic;
using System.Reflection;

internal abstract class SimpleTestDataAttribute : Attribute, ITestDataSource
{
    public abstract string DisplayName { get; }

    public abstract object[] Data { get; }

    public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
    {
        yield return Data;
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data) => DisplayName;
}
