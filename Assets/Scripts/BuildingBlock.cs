using UnityEngine;

public class BuildingBlock : MonoBehaviour
{
    public GameObject Occupant;
    public GameObject[] Borders;

    public Vector3 GetNeighborPosition(int x, int y)
    {
        if (!NeighborExists(x, y))
            return transform.position;

        return Borders[GameManager.ConvertXYToNeighborIndex(x, y)].transform.position;
    }

    public BuildingBlock GetNeighbor(int x, int y)
    {
        if (!NeighborExists(x, y))
            return null;

        return Borders[GameManager.ConvertXYToNeighborIndex(x, y)].GetComponent<BuildingBlock>();
    } 

    private bool NeighborExists(int x, int y)
    {
        return Borders[GameManager.ConvertXYToNeighborIndex(x, y)] != null;
    }
}
