// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp3108
{
    using System;
    using Exiled.API.Features;
    using Exiled.CustomItems.API;

    /// <inheritdoc />
    public class Plugin : Plugin<Config>
    {
        /// <inheritdoc/>
        public override string Author => "Build";

        /// <inheritdoc/>
        public override string Name => "Scp3108Item";

        /// <inheritdoc/>
        public override string Prefix => "Scp3108Item";

        /// <inheritdoc/>
        public override Version Version { get; } = new(1, 0, 0);

        /// <inheritdoc/>
        public override Version RequiredExiledVersion { get; } = new(5, 1, 3);

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            Config.Scp3108Item?.Register();
            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            Config.Scp3108Item?.Unregister();
            base.OnDisabled();
        }
    }
}