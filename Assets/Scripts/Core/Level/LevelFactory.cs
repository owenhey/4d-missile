using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Scripts.Misc;
using Scripts.Utils;
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

        private static string GetString(LevelModifiers l) {
            switch (l) {
                case LevelModifiers.MoreEnemies: return "More Enemies";
                case LevelModifiers.HarderEnemies: return "Stronger Enemies";
                case LevelModifiers.Longer: return "Longer";
                case LevelModifiers.Faster: return "Faster";
                default:
                    throw new ArgumentOutOfRangeException(nameof(l), l, null);
            }
        }
        
        public static LevelData GenerateLevel(int level) {
            var modifiers = GenerateModifiers(level);
            Debug.Log("mods: " + modifiers);
            var speed = GetSpeeds(level, modifiers);
            float averageSpeed = (speed.startSpeed + speed.endSpeed) * .5f; 
            var obstacles = GetObstacles(level, averageSpeed, modifiers);
            float totalDistance = obstacles[^1].GetDistance();
            var credits = GetCredits(obstacles);
            var enemies = GetEnemies(level, totalDistance, modifiers);
                
            LevelData levelData = new() {
                StartingSpeed = speed.startSpeed,
                EndingSpeed = speed.endSpeed,
                Obstacles = obstacles,
                Credits = credits,
                Enemies = enemies,
                Modifiers = TurnModifiersIntoStrings(modifiers)
            };
            Debug.Log(levelData);
            return levelData;
        }

        private static LevelModifiers GenerateModifiers(int level) {
            int numModifiers = 0;
            if (level >= 3) {
                numModifiers = (level - 3) / 3 + 1;
            }
            numModifiers = Mathf.Min(numModifiers, 4);
            Debug.Log($"Num modifiers: {numModifiers}");
            
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
            int startSpeed = (int)((level - 1) * 4.5f) + 20; // Goes from 20 to 65
            int endSpeed = (int)(startSpeed * 1.4f); 

            bool hasFasterModifier = modifiers.HasFlag(LevelModifiers.Faster);
            float factor = hasFasterModifier ? 1.3f : 1.0f;
            return (startSpeed, (int)(endSpeed * factor));
        }

        private static ObstacleSpawnable[] GetObstacles(int level, float averageSpeed, LevelModifiers modifiers) {
            // return new ObstacleSpawnable[] { new ObstacleSpawnable(30, true) };
            
            bool hasLongerModifier = modifiers.HasFlag(LevelModifiers.Longer);
            float totalDistance = averageSpeed * (hasLongerModifier ? 45 : 60);
            int spaceBetween = 40 + level;
            int totalObstacles = (int)(totalDistance / spaceBetween);
            FloatSpawnable[] array = CreateEvenlySpacedArray(level * 10, spaceBetween, totalObstacles);

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

        private static EnemySpawnable[] GetEnemies(int level, float totalDistance, LevelModifiers modifiers) {
            if (level == 1) {
                return Array.Empty<EnemySpawnable>();
            }

            float remappedLevel = Helpers.RemapNoClamp(level, 2, 12, 1, 3);
                
                
            int numEnemies = (int)(5 * remappedLevel);
            bool hasMoreEnemies = modifiers.HasFlag(LevelModifiers.MoreEnemies);
            if (hasMoreEnemies) {
                numEnemies = (int)(numEnemies * 1.5f);
            }

            const float startCap = 100;
            const float endCap = 200;

            float totalValidSpawnSpace = totalDistance - (startCap + endCap);
            float spaceBetweenEnemies =  totalValidSpawnSpace / (numEnemies + 1);
            
            var floatArray = CreateEvenlySpacedArray(100, spaceBetweenEnemies, numEnemies).ToList();
            // Just put 1 as everything to start
            List<EnemySpawnable> enemyList = floatArray.Select(x => new EnemySpawnable(x.GetDistance(), 1)).ToList();
            
            // Calculate a few places to spawn in more enemies (2 or 3 enemies at once)
            int numDoubleEnemies = 0;
            if (level > 3) {
                numDoubleEnemies = hasMoreEnemies ? 3 : 2;
            }
            if (level > 7) {
                numDoubleEnemies = hasMoreEnemies ? 5 : 4;
            }

            int numTripleEnemies = 0;
            if (level > 8) {
                numTripleEnemies = hasMoreEnemies ? level - 7 : (level - 7) / 2;
            }

            // Insert the double enemies
            int randomStartIndex = Random.Range(0, enemyList.Count);
            for (int i = 0; i < numDoubleEnemies; i++) {
                int insertionIndex = (randomStartIndex + (i * 2)) % enemyList.Count;
                enemyList[insertionIndex] = new EnemySpawnable(enemyList[insertionIndex].GetDistance(), 2);
            }
            
            // Insert the triple enemies
            randomStartIndex = Random.Range(0, enemyList.Count);
            for (int i = 0; i < numTripleEnemies; i++) {
                int insertionIndex = (randomStartIndex + (i * 2)) % enemyList.Count;
                enemyList[insertionIndex] = new EnemySpawnable(enemyList[insertionIndex].GetDistance(), 2);
            }
            
            return enemyList.ToArray();
        }

        private static FloatSpawnable[] CreateEvenlySpacedArray(float startAt, float distance, int number) {
            FloatSpawnable[] array = new FloatSpawnable[number];
            for (int i = 0; i < array.Length; i++) {
                array[i] = startAt + distance * i;
            }

            return array;
        }

        private static float Ran01 => Random.Range(0.0f, 1.0f);

        private static List<string> TurnModifiersIntoStrings(LevelModifiers mods) {
            var retList = new List<string>();
            
            var allMods = Enum.GetValues(typeof(LevelModifiers)).Cast<LevelModifiers>().ToArray();
            foreach (var mod in allMods) {
                if(mods.HasFlag(mod))
                    retList.Add(GetString(mod));
            }

            return retList;
        }
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

    public class EnemySpawnable : FloatSpawnable {
        public int NumberToSpawn;

        public EnemySpawnable(float dis, int num) : base(dis) {
            NumberToSpawn = num;
        }
    }

    public class ObstacleSpawnable : FloatSpawnable {
        public bool IsFinishLine { get; private set; }

        public ObstacleSpawnable(float dis, bool isFinishLine) : base(dis) {
            IsFinishLine = isFinishLine;
        }
    }
}