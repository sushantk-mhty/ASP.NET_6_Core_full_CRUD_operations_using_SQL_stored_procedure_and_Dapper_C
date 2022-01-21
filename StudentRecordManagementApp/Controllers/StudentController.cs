namespace StudentRecordManagementApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            this._studentService = studentService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult StudentsList()
        {
            AllModels model = new AllModels();
            model.StudentsList = _studentService.GetStudentsList();
            return View(model);
        }
        [HttpPost]
        public IActionResult AddUpdateStudentRecord(Student formData)
        {
            formData.CreatedOn = DateTime.Now;
            string url = Request.Headers["Referer"].ToString();
            string result=string.Empty;
            if (formData.StudentID>0)
            {
                result = _studentService.UpdateStudent(formData);
            }
            else
            {
                result = _studentService.AddStudent(formData);
            }
            switch (result)
            {
                case "Saved Successfully":
                    TempData["SuccessMsg"] = "Saved Successfully";
                    break;
                case "Updated Successfully":
                    TempData["SuccessMsg"] = "Updated Successfully";
                    break;
                default:
                    TempData["errorMsg"] = result;
                    break;
            }
            return Redirect(url);
        }
        [HttpPost]
        public IActionResult DeleteStudentRecord(int StudentID)
        {
            string result = _studentService.DeleteStudent(StudentID);
            if (result == "Deleted Successfully")
                return Json(new { message= "Deleted Successfully" });
            else
                return Json(new { message = "An error occured" });
        }
    }
}
