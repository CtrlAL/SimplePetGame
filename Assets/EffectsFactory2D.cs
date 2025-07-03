using Assets.Scripts.EventPublishers;
using System;
using UnityEngine;

public class EffectsFactory2D : MonoBehaviour
{
    [SerializeField] GameObject _2dEffectPrefub;
    [SerializeField] Canvas _canvas;

    public void Awake()
    {

        StunEventPublisher.Instance.CharacterStuned += ShowStunEffect;
        StunEventPublisher.Instance.StunStateExited += HideStunEffect;
    }

    private void ShowStunEffect(object sender, CharacterStunedEventArgs e)
    {
        _2dEffectPrefub.SetActive(true);
        _2dEffectPrefub.transform.SetParent(_canvas.transform);
        var screanPos = Camera.main.WorldToScreenPoint(e.Character.transform.position);
        _2dEffectPrefub.transform.position = screanPos;
    }

    private void HideStunEffect(object sender, CharacterStunedEventArgs e)
    {
        _2dEffectPrefub.SetActive(false);
    }

    public void OnDestroy()
    {
        StunEventPublisher.Instance.CharacterStuned -= ShowStunEffect;
        StunEventPublisher.Instance.StunStateExited -= HideStunEffect;
    }
}
