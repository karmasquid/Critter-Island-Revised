using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Spelaren ska interager med detta script.
/// Står spelaren tillräckligt nära och trycker på den bestämda interaktionsknappen så händer det nåt. 
/// Har spelaren redan interagerat med objektet ska inget hända. Därför inkluderas en bool.
/// </summary>

public class Interact : MonoBehaviour
{
    public float interactRad = 0f;
    bool hasInteracted;

    void Start()
    {
        hasInteracted = false;
    }

    void Interaction()
    {
        if (interactRad =< /*playerRange */ && Input.GetButtonDown("e") && hasInteracted == false)
        {
            //kista öppnas, dörr öppnas, objekt plockas upp etc.
            hasInteracted = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRad);
    }
	
}
