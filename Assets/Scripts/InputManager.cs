using System.Collections;
using UnityEngine;

public static class InputManager
{
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


    public static bool Interact()
    {
        return Input.GetButtonDown("Interact");
    }


    public static bool Inventory()
    {
        return Input.GetButtonDown("Inventory");
    }


    public static bool Dodge()
    {
        return Input.GetButtonDown("Dodge");
    }




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

    public static bool MoveMe()
    {
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            return false;
        }
        else
            return true;
    }
}