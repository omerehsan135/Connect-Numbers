using UnityEngine;

namespace Ilumisoft.Hex
{
    public class TileRenderer : MonoBehaviour
    {
        [SerializeField]
        SpriteRenderer spriteRenderer = null;

        [SerializeField]
        TMPro.TextMeshPro textMesh = null;

        HexGameTile gameTile;

        TileLevelBehaviour levelBehaviour;

        private void Awake()
        {
            gameTile = GetComponent<HexGameTile>();
            gameTile.OnTileDestroyed += OnTileDestroyed;
            gameTile.OnLevelChanged += OnLevelChanged;

            levelBehaviour = gameTile.GetComponent<TileLevelBehaviour>();
        }

        private void OnLevelChanged()
        {
            spriteRenderer.color = levelBehaviour.Color;
            textMesh.text = (Mathf.Pow(2,levelBehaviour.CurrentLevel+1)).ToString();
        }

        private void OnTileDestroyed(GameTile tile)
        {
            spriteRenderer.gameObject.SetActive(false);
        }
    }
}