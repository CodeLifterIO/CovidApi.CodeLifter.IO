using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CovidApi.Data;
using CovidApi.Models;
using CovidApi.Repositories;

namespace CovidApi.Controllers
{
    public class DataFilesController : Controller
    {
        private readonly IDataFileRepository _dataFileRepo;

        public DataFilesController(IDataFileRepository dataFileRepo)
        {
            _dataFileRepo = dataFileRepo;
        }

        [HttpPost("[controller]/[action]")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Datatable()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var dataset = await _dataFileRepo.SearchAndSortAsync(searchValue, sortColumn, sortColumnDirection, skip, pageSize);

                recordsTotal = dataset.Count();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = dataset };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: DataFiles
        public async Task<IActionResult> Index()
        {
            return View(await _dataFileRepo.GetAllAsync());
        }

        // GET: DataFiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataFile = await _dataFileRepo.FindAsync((int)id);
            if (dataFile == null)
            {
                return NotFound();
            }

            return View(dataFile);
        }

        // GET: DataFiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DataFiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DataFile dataFile)
        {
            if (ModelState.IsValid)
            {
                await _dataFileRepo.AddAsync(dataFile);
                return RedirectToAction(nameof(Index));
            }
            return View(dataFile);
        }

        // GET: DataFiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataFile = await _dataFileRepo.FindAsync((int)id);
            if (dataFile == null)
            {
                return NotFound();
            }
            return View(dataFile);
        }

        // POST: DataFiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DataFile dataFile)
        {
            if (id != dataFile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _dataFileRepo.UpdateAsync(dataFile);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _dataFileRepo.ExistsAsync(dataFile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dataFile);
        }
    }
}
