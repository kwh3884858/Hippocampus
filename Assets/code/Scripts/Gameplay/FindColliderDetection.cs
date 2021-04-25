using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindColliderDetection 
{
    public Collider RaycastForwardFindCollider(Transform raycastOrigin, float raycastLength, LayerMask raycastMask, LayerMask interactableMask)
    {
        Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, raycastLength, raycastMask.value))
        {

            if (hit.collider)
            {
                if (hit.collider.gameObject)
                {
                    if (((1 << hit.collider.gameObject.layer) | interactableMask) != 0)
                    {
                        return hit.collider;
                    }
                }
            }
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }

        return null;
    }

    public Collider OverlapSphereDetectionFindNearestCollider(Transform raycastOrigin, float interactableRadius, LayerMask interactableLayer)
    {
        Collider interactCollider = null;

        const int maxColliders = 10;
        Collider[] hitColliders = new Collider[maxColliders];
        Dictionary<Collider, float> colliders = new Dictionary<Collider, float>();

        float smallestLength = Default_Max_Distance;

        int numColliders = Physics.OverlapSphereNonAlloc(raycastOrigin.position, interactableRadius, hitColliders, interactableLayer.value);
        //Debug.Log ("Num of Collisions: " + numColliders);
        for (int i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].CompareTag(GamePlay.InteractiveObject.INTERACTABLE_TAG))
            {
                colliders.Add(hitColliders[i], Vector3.SqrMagnitude(hitColliders[i].transform.position - raycastOrigin.position));
            }
        }
        foreach (var pair in colliders)
        {
            if (pair.Value < smallestLength)
            {
                smallestLength = pair.Value;
                interactCollider = pair.Key;
            }
        }

        return interactCollider;
    }

    public List<Collider> OverlapSphereDetectionFindCollider(Vector3 originalPosition, float interactableRadius, LayerMask interactableLayer, int maxColliders = 10)
    {
        Collider[] hitColliders = new Collider[maxColliders];
        List<Collider> outputList = new List<Collider>();
        int numColliders = Physics.OverlapSphereNonAlloc(originalPosition, interactableRadius, hitColliders, interactableLayer.value);
        //Debug.Log ("Num of Collisions: " + numColliders);
        for (int i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].CompareTag(GamePlay.InteractiveObject.INTERACTABLE_TAG))
            {
                outputList.Add(hitColliders[i]);
            }
        }
        return outputList;
    }

    public const int Default_Max_Distance = 1000;
}
