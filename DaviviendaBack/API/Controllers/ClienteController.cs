using AutoMapper;
using Core.Dto;
using Core.Entidades;
using Infraestructura.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ResponseDto _response;
        private readonly ILogger<ClienteController> _logger;
        private readonly IMapper _mapper;
        public ClienteController(ApplicationDbContext db, ILogger<ClienteController> logger,
        IMapper mapper){
            _mapper = mapper;
            _logger = logger;
            _db = db;
            _response = new ResponseDto();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            var lista = await _db.Cliente.ToListAsync();
            _response.Resultado = lista;
            _response.Mensaje = "Listado de CLientes";
            return Ok(_response);
        }

        [HttpGet("{id}", Name = "GetCliente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Cliente>> GetCliente(int id){
            var cliente = await _db.Cliente.FindAsync(id);
            _response.Resultado = cliente;
            _response.Mensaje = "Datos del cliente" + cliente?.Id;
            return Ok(_response); // Status code = 200
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Cliente>> PostCliente([FromBody] Cliente cliente){
            await _db.Cliente.AddAsync(cliente);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetCliente", new { id = cliente.Id }, cliente); //Status Code = 201
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PutCliente(int id, [FromBody] Cliente cliente){
            if (id != cliente.Id){
                return BadRequest("Id del cliente no coincide");
            }
            _db.Update(cliente);
            await _db.SaveChangesAsync();
            return Ok(cliente);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCliente(int id){
            var cliente = await _db.Cliente.FindAsync(id);
            if (cliente == null){
                return NotFound();
            }
            _db.Cliente.Remove(cliente);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}