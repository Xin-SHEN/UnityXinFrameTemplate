using System.Collections.Generic;

namespace PAStateMachine
{
    public class StateMachine
    {
        private Dictionary<string, State> stateDictionary = new Dictionary<string, State>(); //状态字典
        private State currentState; //当前状态

        public StateMachine(State defaultState)
        {
            AddState(defaultState);
            currentState = defaultState;
            currentState.EnterAction();
        }

        /// <summary>
        /// API:增加状态，每一个状态都必须拥有唯一的KEY
        /// </summary>
        /// <param name="newState">新状态</param>
        /// <returns></returns>
        public bool AddState(State newState)
        {
            if (stateDictionary.ContainsKey(newState.Name))
                return false;
            stateDictionary.Add(newState.Name, newState);
            return true;
        }

        /// <summary>
        /// API:改变状态，当前状态的离开行为将和下一个状态的进入行为同时发生。
        /// </summary>
        /// <param name="stateName">下一个状态</param>
        /// <returns></returns>
        public bool ChangeToState(string stateName)
        {
            if (!stateDictionary.ContainsKey(stateName))
            {
                return false;
            }
            if (currentState.ExitAction != null) currentState.ExitAction();
            currentState = stateDictionary[stateName];
            if (currentState.EnterAction != null) currentState.EnterAction();
            return true;
        }
    }
}

