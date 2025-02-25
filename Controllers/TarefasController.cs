﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using toDo.Data;
using toDo.Models;

namespace toDo.Controllers
{
    public class TarefasController : Controller
    {
        private readonly AppCont _appCont;

        public TarefasController(AppCont appCont)
        {
            _appCont = appCont;
        }

        //Action sincrona, manda uma RC para o servidor e irá esperar a resposta para continuar.
        public IActionResult Index() // um Método IActionResult espera o retorno de uma view.
        {
            var allTasks = _appCont.Tarefas.ToList();
            // _appCont: caminho do banco,  ToList: seria o select *
            return View(allTasks);
        }

        // GET: Tarefas /Details/5
        public async Task<IActionResult> Details(int? id)// O "?" é para ele aceitar os valores nulos, para não dar B.O no código
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefa = await _appCont.Tarefas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tarefa == null)
            {
                return NotFound();
            }

            return View(tarefa);
        }

        // GET: Tarefas/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,EndDate,Status")] Tarefa tarefa)
        {
            if (ModelState.IsValid)
            {
                _appCont.Add(tarefa);
                await _appCont.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tarefa);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefa = await _appCont.Tarefas.FindAsync(id);
            if (tarefa == null)
            {
                return NotFound();
            }
            return View(tarefa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Edit(int id, [Bind("Id,Name,EndDate,Status")] Tarefa tarefa)
        {
            if (id != tarefa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _appCont.Update(tarefa);
                    await _appCont.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TarefaExist(tarefa.Id))
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
            return View(tarefa);
        }

        // GET: Tarefas/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefa = await _appCont.Tarefas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tarefa == null)
            {
                return NotFound();
            }

            return View(tarefa);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarefa = await _appCont.Tarefas.FindAsync(id);
            _appCont.Tarefas.Remove(tarefa);
            await _appCont.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TarefaExist(int id)
        {
            return _appCont.Tarefas.Any(e => e.Id == id);
        }
    }
}