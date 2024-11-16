using LostToUnderswingCounter.Configuration;
using System.Globalization;
using TMPro;
using UnityEngine;
using Zenject;

namespace LostToUnderswingCounter.Counter
{
    internal class Counter : CountersPlus.Counters.Custom.BasicCustomCounter
    {
        private int currentScoreWithoutUnderswing = 0;

        private int currentScoreWithoutUnderswingLeft = 0;
        private int currentScoreWithoutUnderswingRight = 0;

        private int currentScoreLeft = 0;
        private int currentScoreRight = 0;

        private int immediateMaxPossibleLeftHandScore = 0;
        private int immediateMaxPossibleRightHandScore = 0;
        private int immediateScore => currentScoreLeft + currentScoreRight;

        [Inject] private readonly ScoreController scoreController;
        [InjectOptional] private readonly GameplayCoreSceneSetupData gameplayCoreSceneSetupData;

        private TMP_Text leftText;
        private TMP_Text rightText;
        private TMP_Text unifiedText;

        public override void CounterDestroy()
        {
            scoreController.scoringForNoteFinishedEvent -= calculateAccuracy;
        }

        public override void CounterInit()
        {
            if (PluginConfig.Instance.showHeaderText)
            {
                var label = CanvasUtility.CreateTextFromSettings(Settings);
                label.text = "Underswing Loss";
                label.fontSize = 3;
            }

            Vector3 leftOffset = new Vector3(0.25f, -0.3f);
            Vector3 rightOffset = new Vector3(-0.25f, -0.3f);
            Vector3 unifiedOffset = new Vector3(0, -0.3f);

            if (PluginConfig.Instance.style == PluginConfig.styleType.Both)
                unifiedOffset = new Vector3(0, -0.6f);

            if (PluginConfig.Instance.style == PluginConfig.styleType.Seperate || PluginConfig.Instance.style == PluginConfig.styleType.Both)
            {
                leftText = CanvasUtility.CreateTextFromSettings(Settings, leftOffset);
                leftText.text = $"-{0.ToString($"F{PluginConfig.Instance.decimalPrecision}", CultureInfo.InvariantCulture)}%";
                leftText.alignment = TextAlignmentOptions.Left;
                leftText.fontSize = 3;

                rightText = CanvasUtility.CreateTextFromSettings(Settings, rightOffset);
                rightText.text = $"-{0.ToString($"F{PluginConfig.Instance.decimalPrecision}", CultureInfo.InvariantCulture)}%";
                rightText.alignment = TextAlignmentOptions.Right;
                rightText.fontSize = 3;

                if (PluginConfig.Instance.inheritHandColors && gameplayCoreSceneSetupData != null)
                {
                    leftText.color = gameplayCoreSceneSetupData.colorScheme.saberBColor;
                    rightText.color = gameplayCoreSceneSetupData.colorScheme.saberAColor;
                }
            }

            if (PluginConfig.Instance.style == PluginConfig.styleType.Unified || PluginConfig.Instance.style == PluginConfig.styleType.Both)
            {
                unifiedText = CanvasUtility.CreateTextFromSettings(Settings, unifiedOffset);
                unifiedText.text = $"-{0.ToString($"F{PluginConfig.Instance.decimalPrecision}", CultureInfo.InvariantCulture)}%";
                unifiedText.fontSize = 3;
            }

            scoreController.scoringForNoteFinishedEvent += calculateAccuracy;
        }

        private void calculateAccuracy(ScoringElement element)
        {

            if (!(element is GoodCutScoringElement goodcut)) return;
            if (element.noteData.colorType == ColorType.None) return;

            if (element.noteData.scoringType != NoteData.ScoringType.Normal) return;

            var goodScoreElement = (GoodCutScoringElement) element;

            var buffer = goodScoreElement.cutScoreBuffer;

            int accuracy = buffer.centerDistanceCutScore;

            currentScoreWithoutUnderswing += (accuracy + 100) * goodScoreElement.multiplier;

            switch (goodScoreElement.noteData.colorType)
            {
                case ColorType.ColorB:
                    currentScoreLeft += goodScoreElement.cutScore * goodScoreElement.multiplier;
                    currentScoreWithoutUnderswingLeft += (accuracy + 100) * goodScoreElement.multiplier;
                    immediateMaxPossibleLeftHandScore += 115 * goodScoreElement.maxMultiplier;
                    break;
                case ColorType.ColorA:
                    currentScoreRight += goodScoreElement.cutScore * goodScoreElement.multiplier;
                    currentScoreWithoutUnderswingRight += (accuracy + 100) * goodScoreElement.multiplier;
                    immediateMaxPossibleRightHandScore += 115 * goodScoreElement.maxMultiplier;
                    break;
            }

            updateText();
        }

        private void updateText()
        {
            if (PluginConfig.Instance.style == PluginConfig.styleType.Seperate || PluginConfig.Instance.style == PluginConfig.styleType.Both)
            {
                var pointsLostToUnderswingLeft = currentScoreWithoutUnderswingLeft - currentScoreLeft;
                var percentLostToUnderswingLeft = ((float)pointsLostToUnderswingLeft / currentScoreLeft) * 100;

                var pointsLostToUnderswingRight = currentScoreWithoutUnderswingRight - currentScoreRight;
                var percentLostToUnderswingRight = ((float)pointsLostToUnderswingRight / currentScoreRight) * 100;
                // WORKS PROPERLY DONT CHANGE EVER
                string leftString = $"-{percentLostToUnderswingLeft.ToString($"F{PluginConfig.Instance.decimalPrecision}", CultureInfo.InvariantCulture)}%";
                string rightString = $"-{percentLostToUnderswingRight.ToString($"F{PluginConfig.Instance.decimalPrecision}", CultureInfo.InvariantCulture)}%";

                if (!PluginConfig.Instance.showDifference)
                {
                    leftString = $"{((((float)currentScoreWithoutUnderswingLeft / immediateMaxPossibleLeftHandScore) * 100) + percentLostToUnderswingLeft).ToString($"F{PluginConfig.Instance.decimalPrecision}", CultureInfo.InvariantCulture)}%";
                    rightString = $"{((((float)currentScoreWithoutUnderswingRight / immediateMaxPossibleRightHandScore) * 100) + percentLostToUnderswingRight).ToString($"F{PluginConfig.Instance.decimalPrecision}", CultureInfo.InvariantCulture)}%";
                }

                leftText.text = leftString;
                rightText.text = rightString;
            }

            if (PluginConfig.Instance.style == PluginConfig.styleType.Unified || PluginConfig.Instance.style == PluginConfig.styleType.Both)
            {
                var pointsLostToUnderswing = currentScoreWithoutUnderswing - scoreController.multipliedScore;

                var percentLostToUnderswing = ((float)pointsLostToUnderswing / immediateScore) * 100;

                string unifiedString = $"-{percentLostToUnderswing.ToString($"F{PluginConfig.Instance.decimalPrecision}", CultureInfo.InvariantCulture)}%";
                if (!PluginConfig.Instance.showDifference)
                {
                    unifiedString = $"%{(((float)currentScoreWithoutUnderswing / scoreController.immediateMaxPossibleMultipliedScore) * 100).ToString($"F{PluginConfig.Instance.decimalPrecision}", CultureInfo.InvariantCulture)}";
                }

                unifiedText.text = unifiedString;
            }
        }
    }
}
