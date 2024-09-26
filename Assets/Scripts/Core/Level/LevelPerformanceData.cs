namespace Scripts.Core.Level {
    public class LevelPerformanceData {
        public int Level;
        public bool Finished;
        public int CreditsEarned;
        public int DamageTaken;

        public LevelPerformanceData(int level, bool finished, int creditsEarned, int damageTaken) {
            Level = level;
            Finished = finished;
            CreditsEarned = creditsEarned;
            DamageTaken = damageTaken;
        }
    }
}