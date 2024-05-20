// ----------------------------------------------------------------------------
// <copyright file="ActionDisposable.cs" company="L0gg3r">
// Copyright (c) L0gg3r Project
// </copyright>
// ----------------------------------------------------------------------------

using System;

namespace L0gg3r.Extensions.Logging;

internal sealed class ActionDisposable : IDisposable
{
    private readonly Action action;

    public ActionDisposable(Action action)
    {
        this.action = action;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        action();
    }
}
