// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp3108
{
    using Exiled.API.Interfaces;
    using Scp3108.CustomItems;

    /// <inheritdoc />
    public class Config : IConfig
    {
        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the configurable instance of the <see cref="Scp3108Item"/> item.
        /// </summary>
        public Scp3108Item Scp3108Item { get; set; } = new();
    }
}