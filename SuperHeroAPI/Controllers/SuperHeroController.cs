using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SuperHeroAPI.Data;
using SuperHeroAPI.Entities;
using System.Reflection.Metadata;

namespace SuperHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        //insertando conexion a base datos, forma mala practica por que las conexiones de la bd no van en los controllers

        private readonly DataContext _dataContext;

        public SuperHeroController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        //metodo o controler para traer todos los datos de la bd , se delimita a una lista esperada
        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAllHeroes()
        {
            var heroes = await _dataContext.SuperHeroes.ToListAsync();


            return Ok(heroes);
        }

        //metodo para encontrar por id
        [HttpGet("{id}")]
        public async Task<ActionResult<List<SuperHero>>> GetAHeroes(int id)
        {
            var hero = await _dataContext.SuperHeroes.FindAsync(id);
            if (hero == null)
            {
                return NotFound("Hero not Found");
            }
            return Ok(hero);
        }

        //metodo para crear
        //para el metodo post(crear) en ves de pasar en el metodo como parametro el objetivo de tipo[] , hay que usar un DTO , un objeto que realmente quieres ver u obtener
        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
            _dataContext.SuperHeroes.Add(hero);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }

        //metodo para actualizar
        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero hero)
        {
            var onehero = await _dataContext.SuperHeroes.FindAsync(hero.Id);
            if (onehero == null)
            {
                return NotFound("Hero not Found");
            }

            onehero.Name = hero.Name;
            onehero.FirstName = hero.FirstName;
            onehero.LastName = hero.LastName;
            onehero.Place = hero.Place;

            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }

        //metodo para borrar
        [HttpDelete]
        public async Task<ActionResult<List<SuperHero>>> DeleteHero(int Id)
        {
            var onehero = await _dataContext.SuperHeroes.FindAsync(Id);
            if (onehero == null)
            {
                return NotFound("Hero not Found");
            }

            _dataContext.SuperHeroes.Remove(onehero);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }


    }
}
