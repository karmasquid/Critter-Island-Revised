using System.Collections;
using UnityEngine;

public static class InputManager
{   //Modular input script that takes in the input and returns it.

    //---------------Attacks---------------
    public static bool Attack()
    {
        return Input.GetButton("Attack");
    }
    public static bool AttackUp()
    {
        return Input.GetButtonUp("Attack");
    }
    public static bool AttackDown()
    {
        return Input.GetButtonDown("Attack");
    }
    public static bool ThrowAttack()
    {
        return Input.GetButtonDown("ThrowAttack");
    }

    //---------------Interactions---------------
    public static bool Interact()
    {
        return Input.GetButtonDown("Interact");
    }

    //---------------Inventory show/hide---------------
    public static bool Inventory()
    {
        return Input.GetButtonDown("Inventory");
    }

    //---------------Dodge---------------
    public static bool Dodge()
    {
        return Input.GetButtonDown("Dodge");
    }



    //---------------Running---------------
    public static bool Run() //Down
    {
        return Input.GetButtonDown("Run");
    }
    public static bool Running() //Holding
    {
        return Input.GetButton("Run");
    }
    public static bool Ran() //Up
    {
        return Input.GetButtonUp("Run");
    }



    //---------------Axis Up/down/left/right---------------
    public static float Horizontal()
    {
        return Input.GetAxis("Horizontal");
    }
    public static float Vertical()
    {
        return Input.GetAxis("Vertical");
    }
    public static float RawHorizontal()
    {
        return Input.GetAxisRaw("Horizontal");
    }
    public static float RawVertical()
    {
        return Input.GetAxisRaw("Vertical");
    }
    //---------------Minor input movement check---------------
    public static bool MoveMe()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            return false;
        }
        else
            return true;
    }
}//Mattias Eriksson