using System.Collections;
using UnityEngine;

public class ScreenShake2D : MonoBehaviour
{
    [SerializeField] private float defaultDuration = 0.2f;
    [SerializeField] private float defaultMagnitude = 0.25f;

    private Vector3 _originalLocalPos;
    private Coroutine _shakeRoutine;

    private void Awake()
    {
        _originalLocalPos = transform.localPosition;
    }

    public void Shake(float duration = -1f, float magnitude = -1f)
    {
        if (_shakeRoutine != null)
        {
            StopCoroutine(_shakeRoutine);
        }
        _shakeRoutine = StartCoroutine(ShakeRoutine(
            duration > 0f ? duration : defaultDuration,
            magnitude > 0f ? magnitude : defaultMagnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float timer = 0f;
        while (timer < duration)
        {
            var offset = Random.insideUnitCircle * magnitude;
            transform.localPosition = _originalLocalPos + new Vector3(offset.x, offset.y, 0f);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = _originalLocalPos;
        _shakeRoutine = null;
    }
}
