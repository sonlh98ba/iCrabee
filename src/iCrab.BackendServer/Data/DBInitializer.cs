using iCrabee.BackendServer.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iCrabee.BackendServer.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly string AdminRoleName = "Admin";
        private readonly string UserRoleName = "Member";

        public DbInitializer(ApplicationDbContext context,
          UserManager<User> userManager,
          RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            #region Quyền

            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Id = AdminRoleName,
                    Name = AdminRoleName,
                    NormalizedName = AdminRoleName.ToUpper(),
                });
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Id = UserRoleName,
                    Name = UserRoleName,
                    NormalizedName = UserRoleName.ToUpper(),
                });
            }

            #endregion Quyền

            #region Người dùng

            if (!_userManager.Users.Any())
            {
                var result = await _userManager.CreateAsync(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "admin",
                    FirstName = "Quản trị",
                    LastName = "1",
                    Email = "sonlh98ba@gmail.com",
                    LockoutEnabled = false
                }, "Admin@123");
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("admin");
                    await _userManager.AddToRoleAsync(user, AdminRoleName);
                }
            }

            #endregion Người dùng

            #region Chức năng

            if (!_context.Functions.Any())
            {
                _context.Functions.AddRange(new List<Function>
                {
                    new Function {Id = "DASHBOARD", Name = "Bảng điều khiển", ParentId = null, SortOrder = 1, Url = "/dashboard", Icon="fa-dashboard"},

                    new Function {Id = "CONTENT", Name = "Nội dung", ParentId = null, SortOrder = 2, Url = "/contents", Icon="fa-table"},

                    new Function {Id = "CONTENT_CATEGORY", Name = "Danh mục", ParentId ="CONTENT", SortOrder = 1, Url = "/contents/categories", Icon="fa-edit"},
                    new Function {Id = "CONTENT_COMMENT", Name = "Bình luận", ParentId = "CONTENT", SortOrder = 2, Url = "/contents/knowledge-bases/comments", Icon="fa-edit" },
                    new Function {Id = "CONTENT_KNOWLEDGEBASE", Name = "Bài viết", ParentId = "CONTENT", SortOrder = 3, Url = "/contents/knowledge-bases", Icon="fa-edit"},
                    new Function {Id = "CONTENT_REPORT", Name = "Báo xấu", ParentId = "CONTENT", SortOrder = 4, Url = "/contents/knowledge-bases/reports", Icon="fa-edit" },

                    new Function {Id = "STATISTIC", Name = "Thống kê", ParentId = null, SortOrder = 3, Url = "/statistics",Icon="fa-bar-chart-o"},

                    new Function {Id = "STATISTIC_MONTHLY_COMMENT", Name = "Comment theo tháng", ParentId = "STATISTIC", SortOrder = 1, Url = "/statistics/monthly-comments", Icon = "fa-wrench" },
                    new Function {Id = "STATISTIC_MONTHLY_NEWKB", Name = "Bài đăng hàng tháng", ParentId = "STATISTIC", SortOrder = 2, Url = "/statistics/monthly-newkbs", Icon = "fa-wrench"},
                    new Function {Id = "STATISTIC_MONTHLY_NEWMEMBER", Name = "Đăng ký từng tháng", ParentId = "STATISTIC", SortOrder = 3, Url = "/statistics/monthly-registers", Icon = "fa-wrench"},

                    new Function {Id = "SYSTEM", Name = "Hệ thống", ParentId = null, SortOrder = 4, Url = "/systems", Icon="fa-th-list"},

                    new Function {Id = "SYSTEM_FUNCTION", Name = "Chức năng", ParentId = "SYSTEM", SortOrder = 1, Url = "/systems/functions", Icon="fa-desktop"},
                    new Function {Id = "SYSTEM_PERMISSION", Name = "Quyền hạn", ParentId = "SYSTEM", SortOrder = 2, Url = "/systems/permissions", Icon="fa-desktop"},
                    new Function {Id = "SYSTEM_ROLE", Name = "Nhóm quyền", ParentId = "SYSTEM", SortOrder = 3, Url = "/systems/roles", Icon="fa-desktop"},
                    new Function {Id = "SYSTEM_USER", Name = "Người dùng", ParentId = "SYSTEM", SortOrder = 4, Url = "/systems/users", Icon="fa-desktop"},
                });
                await _context.SaveChangesAsync();
            }

            if (!_context.Commands.Any())
            {
                _context.Commands.AddRange(new List<Command>()
                {
                    new Command(){Id = "VIEW", Name = "Xem"},
                    new Command(){Id = "CREATE", Name = "Thêm"},
                    new Command(){Id = "UPDATE", Name = "Sửa"},
                    new Command(){Id = "DELETE", Name = "Xoá"},
                    new Command(){Id = "APPROVE", Name = "Duyệt"},
                });
            }

            #endregion Chức năng

            var functions = _context.Functions;

            if (!_context.CommandInFunctions.Any())
            {
                foreach (var function in functions)
                {
                    var createAction = new CommandInFunction()
                    {
                        CommandId = "CREATE",
                        FunctionId = function.Id
                    };
                    _context.CommandInFunctions.Add(createAction);

                    var updateAction = new CommandInFunction()
                    {
                        CommandId = "UPDATE",
                        FunctionId = function.Id
                    };
                    _context.CommandInFunctions.Add(updateAction);
                    var deleteAction = new CommandInFunction()
                    {
                        CommandId = "DELETE",
                        FunctionId = function.Id
                    };
                    _context.CommandInFunctions.Add(deleteAction);

                    var viewAction = new CommandInFunction()
                    {
                        CommandId = "VIEW",
                        FunctionId = function.Id
                    };
                    _context.CommandInFunctions.Add(viewAction);
                }
            }

            if (!_context.Permissions.Any())
            {
                var adminRole = await _roleManager.FindByNameAsync(AdminRoleName);
                foreach (var function in functions)
                {
                    _context.Permissions.Add(new Permission(function.Id, adminRole.Id, "CREATE"));
                    _context.Permissions.Add(new Permission(function.Id, adminRole.Id, "UPDATE"));
                    _context.Permissions.Add(new Permission(function.Id, adminRole.Id, "DELETE"));
                    _context.Permissions.Add(new Permission(function.Id, adminRole.Id, "VIEW"));
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}