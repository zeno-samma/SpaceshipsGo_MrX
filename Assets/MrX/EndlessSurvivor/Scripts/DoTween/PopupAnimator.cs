using UnityEngine;
using DG.Tweening; // QUAN TRỌNG: Đừng bao giờ quên dòng này!
using UnityEngine.UI; // Để dùng được Button

namespace MrX.EndlessSurvivor
{
    public class PopupAnimator : MonoBehaviour
    {
        [Header("UI Elements")]
        public RectTransform panelRectTransform; // Kéo object MyPopupPanel vào đây
        public Button[] buttons; // Kéo 3 nút bấm của bạn vào đây

        [Header("Animation Settings")]
        public float animationDuration = 0.5f;
        public Ease panelEase = Ease.OutBack; // Hiệu ứng "vượt đà" cho panel
        public Ease buttonEase = Ease.OutBack; // Hiệu ứng cho nút

        private Vector2 panelStartPosition = new Vector2(0, -1500); // Vị trí ẩn nấp (dưới màn hình)
        private Vector2 panelEndPosition = Vector2.zero; // Vị trí ở giữa màn hình

        private Sequence popupSequence; // Biến để lưu trữ chuỗi animation

        void Start()
        {
            // Đảm bảo panel ẩn đi khi game bắt đầu
            panelRectTransform.anchoredPosition = panelStartPosition;
            foreach (var btn in buttons)
            {
                btn.transform.localScale = Vector3.zero;
            }
        }

        // Hàm để hiển thị popup
        public void ShowPopup()
        {
            // ⭐️ Kỹ năng quan trọng: Luôn KILL tween/sequence cũ trước khi chạy cái mới
            if (popupSequence != null && popupSequence.IsActive())
            {
                popupSequence.Kill();
            }

            // --- Bắt đầu biên đạo ---
            popupSequence = DOTween.Sequence();

            // 1. Panel trượt vào từ dưới lên
            popupSequence.Append(panelRectTransform.DOAnchorPos(panelEndPosition, animationDuration).SetEase(panelEase));

            // 2. Các nút bấm tuần tự "nảy" ra
            foreach (var btn in buttons)
            {
                // Nối tiếp animation của nút vào chuỗi
                // Mỗi nút sẽ phóng to ra sau khi nút trước đó hoàn thành một phần
                popupSequence.Append(btn.transform.DOScale(1f, animationDuration / 2).SetEase(buttonEase));
            }

            // Thêm hiệu ứng "punch" khi click vào nút
            foreach (var btn in buttons)
            {
                btn.onClick.AddListener(() =>
                {
                    btn.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);
                });
            }
        }

        // Hàm để ẩn popup
        public void HidePopup()
        {
            if (popupSequence != null && popupSequence.IsActive())
            {
                popupSequence.Kill();
            }

            popupSequence = DOTween.Sequence();

            // Lần này chúng ta làm ngược lại và dùng Join để tất cả diễn ra CÙNG LÚC
            popupSequence.Join(panelRectTransform.DOAnchorPos(panelStartPosition, animationDuration).SetEase(Ease.InBack));
            foreach (var btn in buttons)
            {
                popupSequence.Join(btn.transform.DOScale(0f, animationDuration / 2));
            }
        }
    }
}
