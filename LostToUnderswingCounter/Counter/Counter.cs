using CountersPlus.Counters.Interfaces;
using LostToUnderswingCounter.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [Inject] private readonly ScoreController scoreController;
        [Inject] private readonly GameplayCoreSceneSetupData gameplayCoreSceneSetupData;

        private TMP_Text leftText;
        private TMP_Text rightText;

        public override void CounterDestroy() 
        { 
            scoreController.scoringForNoteFinishedEvent -= calculateAccuracy;
        }

        public override void CounterInit()
        {
            var label = CanvasUtility.CreateTextFromSettings(Settings);
            label.text = "Underswing Loss";
            label.fontSize = 3;

            Vector3 leftOffset = new Vector3(0f, -0.6f);
            Vector3 rightOffset = new Vector3(-0.25f, -.6f);

            if (PluginConfig.Instance.seperateHands)
            {
                leftOffset = new Vector3(.25f, -0.6f);

                leftText = CanvasUtility.CreateTextFromSettings(Settings, leftOffset);
                leftText.text = (-0).ToString($"F{PluginConfig.Instance.decimalPrecision}", CultureInfo.InvariantCulture);
                leftText.alignment = TextAlignmentOptions.Left;
                leftText.fontSize = 3;

                rightText = CanvasUtility.CreateTextFromSettings(Settings, rightOffset);
                rightText.text = (-0).ToString($"F{PluginConfig.Instance.decimalPrecision}", CultureInfo.InvariantCulture);
                rightText.alignment = TextAlignmentOptions.Right;
                rightText.fontSize = 3;

                if (PluginConfig.Instance.inheritHandColors)
                {
                    leftText.color = gameplayCoreSceneSetupData.colorScheme.saberBColor;
                    rightText.color = gameplayCoreSceneSetupData.colorScheme.saberAColor;
                }
            }
            else
            {
                leftText = CanvasUtility.CreateTextFromSettings(Settings, leftOffset);
                leftText.text = (-0).ToString($"F{PluginConfig.Instance.decimalPrecision}", CultureInfo.InvariantCulture);
                leftText.alignment= TextAlignmentOptions.Center;
                leftText.fontSize = 4;
            }
            

            scoreController.scoringForNoteFinishedEvent += calculateAccuracy;
        }

        private void calculateAccuracy(ScoringElement element)
        {
            
            if (!(element is GoodCutScoringElement goodcut)) return;
            

            var goodScoreElement = (GoodCutScoringElement) element;

            var buffer = goodScoreElement.cutScoreBuffer;

            int accuracy = buffer.centerDistanceCutScore;

            currentScoreWithoutUnderswing += (accuracy + 100) * goodScoreElement.multiplier;
            switch (element.noteData.colorType)
            {
                case ColorType.ColorB:
                    currentScoreLeft += goodScoreElement.cutScore * goodScoreElement.multiplier;
                    currentScoreWithoutUnderswingLeft += (accuracy + 100) * goodScoreElement.multiplier;
                    break;
                case ColorType.ColorA:
                    currentScoreRight += goodScoreElement.cutScore * goodScoreElement.multiplier;
                    currentScoreWithoutUnderswingRight += (accuracy + 100) * goodScoreElement.multiplier;
                    break;
            }
            updateText();
        }

        private void updateText()
        {
            if (PluginConfig.Instance.seperateHands)
            {
                var pointsLostToUnderswingLeft = currentScoreWithoutUnderswingLeft - currentScoreLeft;
                var percentLostToUnderswingLeft = ((float) pointsLostToUnderswingLeft / currentScoreLeft) * 100;
                leftText.text = $"-{percentLostToUnderswingLeft.ToString($"F{PluginConfig.Instance.decimalPrecision}", CultureInfo.InvariantCulture)}%";

                var pointsLostToUnderswingRight = currentScoreWithoutUnderswingRight - currentScoreRight;
                var percentLostToUnderswingRight = ((float)pointsLostToUnderswingRight / currentScoreRight) * 100;
                rightText.text = $"-{percentLostToUnderswingRight.ToString($"F{PluginConfig.Instance.decimalPrecision}", CultureInfo.InvariantCulture)}%";

                return;
            }

            var pointsLostToUnderswing = currentScoreWithoutUnderswing - scoreController.multipliedScore;

            var percentLostToUnderswing = ((float) pointsLostToUnderswing / scoreController.multipliedScore) * 100;

            leftText.text = $"-{percentLostToUnderswing.ToString($"F{PluginConfig.Instance.decimalPrecision}", CultureInfo.InvariantCulture)}%";
        }
    }
}
