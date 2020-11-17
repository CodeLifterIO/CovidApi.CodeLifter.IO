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
        private readonly CovidContext _context;
        private readonly IDataFileRepository _dataFileRepo;

        public DataFilesController(CovidContext context, IDataFileRepository dataFileRepo)
        {
            _context = context;
            _dataFileRepo = dataFileRepo;
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
