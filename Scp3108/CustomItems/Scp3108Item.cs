// -----------------------------------------------------------------------
// <copyright file="Scp3108Item.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Scp3108.CustomItems
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API;
    using Exiled.CustomItems.API.Features;
    using Exiled.Events.EventArgs;
    using InventorySystem.Items.Pickups;
    using Scp3108.Models;
    using Scp914;
    using Scp914.Processors;
    using UnityEngine;
    using YamlDotNet.Serialization;

    /// <inheritdoc />
    public class Scp3108Item : CustomWeapon
    {
        private static readonly int PickupMask = LayerMask.GetMask("Pickup");

        /// <inheritdoc />
        public override uint Id { get; set; } = 3108;

        /// <inheritdoc />
        public override string Name { get; set; } = "Scp3108";

        /// <inheritdoc />
        public override string Description { get; set; } = "Transforms shot objects into an inferior version of itself. Has miscellaneous effects on sentient beings.";

        /// <inheritdoc />
        public override float Weight { get; set; } = 0f;

        /// <inheritdoc />
        public override SpawnProperties SpawnProperties { get; set; } = new()
        {
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new()
                {
                    Location = SpawnLocation.InsideHczArmory,
                    Chance = 100,
                },
            },
        };

        /// <inheritdoc />
        public override ItemType Type { get; set; } = ItemType.GunRevolver;

        /// <inheritdoc />
        public override byte ClipSize { get; set; } = 1;

        /// <inheritdoc />
        [YamlIgnore]
        public override float Damage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether players should be affected.
        /// </summary>
        [Description("Whether players should be affected.")]
        public bool AffectPlayers { get; set; } = true;

        /// <summary>
        /// Gets or sets the amount of effects to apply to a hit player per shot.
        /// </summary>
        [Description("The amount of effects to apply to a hit player per shot. If the same effect is chosen, the duration will stack.")]
        public int EffectsToApply { get; set; } = 1;

        /// <summary>
        /// Gets or sets the effects that can be applied to a hit player.
        /// </summary>
        [Description("The effects that can be applied to a hit player.")]
        public List<ConfiguredEffect> Effects { get; set; } = new()
        {
            new ConfiguredEffect(EffectType.Bleeding, 2, 10f),
            new ConfiguredEffect(EffectType.Concussed, 2, 10f),
        };

        /// <summary>
        /// Gets or sets a value indicating whether items should be affected.
        /// </summary>
        [Description("Whether items should be affected.")]
        public bool AffectItems { get; set; } = true;

        /// <summary>
        /// Gets or sets the setting to upgrade a shot item on.
        /// </summary>
        [Description("The setting to upgrade a shot item on.")]
        public Scp914KnobSetting UpgradeSetting { get; set; } = Scp914KnobSetting.Coarse;

        /// <inheritdoc />
        protected override void OnShooting(ShootingEventArgs ev)
        {
            Player target = Player.Get(ev.TargetNetId);
            if (target is not null)
            {
                AffectPlayer(target);
                return;
            }

            if (!AffectItems || !Physics.Raycast(ev.Shooter.CameraTransform.position, ev.Shooter.CameraTransform.forward, out RaycastHit hit, 100f, PickupMask))
                return;

            ItemPickupBase pickupBase = hit.collider.GetComponentInParent<ItemPickupBase>();
            if (pickupBase)
                AffectItem(pickupBase);
        }

        private void AffectPlayer(Player target)
        {
            if (AffectPlayers && Effects is { Count: > 0 })
            {
                for (int i = 0; i < EffectsToApply; i++)
                    Effects[Exiled.Loader.Loader.Random.Next(Effects.Count)].Apply(target);
            }
        }

        private void AffectItem(ItemPickupBase pickup)
        {
            if (Scp914Upgrader.TryGetProcessor(pickup.Info.ItemId, out Scp914ItemProcessor processor))
                processor.OnPickupUpgraded(UpgradeSetting, pickup, pickup.transform.position + Vector3.up);
        }
    }
}