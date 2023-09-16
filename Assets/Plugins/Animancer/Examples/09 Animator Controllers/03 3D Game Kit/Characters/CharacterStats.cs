// Animancer // https://kybernetik.com.au/animancer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using System;
using UnityEngine;
using static Animancer.Validate;

namespace Animancer.Examples.AnimatorControllers.GameKit
{
    /// <summary>The numerical details of a <see cref="Character"/>.</summary>
    /// <example><see href="https://kybernetik.com.au/animancer/docs/examples/animator-controllers/3d-game-kit">3D Game Kit</see></example>
    /// https://kybernetik.com.au/animancer/api/Animancer.Examples.AnimatorControllers.GameKit/CharacterStats
    /// 
    [Serializable]
    public sealed class CharacterStats
    {
        /************************************************************************************************************************/

        [SerializeField, MetersPerSecond(Rule = Value.IsNotNegative)]
        private float _MaxSpeed = 8;
        public float MaxSpeed => _MaxSpeed;

        [SerializeField, MetersPerSecondPerSecond(Rule = Value.IsNotNegative)]
        private float _Acceleration = 20;
        public float Acceleration => _Acceleration;

        [SerializeField, MetersPerSecondPerSecond(Rule = Value.IsNotNegative)]
        private float _Deceleration = 25;
        public float Deceleration => _Deceleration;

        [SerializeField, DegreesPerSecond(Rule = Value.IsNotNegative)]
        private float _MinTurnSpeed = 400;
        public float MinTurnSpeed => _MinTurnSpeed;

        [SerializeField, DegreesPerSecond(Rule = Value.IsNotNegative)]
        private float _MaxTurnSpeed = 1200;
        public float MaxTurnSpeed => _MaxTurnSpeed;

        [SerializeField, MetersPerSecondPerSecond(Rule = Value.IsNotNegative)]
        private float _Gravity = 20;
        public float Gravity => _Gravity;

        [SerializeField, Multiplier(Rule = Value.IsNotNegative)]
        private float _StickingGravityProportion = 0.3f;
        public float StickingGravityProportion => _StickingGravityProportion;

        /************************************************************************************************************************/
    }
}
