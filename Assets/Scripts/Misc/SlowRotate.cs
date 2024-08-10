using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Misc{
    public class SlowRotate : MonoBehaviour {
        [SerializeField] private Vector3 _rotationAmount;
        private void Update() {
            float factor = Mathf.Sin(Time.time);
            transform.Rotate(_rotationAmount * (factor * Time.deltaTime));
        }
    }
}