using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    [System.Serializable]
    public class EffectData
    {
        public string effectName;
        public GameObject effectPrefab;
        public float defaultDuration = 2f;
    }

    [Header("이펙트 목록")]
    [SerializeField] private List<EffectData> effectList = new List<EffectData>();

    private Dictionary<string, EffectData> effectDictionary = new Dictionary<string, EffectData>();         //이펙트를 이름으로 빠르게 찾기 위한 딕셔너리

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDictionary()
    {
        effectDictionary.Clear();
        foreach (var effect in effectList)
        {
            if (!effectDictionary.ContainsKey(effect.effectName))
            {
                effectDictionary.Add(effect.effectName, effect);

            }
            else
            {
                Debug.Log($"중복된 이펙트 이름 : {effect.effectName}");
            }
        }
    }

    public GameObject PlayEffect(string effectName, Vector3 position, Quaternion rotation)
    {
        if (effectDictionary.TryGetValue(effectName, out EffectData data))
        {
            GameObject effect = Instantiate(data.effectPrefab, position, rotation);
            Destroy(effect, data.defaultDuration);
            return effect;
        }
        else
        {
            Debug.LogWarning($"이펙트를 찾을 수 없습니다. : {effectName}");
            return null;
        }
    }
    public GameObject PlayEffect(string effectName, Vector3 position, Quaternion rotation, float duration)
    {
        if (effectDictionary.TryGetValue(effectName, out EffectData data))
        {
            GameObject effect = Instantiate(data.effectPrefab, position, rotation);
            Destroy(effect, data.defaultDuration);
            return effect;
        }
        else
        {
            Debug.LogWarning($"이펙트를 찾을 수 없습니다. : {effectName}");
            return null;
        }
    }


    public GameObject PlayEffect(string effectName, Vector3 position)
    {
        return PlayEffect(effectName, position, Quaternion.identity);
    }

    public GameObject PlayEffect(string effectName, Vector3 position, float duration)
    {
        return PlayEffect(effectName, position, Quaternion.identity, duration);
    }

    public void PlayEffectWithDelay(string effectName, Vector3 position, Quaternion roatation, float delay, float duration)
    {
        StartCoroutine(PlayEffectDelayed(effectName, position, roatation, delay, duration));
    }

    private IEnumerator PlayEffectDelayed(string effectName, Vector3 position, Quaternion rotation, float delay, float duartion)
    {
        yield return new WaitForSeconds(delay);

        if (duartion > 0)
        {
            PlayEffect(effectName, position, rotation, duartion);
        }
        else
        {
            PlayEffect(effectName, position, rotation);
        }
    }
}
