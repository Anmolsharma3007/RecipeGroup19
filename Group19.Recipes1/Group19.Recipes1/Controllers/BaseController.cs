using System.Web.Mvc;
using Group19.Recipes1.DataAccess.Repository;

namespace Group19.Recipes1.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        protected BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.IsChildAction)
                _unitOfWork.BeginTransaction();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.IsChildAction)
            {
                if (filterContext.Exception != null)
                    _unitOfWork.Rollback();
                else
                    _unitOfWork.Commit();
            }
        }
    }
}