using UnityEngine;
using System.Collections.Generic; // Cần để dùng List
using System.Linq; // Cần để dùng LINQ cho tiện

namespace MrX.EndlessSurvivor
{

    public class AutoAttackController : MonoBehaviour
    {
        private Transform currentTarget;
        private float findTargetTimer;
        private float findTargetInterval = 0.25f; // Tần suất quét tìm mục tiêu (4 lần/giây)

        // Tham chiếu đến các bộ phận khác của Player
        private WeaponManager weaponManager;
        // private WeaponAim weaponAim; // Giả sử bạn có script này để xoay vũ khí

        void Awake()
        {
            // Lấy các component cần thiết
            weaponManager = GetComponentInChildren<WeaponManager>();
            // weaponAim = GetComponentInChildren<WeaponAim>();
        }

        void Update()
        {
            // Script này chỉ hoạt động khi game đang ở trạng thái COMBAT
            // (Giả sử GameManager dùng UniRx, nếu không bạn có thể kiểm tra bằng cách khác)
            // if (GameManager.Ins.CurrentState.Value != GameManager.GameState.COMBAT) return;

            // Quét tìm mục tiêu theo một tần suất cố định để tối ưu
            findTargetTimer -= Time.deltaTime;
            if (findTargetTimer <= 0f)
            {
                FindClosestEnemy();
                findTargetTimer = findTargetInterval;
            }

            // Nếu đã có mục tiêu, thực hiện tấn công
            if (currentTarget != null)
            {
                // Ra lệnh cho các bộ phận khác hành động
                // weaponAim.AimAt(currentTarget);
                weaponManager.Shoot();
                Debug.Log("Tấn công");
            }
        }

        void FindClosestEnemy()
        {
            // Lấy danh sách địch từ "Tổng Chỉ Huy"
            List<Enemy> activeEnemies = EnemyManager.Ins.activeEnemies;

            if (activeEnemies.Count == 0)
            {
                currentTarget = null;
                return;
            }

            Enemy closestEnemy = null;
            float minDistance = float.MaxValue;

            // Duyệt qua danh sách để tìm con gần nhất
            foreach (Enemy enemy in activeEnemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemy;
                }
            }

            if (closestEnemy != null)
            {
                currentTarget = closestEnemy.transform;
            }
        }
    }
}