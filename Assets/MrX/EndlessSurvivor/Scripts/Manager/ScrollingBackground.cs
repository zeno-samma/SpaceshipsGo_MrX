using UnityEngine;

namespace MrX.EndlessSurvivor
{
    public class ScrollingBackground : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed = 1f; // Tốc độ di chuyển của background
        private float backgroundWidth;
        private void OnEnable()
        {
            // Đăng ký lắng nghe sự thay đổi trạng thái từ GameManager
            EventBus.Subscribe<StateUpdatedEvent>(CombatState);//Lắng nghe trạng thái game do gamemanager quản lý
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<StateUpdatedEvent>(CombatState);
        }
        private void CombatState(StateUpdatedEvent Value)
        {
            if (Value.CurState == GameManager.GameState.COMBAT || Value.CurState == GameManager.GameState.GAMEOVER)
            {
                scrollSpeed = 0f;
            }
            else
            {
                scrollSpeed = 1f;
            }
        }
        void Start()
        {
            // Lấy chiều rộng của sprite
            backgroundWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        void Update()
        {
            // Di chuyển background sang trái
            transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

            // Nếu background đã di chuyển ra khỏi màn hình một khoảng bằng chiều rộng của nó
            if (transform.position.x < -backgroundWidth)
            {
                // Dịch chuyển nó về phía trước một khoảng bằng 2 lần chiều rộng
                // để nó nằm ngay sau background kia
                transform.position += new Vector3(backgroundWidth * 2f, 0, 0);
            }
        }
    }

}

