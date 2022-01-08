using System.Data;
using Microsoft.AspNetCore.Mvc;
using Task.Models;
using System.Globalization;
using Microsoft.EntityFrameworkCore;


public class HomeController : Controller
{

    private readonly IWebHostEnvironment Environment;
    private readonly AppDbContext _context;
    public HomeController(IWebHostEnvironment _environment, AppDbContext context)
    {
        Environment = _environment;
        _context = context;
    }


    // get employees
    [HttpGet]
    public ActionResult Index(string search)
    {
        if (_context.Employees != null)
        {
            
            //variable to calculate proccessed rows
            var c = _context.Employees.Count();
            
            //Search employee by surname
            if (search != null)
            {
                return View(_context.Employees.Where(x => x.Surname == search).ToList());
            }
            //sort Surname by ascending 
            var Sortedemployees = _context.Employees.OrderBy(x => x.Surname).ToList();

            ViewBag.C = c;
            return View(Sortedemployees);
        }
        
        return View();
    }

    [HttpPost]
    public IActionResult Index(IFormFile postedFile)
    {

        try
        {
            if (postedFile != null)
            {
                string path = Path.Combine(Environment.WebRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Path.GetFileName(postedFile.FileName);
                string filePath = Path.Combine(path, fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                //reading data from file
                List<string> csvData = System.IO.File.ReadAllLines(filePath)
                    .Skip(1)
                    .Where(line => line.Length > 1)
                    .ToList();

                //inserting csv data into sql database 
                foreach (var row in csvData)
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        var cells = row.Split(',');
                        var emp = new Employee
                        {
                            Payroll_Number = cells[0],
                            Forenames = cells[1],
                            Surname = cells[2],
                            Date_of_Birth = DateTime.ParseExact(cells[3], "d/M/yyyy", CultureInfo.InvariantCulture),
                            Telephone = int.Parse(cells[4]),
                            Mobile = int.Parse(cells[5]),
                            Address = cells[6],
                            Address_2 = cells[7],
                            Postcode = cells[8],
                            Email_Home = cells[9],
                            Start_Date = DateTime.ParseExact(cells[10], "dd/MM/yyyy", CultureInfo.InvariantCulture)

                        };

                        _context.Add(emp);
                        _context.SaveChanges();
                    }

                }

                return RedirectToAction("Index");
            }
            return RedirectToAction("NoData");
        }
        catch (Exception ex)
        {

            return BadRequest($"{ex.Message} \nTry again with correct file and format!!!");
        }

       
    }
    [HttpGet]
    public ActionResult NoData()
    {
        return View();
    }

    //edit employee
    [HttpGet]
    public ActionResult Edit(string Payroll_Number)
    {
        var employee = _context.Employees.Find(Payroll_Number);
        if(employee == null)
        {
            return NotFound();
        }
        return View(employee);
    }

    //Save editted employee
    [HttpPost]
    public ActionResult Edit(Employee employee)
    {
        if (ModelState.IsValid)
        {
            _context.Entry(employee).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("Index");   
        }

        return View(employee);
    }


    




}