using PAStateMachine;
using UnityEngine;

public class PASMClass : MonoBehaviour
{
    //---序列化变量-------------------------------
    //[SerializeField] private float variable = 12345678;   //需要在面板内拖拽的变量一
    
    //---状态变量---------------------------------
    public StateMachine stateMachine;
    enum StatesList { /*状态一,状态二,状态三*/ }
    //int state_1_variable_1 = 12345678         //状态一变量一
    //float state_1_variable_2 = 12345678       //状态一变量二

    //---内部变量---------------------------------   
    //int local_variable_1 = 12345678           //.......

    void Awake() { }
    void Start() { InitStateMachine(); }
    void Update() { }
    void LateUpdate() { }

    // 初始化状态机
    void InitStateMachine() { }
}
