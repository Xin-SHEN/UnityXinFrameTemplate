using System;

namespace PAStateMachine
{
    public class State
    {
        public string Name; // 状态名称，该名称在状态字典必须是唯一的
        public Action EnterAction, ExitAction; // 进入行为，离开行为

        /// <summary>
        /// 新建一个状态时必须指定【名称】和【进入行为】，【离开行为】可以不指定
        /// </summary>
        /// <param name="name">状态名称</param>
        /// <param name="enterAction">进入行为</param>
        /// <param name="exitAction">离开行为</param>
        public State(string name, Action enterAction, Action exitAction = null)
        {
            this.Name = name;
            this.EnterAction = enterAction;
            this.ExitAction = exitAction;
        }
    }
}
