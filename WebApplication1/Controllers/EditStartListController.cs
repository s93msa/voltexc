using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using VoltigeCore.Business.Logic.Contest;
using VoltigeCore.Models;

namespace VoltigeCore.Controllers
{
    public class EditStartListController : Controller
    {
        public IActionResult Index()
        {
            var contest = ContestService.GetContestInstance();
            var horseOptions = ContestService.GetHorses()
                .OrderBy(h => h.HorseName)
                .Select(h => new SelectListItem
                {
                    Value = h.HorseId.ToString(),
                    Text = h.Lunger != null
                        ? $"{h.HorseName} ({h.Lunger.LungerName})"
                        : h.HorseName
                })
                .ToList();
            ViewBag.HorseOptions = horseOptions;
            return View(contest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateHorse(int horseOrderId, int horseId)
        {
            var horseOrder = ContestService.GetHorseOrder(horseOrderId);
            if (horseOrder != null)
            {
                horseOrder.HorseId = horseId;
                ContestService.UpdateHorseOrder(horseOrder);
                ContestService.GetNewDataFromDatabase();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MoveVaulter(
            int vaulterOrderId,
            int targetHorseOrderId,
            int[] targetVaulterOrderIds,
            int[] targetStartOrders,
            int[] sourceVaulterOrderIds,
            int[] sourceStartOrders)
        {
            // Update vaulters on the target HorseOrder (includes the moved vaulter)
            if (targetVaulterOrderIds != null && targetStartOrders != null && targetVaulterOrderIds.Length == targetStartOrders.Length)
            {
                for (int i = 0; i < targetVaulterOrderIds.Length; i++)
                {
                    var vo = ContestService.GetVaulterOrder(targetVaulterOrderIds[i]);
                    if (vo == null) continue;
                    vo.StartOrder = targetStartOrders[i];
                    if (vo.VaulterOrderID == vaulterOrderId)
                        vo.HorseOrderId = targetHorseOrderId;
                    ContestService.UpdateVaulterOrder(vo);
                }
            }

            // Update remaining vaulters on the source HorseOrder
            if (sourceVaulterOrderIds != null && sourceStartOrders != null && sourceVaulterOrderIds.Length == sourceStartOrders.Length)
            {
                for (int i = 0; i < sourceVaulterOrderIds.Length; i++)
                {
                    var vo = ContestService.GetVaulterOrder(sourceVaulterOrderIds[i]);
                    if (vo == null) continue;
                    vo.StartOrder = sourceStartOrders[i];
                    ContestService.UpdateVaulterOrder(vo);
                }
            }

            ContestService.GetNewDataFromDatabase();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SplitAndMoveVaulter(int vaulterOrderId, int newHorseId)
        {
            var movingVo = ContestService.GetVaulterOrder(vaulterOrderId);
            if (movingVo == null) return RedirectToAction("Index");

            var sourceHo = ContestService.GetHorseOrder(movingVo.HorseOrderId ?? 0);
            if (sourceHo == null) return RedirectToAction("Index");

            var activeVaulters = sourceHo.GetActiveVaulters().OrderBy(v => v.StartOrder).ToList();
            var movingStartOrder = movingVo.StartOrder;
            var beforeVaulters = activeVaulters.Where(v => v.StartOrder < movingStartOrder).ToList();
            var afterVaulters = activeVaulters.Where(v => v.StartOrder > movingStartOrder).ToList();

            // Create new HorseOrder for the moving vaulter with the chosen horse
            var nho1Id = ContestService.AddHorseOrder(new HorseOrder
            {
                HorseId = newHorseId,
                IsTeam = false,
                IsActive = true,
                StartListClassStepId = sourceHo.StartListClassStepId,
                StartNumber = 0
            });

            // If there are vaulters after the mover, they need their own new HorseOrder
            // (same original horse — the original HorseOrder is split)
            int? nho2Id = null;
            if (afterVaulters.Count > 0)
            {
                nho2Id = ContestService.AddHorseOrder(new HorseOrder
                {
                    HorseId = sourceHo.HorseId,
                    IsTeam = false,
                    IsActive = true,
                    StartListClassStepId = sourceHo.StartListClassStepId,
                    StartNumber = 0
                });
            }

            // Batch-update all affected VaulterOrders in one trip
            var voUpdates = new List<VaulterOrder>();

            movingVo.HorseOrderId = nho1Id;
            movingVo.StartOrder = 1;
            voUpdates.Add(movingVo);

            for (int i = 0; i < afterVaulters.Count; i++)
            {
                afterVaulters[i].HorseOrderId = nho2Id!.Value;
                afterVaulters[i].StartOrder = i + 1;
                voUpdates.Add(afterVaulters[i]);
            }

            // Close the gap in the before-group start orders
            for (int i = 0; i < beforeVaulters.Count; i++)
            {
                beforeVaulters[i].StartOrder = i + 1;
                voUpdates.Add(beforeVaulters[i]);
            }

            ContestService.UpdateVaulterOrders(voUpdates.ToArray());

            // Deactivate OHO if it no longer has any vaulters
            if (beforeVaulters.Count == 0)
            {
                var oho = ContestService.GetHorseOrder(sourceHo.HorseOrderId);
                if (oho != null)
                {
                    oho.IsActive = false;
                    ContestService.UpdateHorseOrder(oho);
                }
            }

            RenumberHorseOrdersInStep(sourceHo.HorseOrderId, sourceHo.StartListClassStepId ?? 0, nho1Id, nho2Id);

            ContestService.GetNewDataFromDatabase();
            return RedirectToAction("Index");
        }

        private static void RenumberHorseOrdersInStep(int sourceHoId, int stepId, int nho1Id, int? nho2Id)
        {
            // Reload from DB — includes the newly created HOs (StartNumber=0) and deactivated OHO
            var allHos = ContestService.GetHorseOrders(stepId).OrderBy(ho => ho.StartNumber).ToList();

            var nho1 = allHos.FirstOrDefault(h => h.HorseOrderId == nho1Id);
            var nho2 = nho2Id.HasValue ? allHos.FirstOrDefault(h => h.HorseOrderId == nho2Id.Value) : null;

            var ordered = new List<HorseOrder>();
            foreach (var ho in allHos)
            {
                // NHO1 and NHO2 will be inserted at the right place below — skip them in the sweep
                if (ho.HorseOrderId == nho1Id || (nho2Id.HasValue && ho.HorseOrderId == nho2Id.Value))
                    continue;

                if (ho.HorseOrderId == sourceHoId)
                {
                    if (ho.IsActive) ordered.Add(ho);  // OHO stays if it still has before-vaulters
                    if (nho1 != null) ordered.Add(nho1);
                    if (nho2 != null) ordered.Add(nho2);
                }
                else if (ho.IsActive)
                {
                    ordered.Add(ho);
                }
            }

            for (int i = 0; i < ordered.Count; i++)
                ordered[i].StartNumber = i + 1;

            ContestService.UpdateHorseOrder(ordered.ToArray());
        }
    }
}
