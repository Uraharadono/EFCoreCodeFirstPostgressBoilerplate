using EFCoreCodeFirstPostgressBoilerplate.UowRepo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreCodeFirstPostgressBoilerplate.UowRepo.Util
{
    // TODO: Figure out how to register this in Startup to resolve 
    public abstract class BaseController : ControllerBase
    {
        public IUnitOfWork UnitOfWork { get; set; }

        [NonAction]
        protected IRepository<T> Repo<T>() where T : class, IEntity
        {
            return UnitOfWork.Repo<T>();
        }

        [NonAction]
        protected void Commit()
        {
            UnitOfWork.Commit();
        }
    }
}
