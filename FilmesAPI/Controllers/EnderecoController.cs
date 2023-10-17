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
    public class EnderecoController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public EnderecoController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AdicionaEndereco([FromBody] CreateEnderecoDto enderecoDto)
        {
            var endereco = _mapper.Map<Endereco>(enderecoDto);
            _context.Enderecos.Add(endereco);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaEnderecoPorId),
                new { id = endereco.Id }, endereco);
        }

        [HttpGet]
        public IEnumerable<ReadEnderecoDto> RecuperaEnderecos()
        {
            return _mapper.Map<List<ReadEnderecoDto>>(_context.Enderecos);
        }

        [HttpGet("{id}")]
        public IActionResult RecuperaEnderecoPorId(int id)
        {
            var endereco = _context.Enderecos
                .FirstOrDefault(endereco => endereco.Id == id);
            if (endereco == null) return NotFound();
            var enderecoDto = _mapper.Map<ReadEnderecoDto>(endereco);
            return Ok(enderecoDto);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizaEndereco(int id, [FromBody] UpdateFilmeDto enderecoDto)
        {
            var endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
            if (endereco == null) return NotFound();
            _mapper.Map(enderecoDto, endereco);
            _context.SaveChanges();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeletaEndereco(int id)
        {
            var endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
            if (endereco == null) return NotFound();
            _context.Remove(endereco);
            _context.SaveChanges();
            return NoContent();
        }
    }
}