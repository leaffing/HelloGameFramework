using System.Collections;
using Coffee.UIExtensions;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Leaf.CustomUI
{
    [AddComponentMenu("UI/UIEffect/CustomUIEffect", 200)]
    public class CustomUIEffect : MonoBehaviour
    {
        public enum EffectType
        {
            Position,
            Scale,
            Rotation,
            Fade,
            NumberRoll,
            CutOff,
        }

        public EffectType effectType = EffectType.Scale;

        public float startTime = 0, duration = 1;

        public Ease effectEase = Ease.Linear;

        public Vector3 targetVectorValue;

        public int targetIntValue;

        public float targetFloatValue;

        private Vector3 tempVector;

        private float tempFloat;

        // Use this for initialization
        void OnEnable()
        {
            if(effectType != EffectType.NumberRoll)
                StartCoroutine(ApplyNormalEffect());
            else
            {
                _text = GetComponent<Text>();
                if(_text == null)
                    Debug.LogError("NumberRoll effect need Text component!");
                else                
                    StartCoroutine(ApplyNumberRollEffect(_text));
            }
        }

        void OnDisable()
        {
            switch (effectType)
            {
                case EffectType.Position:
                    transform.localPosition = tempVector;
                    break;
                case EffectType.Scale:
                    transform.localScale = tempVector;
                    break;
                case EffectType.Rotation:
                    transform.localEulerAngles = tempVector;
                    break;
                case EffectType.Fade:
                    var group = GetComponent<CanvasGroup>();
                    if (group == null)
                    {
                        Debug.LogError("Fade effect need CanvasGroup component");
                        break;
                    }
                    group.alpha = tempFloat;                    
                    break;
                case EffectType.CutOff:
                    var cut = GetComponent<UITransitionEffect>();
                    if (cut == null)
                    {
                        Debug.LogError("Fade effect need CanvasGroup component");
                        break;
                    }
                    cut.effectFactor = tempFloat; 
                    break;
                default:
                    break;
            }
        }

        private Text _text;
        private CanvasGroup _group;
        private UITransitionEffect _transition;
        IEnumerator ApplyNormalEffect()
        {
            yield return new WaitForSeconds(startTime);
            DoNormalEffect();
        }

        private void DoNormalEffect()
        {
            switch (effectType)
            {
                case EffectType.Position:
                    tempVector = transform.localPosition;
                    transform.DOLocalMove(targetVectorValue, duration).SetEase(effectEase);
                    break;
                case EffectType.Scale:
                    tempVector = transform.localScale;
                    transform.DOScale(targetVectorValue, duration).SetEase(effectEase);
                    break;
                case EffectType.Rotation:
                    tempVector = transform.localEulerAngles;
                    transform.DOLocalRotate(targetVectorValue, duration).SetEase(effectEase);
                    break;
                case EffectType.Fade:
                    _group = GetComponent<CanvasGroup>();
                    if (_group == null)
                    {
                        Debug.LogError("Fade effect need CanvasGroup component");
                        break;
                    }
                    tempFloat = _group.alpha;
                    _group.DOFade(targetFloatValue, duration).SetEase(effectEase);
                    break;
                case EffectType.CutOff:
                    _transition = GetComponent<UITransitionEffect>();
                    if (_transition == null)
                    {
                        Debug.LogError("Fade effect need CanvasGroup component");
                        break;
                    }
                    tempFloat = _transition.effectFactor; 
                    DOTween.To(() => _transition.effectFactor, x => _transition.effectFactor = x, targetFloatValue, duration).SetEase(effectEase);
                    break;
                default:
                    break;
            }
        }

        IEnumerator ApplyNumberRollEffect(Text text)
        {
            yield return new WaitForSeconds(startTime);
            float time = 0;
            while (time < duration)
            {
                text.text = targetIntValue < 10 ? Random.Range(0, 9).ToString() : Random.Range(0, targetIntValue).ToString(); 
                yield return new WaitForSeconds(0.025f);
                time += 0.025f;
            }
            text.text = targetIntValue.ToString();
        }
    }
}