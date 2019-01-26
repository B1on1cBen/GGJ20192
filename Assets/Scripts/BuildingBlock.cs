using UnityEngine;

public class BuildingBlock : MonoBehaviour
{
    public GameObject Occupant;
    public GameObject[] Borders;

    public Vector3 GetNeighborPosition(int x, int y)
    {
        if (!NeighborExists(x, y))
            return transform.position;

        return Borders[ConvertXYToNeighborIndex(x, y)].transform.position;
    }

    public BuildingBlock GetNeighbor(int x, int y)
    {
        if (!NeighborExists(x, y))
            return null;

        return Borders[ConvertXYToNeighborIndex(x, y)].GetComponent<BuildingBlock>();
    } 

    private bool NeighborExists(int x, int y)
    {
        return Borders[ConvertXYToNeighborIndex(x, y)] != null;
    }

    private int ConvertXYToNeighborIndex(int x, int y)
    {
        if (y == 1)
            return 0;

        if (x == 1)
            return 1;

        if (y == -1)
            return 2;

        if (x == -1)
            return 3;

        return 0;
    }
}
