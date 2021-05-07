using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevels
{
    public enum stars
    {
        none,
        first,
        second,
        third
    }

    private static Dictionary<GameController.Difficulty, Dictionary<stars, int>> scoreTresholds =
        new Dictionary<GameController.Difficulty, Dictionary<stars, int>>
        {
            {GameController.Difficulty.Pussy,
                new Dictionary<stars, int>
                {
                    { stars.first, 20000 },
                    { stars.second, 50000 },
                    { stars.third, 100000 }
                }
            },
            {GameController.Difficulty.VeryHard,
                new Dictionary<stars, int>
                {
                    { stars.first, 100000 },
                    { stars.second, 200000 },
                    { stars.third, 300000 }
                }
            },
            {GameController.Difficulty.Jedi,
                new Dictionary<stars, int>
                {
                    { stars.first, 200000 },
                    { stars.second, 400000 },
                    { stars.third, 600000 }
                }
            }
        };
    public static Dictionary<stars, int> getScoreTresholdsFor(GameController.Difficulty difficulty)
    {
        return scoreTresholds[difficulty];
    }
}
