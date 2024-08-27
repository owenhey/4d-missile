namespace Scripts.Core.Level {
    public interface IDataSpawnable {
        public float GetDistance();
    }
    
    public class SpawnAfterDistance<T> where T : IDataSpawnable{
        private System.Action<T> _callback;
        private T[] _values;

        private float _totalDistanceTraveled;
        private int _index;
        
        public SpawnAfterDistance(T[] values, System.Action<T> callback) {
            _totalDistanceTraveled = 0;
            _index = 0;
            _values = values;
            _callback = callback;
        }

        public void Advance(float distance) {
            if (_index >= _values.Length) return;
            
            _totalDistanceTraveled += distance;
            
            float nextAt = _values[_index].GetDistance();
            bool traveledEnoughForNext = _totalDistanceTraveled > nextAt;
            if (traveledEnoughForNext) {
                _callback?.Invoke(_values[_index]);
                _index++;
            }
        }
    }
}