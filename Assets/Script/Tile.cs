using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("UI")]
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI posText;

    public Tile cameFromTile;
    public bool isWalkable = true;
    public int gCost, hCost, fCost;
    int x, y;
    void Start()
    {
        UpdateTileInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateTileInfo()
    {
        int xPos=Mathf.RoundToInt(transform.position.x);
        int yPos = Mathf.RoundToInt(transform.position.z);
        posText.text="( "+xPos+","+yPos+" )";
        x=xPos;
        y=yPos;
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }
    public string GetTileName()
    {
        return posText.text;
    }

    public void UpdateTileUI(bool shouldEnable)
    {
        posText.gameObject.SetActive(shouldEnable);
    }
    
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
