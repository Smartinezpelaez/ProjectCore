using System.Collections.Generic;
using System.Linq;

namespace ProjectCore.Logica.BL
{
    public class Tenats

        // Obtiene las organizaciones por usuario
    {
        public List<Models.DB.Tenants> GetTenants(string userId)
        {
            DAL.Models.ProjectCoreContext _context = new DAL.Models.ProjectCoreContext();

            var lisTenats = (from _Tenats in _context.Tenants
                             join _aspNetUser in _context.AspNetUsers on _Tenats.Id equals _aspNetUser.TenantId
                             where _aspNetUser.Id.Equals(userId)
                             select new Models.DB.Tenants
                             {
                                 Id = _Tenats.Id,
                                 Name = _Tenats.Name,
                                 Plan = _Tenats.Plan,
                                 CreatedAt = _Tenats.CreatedAt,
                                 UpdatedAt = _Tenats.UpdatedAt
                             }).ToList();

            return lisTenats;

        }
    }
}
