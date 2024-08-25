using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Scripts.Misc;
using UnityEngine;

namespace Scripts.Core.Level {
    public static class LevelFactory {
        [System.Flags]
        private enum LevelModifiers {
            MoreEnemies = 1,
            HarderEnemies = 2,
            Longer = 4,
            Faster = 8,
        }
        
        public static LevelData GenerateLevel(int level) {
            var modifiers = GenerateModifiers(level);
            Debug.Log("mods: " + modifiers);
            var speed = GetSpeeds(level, modifiers);
            var obstacles = GetObstacles(level, modifiers);
            var credits = GetCredits(obstacles);
                
            LevelData levelData = new() {
                StartingSpeed = speed.startSpeed,
                EndingSpeed = speed.endSpeed,
                Obstacles = obstacles,
                Credits = credits
            };
            Debug.Log(levelData);
            return levelData;
        }

        private static LevelModifiers GenerateModifiers(int level) {
            int numModifiers = 0;
            if (level >= 3) {
                numModifiers = (level - 3) / 4 + 1;
            }
            numModifiers = Mathf.Min(numModifiers, 4);
            Debug.Log($"num modifiers: {numModifiers}");
            
            List<LevelModifiers> possibleMods = System.Enum.GetValues(typeof(LevelModifiers)).Cast<LevelModifiers>().ToList();
            List<LevelModifiers> chosenMods = new(possibleMods.Count);

            while (chosenMods.Count < numModifiers) {
                LevelModifiers chosen = possibleMods.GetRandom();
                possibleMods.Remove(chosen);
                chosenMods.Add(chosen);
            }

            LevelModifiers mods = 0;
            foreach (var chosenMod in chosenMods) {
                mods += (int)chosenMod;
            }

            return mods;
        }

        private static (int startSpeed, int endSpeed) GetSpeeds(int level, LevelModifiers modifiers) {
            int startSpeed = (level - 1) * 5 + 15;
            int endSpeed = startSpeed * 2;

            bool hasFasterModifier = modifiers.HasFlag(LevelModifiers.Faster);
            float factor = modifiers.HasFlag(LevelModifiers.Faster) ? 1.5f : 1.0f;
            return ((int)(startSpeed * factor), (int)(endSpeed * factor));
        }

        private static float[] GetObstacles(int level, LevelModifiers modifiers) {
            if (modifiers.HasFlag(LevelModifiers.Longer)) {
                return CreateEvenlySpacedArray(0, 40, 30 + (level - 1) * 4);
            }
            else {
                return CreateEvenlySpacedArray(0, 40, 20 + (level - 1) * 2);
            }
        }

        private static float[] GetCredits(float[] obstacles) {
            const float CREDIT_CHANCE = .5f;
            const float DOUBLE_CREDIT_CHANCE = .25f;
            const float TRIPLE_CREDIT_CHANCE = .1f;
            
            List<float> credits = new();
            for (int i = 1; i < obstacles.Length - 1; i++) {
                float obstacleStart = obstacles[i];
                float nextObstacle = obstacles[i + 1];

                if (Ran01 < CREDIT_CHANCE) {
                    if (Ran01 < TRIPLE_CREDIT_CHANCE) {
                        float randomPosition1 = Random.Range(.2f, .35f);
                        float randomPosition2 = Random.Range(.4f, .6f);
                        float randomPosition3 = Random.Range(.65f, .8f);
                        credits.Add(Mathf.Lerp(obstacleStart, nextObstacle, randomPosition1));
                        credits.Add(Mathf.Lerp(obstacleStart, nextObstacle, randomPosition2));
                        credits.Add(Mathf.Lerp(obstacleStart, nextObstacle, randomPosition3));
                    }
                    else if (Ran01 < DOUBLE_CREDIT_CHANCE) {
                        float randomPosition1 = Random.Range(.2f, .45f);
                        float randomPosition2 = Random.Range(.55f, .8f);
                        credits.Add(Mathf.Lerp(obstacleStart, nextObstacle, randomPosition1));
                        credits.Add(Mathf.Lerp(obstacleStart, nextObstacle, randomPosition2));
                    }
                    else {
                        // Just a single one
                        float randomPosition = Random.Range(.25f, .75f);
                        credits.Add(Mathf.Lerp(obstacleStart, nextObstacle, randomPosition));
                    }
                }
            }

            return credits.ToArray();
        }
        
        private static float[] CreateEvenlySpacedArray(float startAt, float distance, int number) {
            float[] array = new float[number];
            for (int i = 0; i < array.Length; i++) {
                array[i] = startAt + distance * i;
            }

            return array;
        }

        private static float Ran01 => Random.Range(0.0f, 1.0f);
        
    }
}