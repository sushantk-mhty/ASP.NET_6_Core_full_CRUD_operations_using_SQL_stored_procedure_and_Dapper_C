
namespace StudentRecordManagementApp.Services
{
    public class StudentServices:IStudentService
    {
        private readonly IConfiguration _configuration;
        public string ConnectionString { get; set; }
        public string providerName { get; set; }
        public StudentServices(IConfiguration configuration)
        {
            this._configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("DBConnection");
            providerName = "System.Data.SqlClient";
        }
        public IDbConnection Connection
        {
            get { return new SqlConnection(ConnectionString); }
        }
        public string AddStudent(Student std)
        {
            string result=string.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var _students = dbConnection.Query<Student>("InsertStudentRecord", new { FullName=std.FullName, EmailAddress=std.EmailAddress,City=std.City, CreatedBy=1 }, commandType: CommandType.StoredProcedure).ToList();
                    if (_students != null && _students.FirstOrDefault().Response == "Saved Successfully")
                    {
                        result= "Saved Successfully";
                    }
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        public List<Student> GetStudentsList()
        {
            List <Student> results= new List<Student>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    results = dbConnection.Query<Student>("GetStudentsList", commandType: CommandType.StoredProcedure).ToList();
                    //var topRow = _students.FirstOrDefault();
                    //if (topRow != null)
                    //{

                    //}
                    dbConnection.Close();
                    return results;
                }
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                return results;
            }
        }

        public string DeleteStudent(int studentID)
        {
            string result = string.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var _students = dbConnection.Query<Student>("DeleteStudentRecordById", new { StudentID = studentID }, commandType: CommandType.StoredProcedure).ToList();
                    if (_students != null && _students.FirstOrDefault().Response == "Deleted Successfully")
                    {
                        result = "Deleted Successfully";
                    }
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public string UpdateStudent(Student std)
        {
            string result = string.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var _students = dbConnection.Query<Student>("UpdateStudentRecordById", new { FullName = std.FullName, EmailAddress = std.EmailAddress, City = std.City, StudentID = std.StudentID }, commandType: CommandType.StoredProcedure).ToList();
                    if (_students != null && _students.FirstOrDefault().Response == "Updated Successfully")
                    {
                        result = "Updated Successfully";
                    }
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
    }
    public interface IStudentService
    {
        public string AddStudent(Student std);
        public string UpdateStudent(Student std);
        public string DeleteStudent(int studentID);
        public List<Student> GetStudentsList();
    }
}
