using Assets.Scripts.EventPublishers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EffectsFactory2D : MonoBehaviour
{
    [SerializeField] StundEffectPresenter _2dEffectPrefub;
    [SerializeField] Canvas _canvas;

    private List<StundEffectPresenter> _hidePull = new();
    private List<StundEffectPresenter> _showPull = new();
    public Vector3 _offset = new Vector3(0f, 1.5f, 0f);

    public void Awake()
    {
        StunEventPublisher.Instance.CharacterStuned += ShowStunEffect;
        StunEventPublisher.Instance.StunStateExited += HideStunEffect;
    }

    public void PublicUpdate()
    {
        foreach (var effect in _showPull)
        {
            if (effect.StunedCharacter == null)
            {
                _showPull.Remove(effect);
                _hidePull.Add(effect);
            }
            else
            {
                effect.UpdatePosition();
            }
        }
    }

    private void ShowStunEffect(object sender, CharacterStunedEventArgs e)
    {
        StundEffectPresenter effect;

        if (_hidePull.Any())
        {
            effect = _hidePull.First();
        }
        else
        {
            var effectObject = Instantiate(_2dEffectPrefub.gameObject);
            effect = effectObject.GetComponent<StundEffectPresenter>();
            effect.transform.SetParent(_canvas.transform);
        }

        effect.StunedCharacter = e.Character;
        effect.gameObject.SetActive(true);
        effect.UpdatePosition();
        _showPull.Add(effect);
    }

    private Rect GetScreenBounds(GameObject gameObject)
    {
        var meshFilter = gameObject.GetComponent<MeshFilter>();

        if (meshFilter == null || meshFilter.sharedMesh == null)
            throw new System.Exception("Mesh not found");

        Vector3[] vertices = meshFilter.sharedMesh.vertices;
        Vector3 min = Vector3.one * float.MaxValue;
        Vector3 max = Vector3.one * float.MinValue;

        foreach (Vector3 vertex in vertices)
        {
            Vector3 worldPos = transform.TransformPoint(vertex);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

            min = Vector3.Min(min, screenPos);
            max = Vector3.Max(max, screenPos);
        }

        return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    }

    private void HideStunEffect(object sender, CharacterStunedEventArgs e)
    {
        var effect = _showPull.FirstOrDefault(x => x.StunedCharacter == e.Character);
        effect.gameObject.SetActive(false);

        _showPull.Remove(effect);
        _hidePull.Add(effect);
    }

    public void OnDestroy()
    {
        StunEventPublisher.Instance.CharacterStuned -= ShowStunEffect;
        StunEventPublisher.Instance.StunStateExited -= HideStunEffect;
    }
}
