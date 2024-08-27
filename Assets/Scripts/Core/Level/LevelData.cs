namespace Scripts.Core.Level {
    public class LevelData {
        public float StartingSpeed;
        public float EndingSpeed;
        
        public ObstacleSpawnable[] Obstacles;
        public FloatSpawnable[] Credits;
        public FloatSpawnable[] Enemies;

        public override string ToString() {
            return $"Level: Starting Speed: {StartingSpeed}\nEnding Speed: {EndingSpeed}\nObstacle Count:{Obstacles.Length}";
        }
    }
}