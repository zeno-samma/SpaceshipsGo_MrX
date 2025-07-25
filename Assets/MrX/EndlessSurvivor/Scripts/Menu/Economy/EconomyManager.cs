using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;

namespace MrX.EndlessSurvivor
{
    public class EconomyManager : MonoBehaviour
    {
        // Hàm để lấy về tất cả số dư tiền tệ của người chơi
        public async Task<GetBalancesResult> GetPlayerBalancesAsync()
        {
            try
            {
                var options = new GetBalancesOptions { ItemsPerFetch = 100 };
                var result = await EconomyService.Instance.PlayerBalances.GetBalancesAsync(options);
                return result;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        // Hàm để lấy về tất cả vật phẩm trong túi đồ của người chơi
        public async Task<GetInventoryResult> GetPlayerInventoryAsync()
        {
            try
            {
                var options = new GetInventoryOptions { ItemsPerFetch = 100 };
                var result = await EconomyService.Instance.PlayerInventory.GetInventoryAsync(options);
                return result;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        // Hàm để thực hiện một giao dịch mua ảo
        // public async Task<MakeVirtualPurchaseResult> MakePurchaseAsync(string purchaseId)
        // {
        //     try
        //     {
        //         // "purchaseId" phải trùng với ID bạn đã tạo trên Dashboard (ví dụ: "BUY_IRON_HELMET")
        //         var result = await EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync(purchaseId);
        //         return result;
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.LogException(e);
        //         return null;
        //     }
        // }
        // ================================
        public async Task<MakeVirtualPurchaseResult> MakePurchaseAsync(string purchaseId)
        {
            try
            {
                return await EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync(purchaseId);
            }
            catch (EconomyException e)
            {
                Debug.LogWarning($"[Economy] Purchase failed with code {e.ErrorCode}: {e.Message}");
                throw; // Ném lại nguyên lỗi để ShopMenu xử lý đúng
            }
            catch (Exception e)
            {
                Debug.LogError($"[Economy] Unknown error during purchase: {e.Message}");
                throw; // Vẫn ném lại để xử lý riêng
            }
        }
        // =====================================
        // --- HÀM MỚI ĐỂ THÊM TIỀN ---
        public async Task GrantGoldAsync(int amount)
        {
            try
            {
                // Gọi hàm IncrementBalanceAsync của dịch vụ Economy
                // Dùng hằng số EconomyIds.GOLD_CURRENCY để đảm bảo chính xác
                await EconomyService.Instance.PlayerBalances.IncrementBalanceAsync(EconomyConst.DIAMOND, amount);
                // await EconomyService.Instance.PlayerBalances.DecrementBalanceAsync(EconomyConst.ID_GOLD_CURRENCY, amount);
                Debug.Log($"Successfully granted {amount} {EconomyConst.DIAMOND}");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}

