using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UniRx;

namespace MrX.EndlessSurvivor
{
    public class PlayerHealth : MonoBehaviour
    {
        // === DỮ LIỆU ===
        public PlayerConfigSO playerConfig; //
        [SerializeField] private Image amountImage;
        // Dữ liệu động của người chơi
        private int healthLevel;
        // private float currentHealth;
        // =========Unirx
        // Vừa là biến lưu trữ, vừa là một dòng chảy sự kiện
        public ReactiveProperty<float> CurrentHealth { get; private set; }
        // =======================================
        // private float damageLevel;
        // private float cooldownLevel;
        // private int currentGold;

        // --- Các thuộc tính (Properties) để tính toán chỉ số cuối cùng ---
        public float MaxHealth => playerConfig.initialHealth + (playerConfig.healthBonusPerLevel * healthLevel);
        // public float CurrentDamage => playerConfig.initialDamage + (playerConfig.damageBonusPerLevel * damageLevel);
        // public float CurrentCooldown => playerConfig.initialCooldown - (playerConfig.cooldownReductionPerLevel * cooldownLevel);
        // Hàm này sẽ được GameManager gọi khi load game xong

        void Awake()
        {
            // Khởi tạo ReactiveProperty với giá trị ban đầu là maxHealth
            CurrentHealth = new ReactiveProperty<float>(MaxHealth);
        }
        public void ApplyLoadedData(PlayerData data)
        {
            healthLevel = data.healthUpgradeLevel;
            // damageLevel = data.damageUpgradeLevel;
            // cooldownLevel = data.cooldownUpgradeLevel;
            // currentGold = data.gold;

            // Debug.Log("Player data applied. Current Damage: " + CurrentDamage);
        }

        // Hàm này được GameManager gọi trước khi save game
        public PlayerData GetDataToSave()
        {
            PlayerData data = new PlayerData();
            data.healthUpgradeLevel = healthLevel;
            // data.damageUpgradeLevel = damageLevel;
            // data.cooldownUpgradeLevel = cooldownLevel;
            // data.gold = currentGold;
            return data;
        }

        // Ví dụ về việc nâng cấp
        public void UpgradeHealth()
        {
            // (Kiểm tra xem có đủ vàng không...)
            healthLevel++;
            // (Trừ vàng...)
            // << BÁO HIỆU CHO GAMEMANAGER >>
            // GameManager.Ins.SaveGame();//Dùng cho ít lần thay đổi và các thay đổi quan trọng(Qua một chương, hoàn thành được thành tựu)
            // GameManager.Ins.MarkDataAsDirty();//Dùng cho trường hợp nhặt liên tục 10 coins
        }
        void Start()
        {
            // Khởi tạo ReactiveProperty với giá trị ban đầu là maxHealth
            CurrentHealth = new ReactiveProperty<float>(MaxHealth);
            // currentHealth = MaxHealth;
        }

        public void TakeDamagePlayer(float damage)//Player nhận sát thương từ enemy
        {
            // Đảm bảo máu không âm
            if (CurrentHealth.Value < 0)
            {
                CurrentHealth.Value = 0;
            }
            // Kiểm tra nếu đã chết
            if (CurrentHealth.Value == 0)
            {
                // int coinBonus = UnityEngine.Random.Range(minCoinBonus, maxCoinBonus);
                Debug.Log("Phát event player chết");
                // gameObject.SetActive(false);
                EventBus.Publish(new PlayerDiedEvent { });
                return;
            }
            // Debug.Log("TakeDamage: " + damage);
            if (playerConfig.initialHealth <= 0) return; // Nếu đã chết rồi thì không nhận thêm sát thương
            // currentHealth -= damage;
            CurrentHealth.Value -= damage;
            // Phát đi sự kiện với dữ liệu là tỉ lệ máu
            // float healthPercentage = currentHealth / MaxHealth;
            // EventBus.Publish(new PlayerHealthChangedEvent { NewHealthPercentage = healthPercentage });
            // Debug.Log($"currentHealth{currentHealth}");
        }
    }
}

