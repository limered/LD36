using System.Linq;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.GameComponents
{
    public class WristBehaviour : MonoBehaviour
    {
        private readonly IntReactiveProperty _isHolding = new IntReactiveProperty(0);
        private Collider _collider;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            var fingerTips = GetComponentsInChildren<FingertipBehaviour>();
            foreach (var value in fingerTips.Select(ft => ft.HasObject))
            {
                value.Skip(1)
                    .Subscribe(next => _isHolding.Value = next ? _isHolding.Value + 1 : _isHolding.Value - 1)
                    .AddTo(this);
            }

            _isHolding
                .Subscribe(i => _collider.enabled = i <= 0)
                .AddTo(this);
        }
    }
}