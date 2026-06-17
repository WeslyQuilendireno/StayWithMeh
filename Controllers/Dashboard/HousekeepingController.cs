using Microsoft.AspNetCore.Mvc;
using StayWithMeh.Models;
using Supabase;

namespace StayWithMeh.Controllers
{
    public class HousekeepingController : Controller
    {
        private readonly Client _supabase;

        public HousekeepingController(Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"]            = "Housekeeping Dashboard";
            ViewData["ActiveStaffPage"]  = "Housekeeping";
            ViewData["StaffRoleContext"] = "Housekeeping";
            ViewData["StaffName"]        = "Elena R.";
            ViewData["StaffRole"]        = "Floor 4 Lead";
            ViewData["StaffInitials"]    = "ER";

            var roomsResult = await _supabase.From<Room>().Get();
            var tasksResult = await _supabase.From<HousekeepingTask>().Get();

            var model = new HousekeepingDashboardViewModel
            {
                Rooms = roomsResult.Models,
                Tasks = tasksResult.Models
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoomStatus([FromBody] RoomStatusUpdateRequest request)
        {
            if (string.IsNullOrEmpty(request.RoomId) || string.IsNullOrEmpty(request.Status))
                return BadRequest("RoomId and Status are required.");

            var validStatuses = new[] { "available", "occupied", "dirty", "in_progress", "maintenance" };
            if (!validStatuses.Contains(request.Status))
                return BadRequest("Invalid status value.");

            var result = await _supabase.From<Room>()
                .Where(r => r.Id == request.RoomId)
                .Get();

            var room = result.Models.FirstOrDefault();
            if (room is null) return NotFound("Room not found.");

            room.Status = request.Status;
            await _supabase.From<Room>().Update(room);

            return Json(new { success = true, roomId = request.RoomId, status = request.Status });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTaskStatus([FromBody] TaskStatusUpdateRequest request)
        {
            if (string.IsNullOrEmpty(request.TaskId) || string.IsNullOrEmpty(request.Status))
                return BadRequest("TaskId and Status are required.");

            var validStatuses = new[] { "pending", "in_progress", "completed" };
            if (!validStatuses.Contains(request.Status))
                return BadRequest("Invalid status value.");

            var result = await _supabase.From<HousekeepingTask>()
                .Where(t => t.Id == request.TaskId)
                .Get();

            var task = result.Models.FirstOrDefault();
            if (task is null) return NotFound("Task not found.");

            task.Status = request.Status;
            await _supabase.From<HousekeepingTask>().Update(task);

            return Json(new { success = true, taskId = request.TaskId, status = request.Status });
        }
    }
}
