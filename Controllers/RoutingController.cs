using Microsoft.AspNetCore.Mvc;
using UCITMS.Data.IRepositories;

namespace UCITMS.Controllers
{
    public class RoutingController : Controller
    {
        private readonly IUserInfoRepository _userinfoRepository;

        public RoutingController(IUserInfoRepository userInfoRepository)
        {
            _userinfoRepository = userInfoRepository;
        }

        //------------------------------
        //-X-X-X-X-X-X-X-X

        //FOR LOGIN

        //-X-X-X-X-X-X-X-X

        public IActionResult Login()
        {
            return View("Login/Index");
        }
        public IActionResult LandingPage()
        {
            var user = _userinfoRepository.GetuserbyEmail(User.Identity.Name);
            if (user == null)
            {
                ViewBag.Message = "Invalid Credentials";
                return View("Login/Index");
            }

            UserSession.StoreUserId(HttpContext, user.UserID);
            UserSession.StoreUserName(HttpContext, user.Username);
            UserSession.StoreUserEmail(HttpContext, user.Email);

            return View("Home/Index");
        }
        [HttpPost]
        public IActionResult Authenticate()
        {
            
                
            var user = _userinfoRepository.GetuserbyEmail(User.Identity.Name);
            if (user == null)
            {
                ViewBag.Message = "Invalid Credentials";
                return View("Login/Index");
            }

            UserSession.StoreUserId(HttpContext, user.UserID);
            UserSession.StoreUserName(HttpContext, user.Username);
            UserSession.StoreUserEmail(HttpContext, user.Email);

            return RedirectToAction("HomePage");
        }

        [HttpGet("/HomePage")]
        public IActionResult HomePage()
        {
            return View("Home/Index");
        }

        //------------------------------
        //-X-X-X-X-X-X-X-X

        // MANAGER

        //-X-X-X-X-X-X-X-X


        [HttpGet("/ManageEngagement")]
        public IActionResult ManageEngagement()
        {
            return View("Manager/ManageEngagement");
        }

        [HttpGet("/ManagerDashboard")]
        public IActionResult Dashboard()
        {
            return View("Manager/ManagerDashboard");
        }

        [HttpGet("/ApprovedTimesheets")]
        public IActionResult ApprovedTimesheets()
        {
            return View("Manager/ApprovedTimesheets");
        }

        [HttpGet("/PendingApprovals")]
        public IActionResult PendingApprovals()
        {
            return View("Manager/PendingApprovals");
        }

        //------------------------------
        //-X-X-X-X-X-X-X-X

        // HR

        //-X-X-X-X-X-X-X-X


        [HttpGet("/HRDashboard")]
        public IActionResult HRDashboard()
        {
            return View("HRAdmin/HRDashboard");
        }

        [HttpGet("/ApprovalStatus")]
        public IActionResult ApprovalStatus()
        {
            return View("HRAdmin/ApprovalStatus");
        }

        [HttpGet("/AssignApprover")]
        public IActionResult AssignApprover()
        {
            return View("HRAdmin/AssignApprover");
        }

        [HttpGet("/EngagementStatus")]
        public IActionResult EngagementStatus()
        {
            return View("HRAdmin/EngagementStatus");
        }

        //------------------------------
        //-X-X-X-X-X-X-X-X
        
        // EMPLOYEE
        
        //-X-X-X-X-X-X-X-X


        [HttpGet("/MyDashboard")]
        public IActionResult MyDashboard()
        {
            return View("Employee/MyDashboard");
        }

        [HttpGet("/MyEngagements")]
        public IActionResult MyEngagements()
        {
            return View("Employee/MyEngagements");
        }

        [HttpGet("/NewTimesheet")]
        public IActionResult NewTimesheet()
        {
            return View("Employee/NewTimesheet");
        }

        [HttpGet("/PreviousTimesheets")]
        public IActionResult PreviousTImesheets()
        {
            return View("Employee/PreviousTimesheets");
        }
    }
}
