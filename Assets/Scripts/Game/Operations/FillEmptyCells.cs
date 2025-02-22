﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ilumisoft.Hex.Operations
{
    public class FillEmptyCells : IOperation
    {
        IGameGrid grid;
        IGameTileFactory gameTileFactory;

        public FillEmptyCells(GameBoard gameBoard)
        {
            this.grid = gameBoard;
            this.gameTileFactory = gameBoard;
        }

        public IEnumerator Execute()
        {
            List<Vector2Int> emptyCells = FindEmptyCells();

            SpawnCells(emptyCells);

            yield return new WaitForSeconds(0.25f);
        }

        List<Vector2Int> FindEmptyCells()
        {
            List<Vector2Int> emptyCells = new List<Vector2Int>();

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    var raycast = new GameTileRaycast(grid.GetPosition(x, y), Vector2.zero, 0);

                    if (!raycast.Perform(out _))
                    {
                        emptyCells.Add(new Vector2Int(x, y));
                    }
                }
            }

            return emptyCells;
        }

        void SpawnCells(List<Vector2Int> cells)
        {
            foreach (var cell in cells)
            {
                SpawnCell(cell);
            }
        }

        void SpawnCell(Vector2Int cell)
        {
            Vector3 position = grid.GetPosition(cell.x, cell.y);

            gameTileFactory.Spawn(position);
        }
    }
}