namespace Scripts.Core.Level {
    public class LevelData {
        public float StartingSpeed;
        public float EndingSpeed;
        
        public float[] Obstacles;
        public float[] Credits;
        public float[] Enemies;

        public override string ToString() {
            return $"Level: Starting Speed: {StartingSpeed}\nEnding Speed: {EndingSpeed}\nObstacle Count:{Obstacles.Length}";
        }
    }
}