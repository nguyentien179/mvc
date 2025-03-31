using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using mvc.Pages.Enums;
using OfficeOpenXml;

namespace mvc.Controllers
{
    [Route("NashTech")]
    public class RookiesController : Controller
    {
        private readonly List<Person> _members;

        public RookiesController()
        {
            _members = DummyData.GetPeople();
        }

        [Route("Males")]
        public IActionResult Males()
        {
            var maleMembers = _members.Where(m => m.Gender == Gender.Male).ToList();
            // return View(maleMembers);
            return Json(maleMembers);
        }

        [Route("Oldest")]
        public IActionResult Oldest()
        {
            var oldestMember = _members.OrderByDescending(m => m.DateOfBirth).LastOrDefault();
            // return View(oldestMember);
            return Json(oldestMember);
        }

        [Route("FullNames")]
        public IActionResult FullNames()
        {
            var fullNames = _members.Select(m => $"{m.LastName} {m.FirstName}").ToList();
            // return View(fullNames);
            return Json(fullNames);
        }

        [Route("BirthYearFilter")]
        public IActionResult BirthYearFilter([FromQuery] string action)
        {
            Console.WriteLine($"Received action: '{action}'");
            List<Person> filteredMembers = action switch
            {
                "equals2000" => _members.Where(m => m.DateOfBirth.Year == 2000).ToList(),
                "greaterthan2000" => _members.Where(m => m.DateOfBirth.Year > 2000).ToList(),
                "lessthan2000" => _members.Where(m => m.DateOfBirth.Year < 2000).ToList(),
                _ => new List<Person>(),
            };

            // return View(filteredMembers);
            Console.WriteLine($"Action: {action}, Members Found: {filteredMembers.Count}");
            return Json(filteredMembers);
        }

        [Route("Excel")]
        public IActionResult ExportToExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("Tien");

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Persons");

                worksheet.Cells[1, 1].Value = "First Name";
                worksheet.Cells[1, 2].Value = "Last Name";
                worksheet.Cells[1, 3].Value = "Gender";
                worksheet.Cells[1, 4].Value = "Date of Birth";
                worksheet.Cells[1, 5].Value = "Phone Number";
                worksheet.Cells[1, 6].Value = "Birth Place";
                worksheet.Cells[1, 7].Value = "Is Graduated";

                int row = 2;
                foreach (var member in _members)
                {
                    worksheet.Cells[row, 1].Value = member.FirstName;
                    worksheet.Cells[row, 2].Value = member.LastName;
                    worksheet.Cells[row, 3].Value = member.Gender.ToString();
                    worksheet.Cells[row, 4].Value = member.DateOfBirth.ToString("yyyy-MM-dd");
                    worksheet.Cells[row, 5].Value = member.PhoneNumber;
                    worksheet.Cells[row, 6].Value = member.BirthPlace;
                    worksheet.Cells[row, 7].Value = member.IsGraduated ? "Yes" : "No";
                    row++;
                }

                worksheet.Cells.AutoFitColumns();

                var excelBytes = package.GetAsByteArray();

                return File(
                    excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Persons.xlsx"
                );
            }
        }
    }
}
