using UnityEngine;
using UnityEngine.UI;

public class StundEffectPresenter : MonoBehaviour
{
    public GameObject StunedCharacter { get; set; }

    private Image _stunEffectIamge;

    public void UpdatePosition()
    {
        if (StunedCharacter != null)
        {
            _stunEffectIamge.transform.position = Camera.main.WorldToScreenPoint(StunedCharacter.transform.position);
        }
    }
}
