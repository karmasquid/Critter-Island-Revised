using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RangeEnemy))]
public class AIRangeDrawRange : Editor
{

    private void OnSceneGUI()
    {
        RangeEnemy aI = (RangeEnemy)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(aI.transform.position, Vector3.up, Vector3.forward, 360, aI.ViewRange);
        Vector3 viewAngle1 = aI.DirectionsFromDegrees(-aI.ViewDeg / 2, false);
        Vector3 viewAngle2 = aI.DirectionsFromDegrees(aI.ViewDeg / 2, false);

        Handles.DrawLine(aI.transform.position, aI.transform.position + viewAngle1 * aI.ViewRange);
        Handles.DrawLine(aI.transform.position, aI.transform.position + viewAngle2 * aI.ViewRange);

        Handles.color = Color.red;
        Handles.DrawWireArc(aI.transform.position, Vector3.up, Vector3.forward, 360, aI.AttackRangeMax);
        Handles.color = Color.red;
        Handles.DrawWireArc(aI.transform.position, Vector3.up, Vector3.forward, 360, aI.AttackRangeMin);
    }
}

