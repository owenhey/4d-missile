using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Core.Obstacles {
    public class AfterPlayerCollider : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Obstacle")) {
                ObstacleBehavior obstacleBehavior = other.GetComponentInParent<ObstacleBehavior>();
                obstacleBehavior.OnPassedPlayer();
            }
        }
    }
}