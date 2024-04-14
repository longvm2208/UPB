using System;
using UnityEngine;

namespace EventStateMachine.Example
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField]
        private State stateMachine = new State();

        private void Start()
        {
            stateMachine.ChangeState(State1);
        }

        private void Update()
        {
            stateMachine?.Update();
        }

        #region State Machine
        private void State1(ref Action onEnter, ref Action onUpdate, ref Action onExit)
        {
            onEnter = () =>
            {
                Debug.Log("Enter state 1");
            };

            onUpdate = () =>
            {
                Debug.Log("Update state 1");

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    stateMachine.ChangeState(State2);
                }
            };

            onExit = () =>
            {
                Debug.Log("Exit state 1");
            };
        }

        private void State2(ref Action onEnter, ref Action onUpdate, ref Action onExit)
        {
            onEnter = () =>
            {
                Debug.Log("Enter state 2");
            };

            onUpdate = () =>
            {
                Debug.Log("Update state 2");

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    stateMachine.ChangeState(State1);
                }
            };

            onExit = () =>
            {
                Debug.Log("Exit state 2");
            };
        }
        #endregion
    }
}
