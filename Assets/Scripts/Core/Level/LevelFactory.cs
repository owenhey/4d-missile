using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Scripts.Misc;
using UnityEngine;
using Random = UnityEngine.Random;

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
            var enemies = GetEnemies(level, modifiers);
                
            LevelData levelData = new() {
                StartingSpeed = speed.startSpeed,
                EndingSpeed = speed.endSpeed,
                Obstacles = obstacles,
                Credits = credits,
                Enemies = enemies,
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
            int endSpeed = (int)(startSpeed * 1.5f);

            bool hasFasterModifier = modifiers.HasFlag(LevelModifiers.Faster);
            float factor = modifiers.HasFlag(LevelModifiers.Faster) ? 1.5f : 1.0f;
            return ((int)(startSpeed * factor), (int)(endSpeed * factor));
        }

        private static ObstacleSpawnable[] GetObstacles(int level, LevelModifiers modifiers) {
            FloatSpawnable[] array;
            if (modifiers.HasFlag(LevelModifiers.Longer)) {
                array = CreateEvenlySpacedArray(0, 40, 30 + (level - 1) * 4);
            }
            else {
                array = CreateEvenlySpacedArray(0, 40, 20 + (level - 1) * 2);
            }

            List<ObstacleSpawnable> _obsSpawnables = new List<ObstacleSpawnable>(array.Length);
            for (int i = 0; i < array.Length; i++) {
                _obsSpawnables.Add(new ObstacleSpawnable(array[i].Value, i == array.Length - 1));
            }

            return _obsSpawnables.ToArray();
        }

        public static FloatSpawnable[] GetCredits<T>(IEnumerable<T> obstacles) where T : IDataSpawnable{
            const float CREDIT_CHANCE = .5f;
            const float DOUBLE_CREDIT_CHANCE = .25f;
            const float TRIPLE_CREDIT_CHANCE = .1f;
            
            List<FloatSpawnable> credits = new();
            // This just prevents from enumerating it many times
            var dataSpawnables = obstacles as T[] ?? obstacles.ToArray();
            for (int i = 1; i < dataSpawnables.Count() - 1; i++) {
                float obstacleStart = dataSpawnables.ElementAt(i).GetDistance();
                float nextObstacle = dataSpawnables.ElementAt(i + 1).GetDistance();

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

        private static FloatSpawnable[] GetEnemies(int level, LevelModifiers modifiers) {
            if (level == 1) {
                return Array.Empty<FloatSpawnable>();
            }

            int numEnemies = level;
            if (modifiers.HasFlag(LevelModifiers.MoreEnemies)) {
                numEnemies = (int)(numEnemies * 1.5f);
            }

            return CreateEvenlySpacedArray(100, 100, numEnemies);
        }

        private static FloatSpawnable[] CreateEvenlySpacedArray(float startAt, float distance, int number) {
            FloatSpawnable[] array = new FloatSpawnable[number];
            for (int i = 0; i < array.Length; i++) {
                array[i] = startAt + distance * i;
            }

            return array;
        }

        private static float Ran01 => Random.Range(0.0f, 1.0f);
    }

    public class FloatSpawnable : IDataSpawnable {
        public static implicit operator FloatSpawnable(float f) => new FloatSpawnable(f);
        
        public float Value { get; private set; }

        public FloatSpawnable(float f) {
            Value = f;
        }
        
        public float GetDistance() {
            return Value;
        }
    }

    public class ObstacleSpawnable : FloatSpawnable {
        public bool IsFinishLine { get; private set; }

        public ObstacleSpawnable(float dis, bool isFinishLine) : base(dis) {
            IsFinishLine = isFinishLine;
        }
    }
}