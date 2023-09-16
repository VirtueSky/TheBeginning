// Animancer // https://kybernetik.com.au/animancer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Examples.StateMachines.Brains;
using Animancer.FSM;
using Animancer.Units;
using UnityEngine;

namespace Animancer.Examples.StateMachines.Weapons
{
    /// <summary>Causes a <see cref="Character"/> to attack in response to player input.</summary>
    /// <remarks>
    /// Normally this would be part of the <see cref="CharacterBrain"/>, but since we want to demonstrate both
    /// <see cref="KeyboardBrain"/> and <see cref="MouseBrain"/> in this example we have implemented it separately.
    /// </remarks>
    /// <example><see href="https://kybernetik.com.au/animancer/docs/examples/fsm/weapons">Weapons</see></example>
    /// https://kybernetik.com.au/animancer/api/Animancer.Examples.StateMachines.Weapons/AttackInput
    /// 
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Weapons - Attack Input")]
    [HelpURL(Strings.DocsURLs.ExampleAPIDocumentation + nameof(StateMachines) + "." + nameof(Weapons) + "/" + nameof(AttackInput))]
    public sealed class AttackInput : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private CharacterState _Attack;
        [SerializeField, Seconds] private float _AttackInputTimeOut = 0.5f;

        private StateMachine<CharacterState>.InputBuffer _InputBuffer;

        /************************************************************************************************************************/

        private void Awake()
        {
            _InputBuffer = new StateMachine<CharacterState>.InputBuffer(_Attack.Character.StateMachine);
        }

        /************************************************************************************************************************/

        private void Update()
        {
            if (Input.GetButtonDown("Fire2"))// Right Click by default.
            {
                _InputBuffer.Buffer(_Attack, _AttackInputTimeOut);
            }

            _InputBuffer.Update();
        }

        /************************************************************************************************************************/
    }
}
