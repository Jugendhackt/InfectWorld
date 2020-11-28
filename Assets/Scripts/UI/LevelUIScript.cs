using UnityEngine;

public class LevelUIScript : MonoBehaviour
{
        public GameObject patientView;
        
        public void ChangePatientView()
        {
                patientView.SetActive(true);    
        }
}