using UnityEngine;

public class TileData
{
    private Vector3 position;
    // occupied ? walkable ? tbd later

    #region Custom methods
    public TileData(Vector3 position) // Constructor
    {
        this.position = position;
    }
    #endregion
}
