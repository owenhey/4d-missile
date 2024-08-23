using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Core.Obstacles{
    public class ObstacleCollider : MonoBehaviour
    {
        [SerializeField] private ObstacleBehavior _parentBehavior;

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                _parentBehavior.CollideWithPlayer();
            }
        }
        
#if UNITY_EDITOR
        private void OnValidate() {
            if (_parentBehavior == null) {
                ObstacleBehavior parentBehav = GetComponentInParent<ObstacleBehavior>();
                if (parentBehav != null) {
                    _parentBehavior = parentBehav;
                    UnityEditor.EditorUtility.SetDirty(gameObject);
                }
            }
        }
#endif
    }
}