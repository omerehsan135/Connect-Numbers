using Ilumisoft.Hex.Events;
using Ilumisoft.Hex.Operations;
using System.Collections;
using UnityEngine;

namespace Ilumisoft.Hex
{
    public class HexGameMode : GameMode
    {
        [SerializeField]
        GameBoard gameBoard = null;

        [SerializeField]
        SelectionLineRenderer lineRenderer = null;

        [SerializeField]
        LoadSavegameUI loadSavegameUIPrefab = null;

        ISelection selection;

        IGameOverCheck gameOverCheck;

        OperationQueue operations = new OperationQueue();

        HexSavesystem savesystem;

        private void Awake()
        {
            savesystem = FindObjectOfType<HexSavesystem>();

            selection = new LineSelection(lineRenderer);
            gameOverCheck = new HexGameOverCheck(gameBoard);

            operations.Clear();
            operations.Add(new ProcessInput(gameBoard, selection));
            operations.Add(new MergeSelection(gameBoard, selection));
            operations.Add(new ProcessVerticalMovement(gameBoard));
            operations.Add(new FillEmptyCells(gameBoard));
        }

        void ResetBoard()
        {
            GameBoard.MaxReachedLevel = 3;
            Score.Reset();

            gameBoard.Spawn();
        }

        public override IEnumerator StartGame()
        {
            if (savesystem.HasSavestate())
            {
                var savegameUI = Instantiate(loadSavegameUIPrefab);

                yield return savegameUI.Execute(savesystem.Load, ResetBoard);

                Destroy(savegameUI.gameObject);
            }
            else
            {
                ResetBoard();
            }

            yield return null;
        }

        public override IEnumerator RunGame()
        {
            while (IsGameOver() == false)
            {
                savesystem.Save();

                yield return new WaitForInput();

                yield return operations.Execute();
            }

            yield return new WaitForSeconds(1);
        }

        public override IEnumerator EndGame()
        {
            GameEvents<UIEventType>.Trigger(UIEventType.GameOver);

            savesystem.ClearSavestate();

            yield return null;
        }

        bool IsGameOver()
        {
            return gameOverCheck.IsGameOver();
        }
    }
}