using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Diamonds.Models.Entities;
using Diamonds.Models.PermissionModels;

namespace Diamonds.Controllers
{
    public class UserController : Controller
    {
        private DiamondsEntities db = new DiamondsEntities();

        public ActionResult Index()
        {
            return RedirectToAction("Admin");
        }

        public ActionResult Admin()
        {
            List<User> users = db.Users.OrderBy(u => u.name).ToList();
            return View(users);
        }


        public ViewResult Edit(int id)
        {
            // Tutaj administrator może zmienić uprawnienia użytkownika
            User user = db.Users.Single(u => u.id == id);
            return View(user);
        }


        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                User dbUser = db.Users.Single(u => u.id == user.id);
                dbUser.roleGroupId = user.roleGroupId;
                db.SaveChanges();

                TempData["Message"] = "Zapisano dane";
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Wystąpił błąd";
            return View(user);
        }


        #region Roles
        
        public ViewResult Roles()
        {
            List<Role> roles = db.Roles.OrderBy(r => r.name).ToList();
            return View(roles);
        }


        public ActionResult RoleCreate()
        {
            ViewBag.RoleAccess = getGroupsAccess(new Role());
            return View("RoleEdit", new Role());
        }


        [HttpPost]
        public ActionResult RoleCreate(Role role, List<RoleAccess> roleAccesses)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(role);
                foreach (var group in roleAccesses)
                    role.rolegroup_role.Add(group.ToRoleGroupRole());
                db.SaveChanges();
                return RedirectToAction("Roles");
            }
            ViewBag.RoleAccess = getGroupsAccess(role);
            return View(role);
        }


        public ActionResult RoleEdit(short id)
        {
            Role role = db.Roles.Single(r => r.roleId == id);
            ViewBag.RoleAccess = getGroupsAccess(role);
            return View(role);
        }


        [HttpPost]
        public ActionResult RoleEdit(int id, Role role, List<RoleAccess> roleAccesses)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Attach(role);
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();

                db = new DiamondsEntities();
                role = db.Roles.Single(r => r.roleId == id);

                role.rolegroup_role.Clear();
                foreach (var group in roleAccesses)
                    role.rolegroup_role.Add(group.ToRoleGroupRole());
                db.SaveChanges();
                return RedirectToAction("RoleEdit", new { id = role.roleId });
            }

            ViewBag.RoleAccess = getGroupsAccess(role);
            return View(role);
        }
        

        public ActionResult RoleDelete(int id)
        {
            Role role = db.Roles.Single(r => r.roleId == id);
            db.Roles.Remove(role);
            db.SaveChanges();
            return RedirectToAction("Roles");
        }


        public ViewResult RoleGroups()
        {
            List<RoleGroup> rolegroups = db.RoleGroups.OrderBy(r => r.name).ToList();
            return View(rolegroups);
        }

        public ViewResult RoleGroupCreate()
        {
            RoleGroup roleGroup = new RoleGroup();
            ViewBag.RoleGroups = db.RoleGroups.ToList();

            var rgaList = new List<RoleGroupAccess>();
            var roles = db.Roles;
            foreach (var role in roles)
            {
                var rga = new RoleGroupAccess();
                rga.Role = role;
                rga.RoleGroup = roleGroup;
                rgaList.Add(rga);
            }
            ViewBag.RoleGroupAccess = rgaList;
            return View("RoleGroupEdit", roleGroup);
        }

        [HttpPost]
        public ActionResult RoleGroupCreate(RoleGroup roleGroup, List<RoleGroupAccess> roles)
        {
            if (ModelState.IsValid)
            {
                db.RoleGroups.Add(roleGroup);

                roleGroup.rolegroup_role.Clear();
                foreach (var role in roles)
                {
                    var rgr = new rolegroup_role();
                    rgr.roleId = role.Role.roleId;
                    if (role.isDefault) continue;
                    rgr.value = role.isAccess;
                    roleGroup.rolegroup_role.Add(rgr);
                }
                db.SaveChanges();
                return RedirectToAction("EditGroup", new { id = roleGroup.id });
            }

            ViewBag.RoleGroups = db.RoleGroups.ToList();
            return View("RoleGroupEdit", roleGroup);
        }


        public ActionResult RoleGroupEdit(int id)
        {
            RoleGroup roleGroup = db.RoleGroups.Include("rolegroup_role").Single(r => r.id == id);
            ViewBag.RoleGroups = db.RoleGroups.ToList();

            var rgaList = new List<RoleGroupAccess>();
            var roles = db.Roles;
            foreach (var role in roles)
            {
                var rga = new RoleGroupAccess();
                rga.Role = role;
                rga.RoleGroup = roleGroup;

                var rgr = roleGroup.rolegroup_role.SingleOrDefault(r => r.roleId == role.roleId);
                if (rgr == null)
                    rga.value = 2;
                else if (rgr.value)
                    rga.value = 1;
                else
                    rga.value = 0;

                rgaList.Add(rga);
            }
            ViewBag.RoleGroupAccess = rgaList;

            return View(roleGroup);
        }


        [HttpPost]
        public ActionResult RoleGroupEdit(RoleGroup roleGroup, List<RoleGroupAccess> roles)
        {
            if (ModelState.IsValid)
            {
                RoleGroup editedRoleGroup = db.RoleGroups.Find(roleGroup.id);
                db.Entry(editedRoleGroup).CurrentValues.SetValues(roleGroup);
                editedRoleGroup.rolegroup_role.Clear();
                foreach (var role in roles)
                {
                    var rgr = new rolegroup_role();
                    rgr.roleId = role.Role.roleId;
                    if (role.isDefault) continue;
                    rgr.value = role.isAccess;
                    editedRoleGroup.rolegroup_role.Add(rgr);
                }
                db.SaveChanges();
                return RedirectToAction("EditGroup", new { id = roleGroup.id });
            }

            ViewBag.RoleGroups = db.RoleGroups.ToList();
            return View(roleGroup);
        }


        public ActionResult RoleGroupDelete(int id)
        {
            RoleGroup roleGroup = db.RoleGroups.Single(r => r.id == id);
            roleGroup.rolegroup_role.Clear();
            db.RoleGroups.Remove(roleGroup);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        private List<RoleAccess> getGroupsAccess(Role role)
        {
            List<RoleAccess> groups = new List<RoleAccess>();
            List<RoleGroup> roleGroups = db.RoleGroups.ToList();

            foreach (var group in roleGroups)
            {
                var roleAccess = new RoleAccess();
                roleAccess.Group = new Group(group);

                if (role == null)
                    roleAccess.value = false;
                else
                {
                    var rgr = role.rolegroup_role.SingleOrDefault(r => r.roleGroupId == group.id);
                    if (rgr != null)
                        roleAccess.value = rgr.value;
                    else
                        roleAccess.value = false;
                }
                groups.Add(roleAccess);
            }

            return groups;
        }

        #endregion

    }
}
