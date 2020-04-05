using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectCore.Logica.BL
{
    public  class Projects
    {
        // Metodo para Obtener Proyectos Para ID y Tenants o User invited

        public List<Models.DB.Projects> GetProjects( int? id,
            int? tenantId,
            string userId = null)

        {
            DAL.Models.ProjectCoreContext _context = new DAL.Models.ProjectCoreContext();

            var listProjectsEf = (from _projects in  _context.Projects 
                                 select _projects).ToList(); //listamos todos los proyectos del contexto de datos

            if (id != null)
                listProjectsEf = listProjectsEf.Where(x => x.Id == id).ToList(); //filtra la lista
            if (tenantId != null)
                listProjectsEf = listProjectsEf.Where(x => x.TenantId == tenantId).ToList();
            if (!string.IsNullOrEmpty(userId))
                listProjectsEf = (from _projects in listProjectsEf
                                  join _userProjects in _context.UserProjects on _projects.Id equals _userProjects.ProjectId
                                  where _userProjects.Id.Equals(userId)
                                  select _projects).ToList();

            var listProjects = (from _projects in listProjectsEf
                                select new Models.DB.Projects
                                {
                                    Id = _projects.Id,
                                    Title= _projects.Title,
                                    Details= _projects.Details,
                                    ExpectedCompletionDate= _projects.ExpectedCompletionDate,
                                    TenantId= _projects.TenantId,
                                    CreatedAt= _projects.CreatedAt,
                                    UpdatedAt= _projects.UpdatedAt,
                                }).ToList();

            return listProjects;


        }

        // Inser in to Projects (Crea el project)
        public void CreateProjects (
            string title, 
            string details,
            DateTime? expectedCompletionDate,
            int? tenantID )
        {
            DAL.Models.ProjectCoreContext _context = new DAL.Models.ProjectCoreContext();
            _context.Projects.Add(new DAL.Models.Projects

            {
                Title = title,
                Details = details,
                ExpectedCompletionDate = expectedCompletionDate,
                TenantId = tenantID,
                CreatedAt = DateTime.Now


            });

            // INSERT INTO Projects (Title, Details,ExpectedCompletionDate,TenantId,CreatedAt )
            // VALUES ("", "", "YYYY-MM-DD","1","YYYY-MM-DD")


            // Aplica todos los cambios a nivel de objetos en la BD
            _context.SaveChanges();

        }

        // Update projects (Actualiza el proyecto)
        public void UpdateProjects(
            int id,
            string title,
            string details,
            DateTime? expectedCompletionDate)
        {
            DAL.Models.ProjectCoreContext _context = new DAL.Models.ProjectCoreContext();



            var projectEF = _context.Projects.Where(x=>x.Id==id).FirstOrDefault();

            //  FORMA LARGA
            //(from _projects in _context.Projects
            //where _projects.Id == id
            // select _projects).FirstOrDefault();

            projectEF.Title=title;
            projectEF.Details = details;
            projectEF.ExpectedCompletionDate = expectedCompletionDate;
            projectEF.UpdatedAt = DateTime.Now;

            // Update Projects 
            //Set (Title=" ", Details=" ",ExpectedCompletionDate= " ",UpdatedAt=" " )
            // WHERE Id = X


            // Aplica todos los cambios a nivel de objetos en la BD
            _context.SaveChanges();

        }

        //Delete Projects (borra los registros del proyecto)

        public void DeleteProjects(int? id)
        {
            DAL.Models.ProjectCoreContext _context = new DAL.Models.ProjectCoreContext();

            // Validamos dependencias de la tabla Projects
            if (_context.Artifacts.Any(x => x.ProjectId == id) || _context.UserProjects.Any(x => x.ProjectId == id))
                return;           

            var projectEF = _context.Projects.Where(x => x.Id == id).FirstOrDefault();

            _context.Projects.Remove(projectEF);

            // Delete FROM  Projects           
            // WHERE Id = X

            // Aplica todos los cambios a nivel de objetos en la BD
            _context.SaveChanges();

        }



    }
}
