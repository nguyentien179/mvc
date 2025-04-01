using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mvc.Interface;
using mvc.Models;
using mvc.Pages.Enums;
using OfficeOpenXml;

namespace mvc.Controllers
{
    [Route("NashTech")]
    public class RookiesController : Controller
    {
        private IPersonService _personService;

        public RookiesController(IPersonService personService)
        {
            _personService = personService;
        }

        public IActionResult Members([FromQuery] string filter)
        {
            var members = _personService.GetAll().ToList();
            switch (filter)
            {
                case "males":
                    members = members.Where(m => m.Gender == Gender.Male).ToList();
                    break;
                case "oldest":
                    var oldestMember = members.OrderBy(m => m.DateOfBirth).FirstOrDefault();
                    members =
                        oldestMember != null
                            ? new List<Person>
                            {
                                new Person
                                {
                                    FirstName = oldestMember.FirstName,
                                    LastName = oldestMember.LastName,
                                    Gender = oldestMember.Gender,
                                    DateOfBirth = oldestMember.DateOfBirth,
                                    PhoneNumber = oldestMember.PhoneNumber,
                                    BirthPlace = oldestMember.BirthPlace,
                                    IsGraduated = oldestMember.IsGraduated,
                                },
                            }
                            : new List<Person>();
                    break;
                case "equals2000":
                    members = members.Where(m => m.DateOfBirth.Year == 2000).ToList();
                    break;
                case "greaterthan2000":
                    members = members.Where(m => m.DateOfBirth.Year > 2000).ToList();
                    break;
                case "lessthan2000":
                    members = members.Where(m => m.DateOfBirth.Year < 2000).ToList();
                    break;
                default:
                    break;
            }
            return View(members);
        }

        public IActionResult MemberDetails(int id)
        {
            var person = _personService.GetAll().FirstOrDefault(p => p.Id == id);
            if (person == null)
            {
                return NotFound("Member not found");
            }
            return View(person);
        }

        public IActionResult Create(Person person)
        {
            _personService.ValidatePerson(person);
            _personService.Create(person);
            return RedirectToAction("Members");
        }

        public IActionResult Edit(Person person)
        {
            if (ModelState.IsValid)
            {
                _personService.Update(person);
                return RedirectToAction("Index");
            }
            return View(person);
        }

        public IActionResult Delete(int id)
        {
            _personService.Delete(id);
            return RedirectToAction("Members");
        }

        [Route("Excel")]
        public IActionResult ExportToExcel()
        {
            var members = _personService.GetAll().ToList();
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
                foreach (var member in members)
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
