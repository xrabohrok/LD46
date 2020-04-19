using UnityEngine;

class Food : MonoBehaviour
{
    private GameObject claimant;

    public bool claim(GameObject registrant)
    {
        if (claimant == null)
        {
            claimant = registrant;
            return true;
        }

        return claimant == registrant;
    }
}