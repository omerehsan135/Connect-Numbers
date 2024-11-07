using Ilumisoft.Hex.Events;
using System.Collections;
using UnityEngine;

namespace Ilumisoft.Hex.Operations
{
    public class MergeSelection : IOperation
    {
        IGameBoard gameBoard;
        ISelection selection;
        IValidator selectionValidator;

        public MergeSelection(IGameBoard gameBoard, ISelection selection)
        {
            this.gameBoard = gameBoard;
            this.selection = selection;
            this.selectionValidator = new SelectionValidator(selection);
        }

        public IEnumerator Execute()
        {
            // Cancel if the selection is not valid
            if (selectionValidator.IsValid == false)
            {
                selection.Clear();
                yield break;
            }

            int sum = GetSelectionSum();
            int newLevel = (int)Mathf.Log(sum, 2);

            var last = selection.GetLast();

            ClearSelectionLine();

            GameEvents<SFXEventType>.Trigger(SFXEventType.Merge);

            MoveSelected(last.transform.position);

            Score.Add(sum);

            yield return new WaitForTileMovement(gameBoard);

            LevelUp(last, newLevel);

            GameEvents<SFXEventType>.Trigger(SFXEventType.Pop);

            PopSelected();

            selection.Clear();

            yield return new WaitForSeconds(0.2f);
        }

        private int GetSelectionSum()
        {
            int result = 0;

            for (int i = 0; i < selection.Count; i++)
            {
                var tileLevel = selection.Get(i).GetComponent<ICanLevelUp>().CurrentLevel;

                result += (int)Mathf.Pow(2, tileLevel);
            }

            return result;
        }

        private void LevelUp(GameTile gameTile, int newLevel)
        {
            if (gameTile is ICanLevelUp canLevelUp)
            {
                canLevelUp.CurrentLevel = newLevel;

                selection.Remove(gameTile);

                if(newLevel>GameBoard.MaxReachedLevel)
                {
                    GameBoard.MaxReachedLevel = newLevel;
                }
            }
        }

        private void ClearSelectionLine()
        {
            if (selection is LineSelection lineSelection)
            {
                lineSelection.ClearLine();
            }
        }

        private void MoveSelected(Vector3 position)
        {
            for (int i = 0; i < selection.Count - 1; i++)
            {
                var gameTile = selection.Get(i);

                if (gameTile is ICanMoveTo canMoveTo)
                {
                    canMoveTo.MoveTo(position, 0.2f);
                }
            }
        }

        private void PopSelected()
        {
            for (int i = 0; i < selection.Count; i++)
            {
                var gameTile = selection.Get(i);

                gameTile.Pop();
            }
        }
    }
}