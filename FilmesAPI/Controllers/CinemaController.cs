using AutoMapper;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Data;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CinemaController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public CinemaController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AdicionaCinema([FromBody] CreateCinemaDto cinemaDto)
        {
            Cinema cinema = _mapper.Map<Cinema>(cinemaDto);
            _context.Cinemas.Add(cinema);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaCinemaPorId),
                new { id = cinema.Id }, cinema);
        }

        [HttpGet]
        public IEnumerable<ReadCinemaDto> RecuperaCinemas([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            return _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas.Skip(skip).Take(take));
        }

        [HttpGet("{id}")]
        public IActionResult RecuperaCinemaPorId(int id)
        {
            var cinema = _context.Cinemas
                .FirstOrDefault(cinema => cinema.Id == id);
            if (cinema == null) return NotFound();
            var cinemaDto = _mapper.Map<ReadCinemaDto>(cinema);
            return Ok(cinemaDto);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizaCinema(int id, [FromBody] UpdateFilmeDto cinemaDto)
        {
            var cinema = _context.Filmes.FirstOrDefault(cinema => cinema.Id == id);
            if (cinema == null) return NotFound();
            _mapper.Map(cinemaDto, cinema);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult AtualizaCinemaParcial(int id,
            JsonPatchDocument<UpdateCinemaDto> patch)
        {
            var cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
            if (cinema == null) return NotFound();

            var cinemaParaAtualizar = _mapper.Map<UpdateCinemaDto>(cinema);
            patch.ApplyTo(cinemaParaAtualizar, ModelState);

            if (!TryValidateModel(cinemaParaAtualizar))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(cinemaParaAtualizar, cinema);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletaCinema(int id)
        {
            var cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
            if (cinema == null) return NotFound();
            _context.Remove(cinema);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
