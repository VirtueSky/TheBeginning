// Animancer // https://kybernetik.com.au/animancer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using System;
using UnityEngine;

namespace Animancer.Examples.StateMachines.Brains
{
    /// <summary>The numerical details of a <see cref="Character"/>.</summary>
    /// <example><see href="https://kybernetik.com.au/animancer/docs/examples/fsm/brains">Brains</see></example>
    /// https://kybernetik.com.au/animancer/api/Animancer.Examples.StateMachines.Brains/CharacterStats
    /// 
    [Serializable]
    public sealed class CharacterStats
    {
        /************************************************************************************************************************/

        [SerializeField, MetersPerSecond]
        private float _WalkSpeed = 2;
        public float WalkSpeed => _WalkSpeed;

        [SerializeField, MetersPerSecond]
        private float _RunSpeed = 4;
        public float RunSpeed => _RunSpeed;

        public float GetMoveSpeed(bool isRunning) => isRunning ? _RunSpeed : _WalkSpeed;

        /************************************************************************************************************************/

        [SerializeField, DegreesPerSecond]
        private float _TurnSpeed = 360;
        public float TurnSpeed => _TurnSpeed;

        /************************************************************************************************************************/

        // Max health.
        // Strength, dexterity, intelligence.
        // Carrying capacity.
        // Etc.

        /************************************************************************************************************************/
    }
}
