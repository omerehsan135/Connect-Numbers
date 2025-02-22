﻿using UnityEngine;

namespace Ilumisoft.Hex
{
    public struct GridDistanceValidator : IConnectionValidator
    {
        float maxDistance;

        public GridDistanceValidator(IGameGrid grid)
        {
            this.maxDistance = Mathf.Sqrt(2 * grid.CellSize * grid.CellSize);
        }

        public bool IsValid(GameTile first, GameTile second)
        {
            var distance = Vector2.Distance(first.transform.position, second.transform.position);

            if (distance < maxDistance || Mathf.Approximately(maxDistance, distance))
            {
                return true;
            }

            return false;
        }
    }
}