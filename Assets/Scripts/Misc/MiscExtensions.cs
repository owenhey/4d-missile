using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Misc {
    public static class MiscExtensions {
        public static T GetRandom<T>(this List<T> list) {
            return list[Random.Range(0, list.Count)];
        }

        public static void EnsureEnoughElementsAndSetActive<T>(this List<T> list, int targetCount) where T : MonoBehaviour {
            for(int i = 0; i < Mathf.Max(targetCount, list.Count); i++) {
                if (list.Count < targetCount) {
                    T newMono = Object.Instantiate(list[0], list[0].transform.parent);
                    list.Add(newMono);
                }
                
                list[i].gameObject.SetActive(i < targetCount);
            }
        }
        
        public static void EnsureEnoughElements<T>(this List<T> list, int targetCount) where T : MonoBehaviour {
            for(int i = 0; i < Mathf.Max(targetCount, list.Count); i++) {
                if (list.Count < targetCount) {
                    T newMono = Object.Instantiate(list[0], list[0].transform.parent);
                    list.Add(newMono);
                    newMono.gameObject.SetActive(false);
                }
            }
        }
    }
}