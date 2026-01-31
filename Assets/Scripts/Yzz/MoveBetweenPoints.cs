using UnityEngine;
using System.Collections.Generic;

namespace Yzz
{
    public class MoveBetweenPoints : MonoBehaviour
    {
        [Header("Path Points")]
        [SerializeField] private Transform pointA;
        [SerializeField] private Transform pointB;

        [Header("Movement")]
        [SerializeField] private float speed = 2f;
        [SerializeField] private bool linear = true;

        private float _t;
        private float _duration;
        private float _direction = 1f;
        
        // 用于摩擦力/带动效果的变量
        private Vector3 _lastPosition;
        private Vector3 _platformVelocity;
        private List<Rigidbody2D> _trackedPlayers = new List<Rigidbody2D>();

        private void Start()
        {
            _lastPosition = transform.position;
            RefreshDuration();
        }

        private void RefreshDuration()
        {
            if (pointA != null && pointB != null && speed > 0f)
            {
                // 只计算 X 轴距离，保持原逻辑
                float dist = Mathf.Abs(pointA.position.x - pointB.position.x);
                _duration = dist / speed;
            }
            else { _duration = 1f; }
        }

        private void FixedUpdate() // 物理移动建议放在 FixedUpdate
        {
            if (pointA == null || pointB == null || _duration <= 0f) return;

            // 1. 计算移动
            _t += _direction * (Time.fixedDeltaTime / _duration);
            if (_t >= 1f) { _t = 1f; _direction = -1f; }
            if (_t <= 0f) { _t = 0f; _direction = 1f; }

            float s = linear ? _t : Mathf.SmoothStep(0f, 1f, _t);
            Vector3 targetPos = Vector3.Lerp(
                new Vector3(pointA.position.x, transform.position.y, transform.position.z), 
                new Vector3(pointB.position.x, transform.position.y, transform.position.z), 
                s
            );

            // 2. 计算本帧位移向量
            _platformVelocity = (targetPos - transform.position);

            // 3. 实际移动平台
            transform.position = targetPos;

            // 4. 关键：同步带动上方的玩家
            foreach (var rb in _trackedPlayers)
            {
                if (rb != null)
                {
                    // 将平台的位移直接加到玩家的 position 上
                    // 这样即使玩家在做 velocity 运算，position 也会被同步修正
                    rb.position += (Vector2)_platformVelocity;
                }
            }
        }

        // --- 碰撞检测逻辑 ---

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // 判断碰撞点是否在平台上方（避免挂在侧面也被带动）
            if (collision.contactCount > 0 && collision.contacts[0].normal.y < -0.5f)
            {
                Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
                if (rb != null && !_trackedPlayers.Contains(rb))
                {
                    _trackedPlayers.Add(rb);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                _trackedPlayers.Remove(rb);
            }
        }
    }
}