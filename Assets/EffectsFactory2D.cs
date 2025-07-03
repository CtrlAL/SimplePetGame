using Assets.Scripts.EventPublishers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;

public class EffectsFactory2D : MonoBehaviour
{
    [SerializeField] GameObject _2dEffectPrefub;
    [SerializeField] Canvas _canvas;

    private List<GameObject> _hidePull = new List<GameObject>(10);
    private Dictionary<GameObject, GameObject> _showPull = new Dictionary<GameObject, GameObject>();

    public Vector3 _offset = new Vector3(0f, 1.5f, 0f);
    private int _spawnLimit = 20;

    public void Awake()
    {

        StunEventPublisher.Instance.CharacterStuned += ShowStunEffect;
        StunEventPublisher.Instance.StunStateExited += HideStunEffect;
    }

    private void ShowStunEffect(object sender, CharacterStunedEventArgs e)
    {
        GameObject effect;

        if (_hidePull.Any())
        {
            effect = _hidePull.First();
        }
        else
        {
            effect = Instantiate(_2dEffectPrefub);
            effect.transform.SetParent(_canvas.transform);
        }

        //var rect = GetScreenBounds(e.Character);
        var pos = Camera.main.WorldToScreenPoint(e.Character.transform.position);

        effect.transform.position = pos + _offset;
        effect.SetActive(true);
        _showPull.TryAdd(e.Character, effect);
    }

    //private Rect GetScreenBounds(GameObject gameObject)
    //{
    //    var meshFilter = gameObject.GetComponent<MeshFilter>();

    //    if (meshFilter == null || meshFilter.sharedMesh == null)
    //        throw new System.Exception("Mesh not found");

    //    Vector3[] vertices = meshFilter.sharedMesh.vertices;
    //    Vector3 min = Vector3.one * float.MaxValue;
    //    Vector3 max = Vector3.one * float.MinValue;

    //    foreach (Vector3 vertex in vertices)
    //    {
    //        Vector3 worldPos = transform.TransformPoint(vertex);
    //        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

    //        min = Vector3.Min(min, screenPos);
    //        max = Vector3.Max(max, screenPos);
    //    }

    //    return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    //}

    private void HideStunEffect(object sender, CharacterStunedEventArgs e)
    {
        var effect = _showPull[e.Character];
        _showPull.Remove(e.Character);
        effect.SetActive(false);
        _hidePull.Add(effect);
    }

    public void OnDestroy()
    {
        StunEventPublisher.Instance.CharacterStuned -= ShowStunEffect;
        StunEventPublisher.Instance.StunStateExited -= HideStunEffect;
    }
}
