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
    public class VentaController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ResponseDto _response;
        private readonly ILogger<VentaController> _logger;
        private readonly IMapper _mapper;

        public VentaController(ApplicationDbContext db, ILogger<VentaController> logger,
        IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _db = db;
            _response = new ResponseDto();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentas(){
            _logger.LogInformation("");
            var lista = await _db.Venta.Include(x=>x.Cliente).Include(x=>x.Producto).ToListAsync();
            _response.Resultado = lista;
            _response.Mensaje = "Listado de Ventas";
            return Ok(_response);
        }
        
        [HttpGet("{id}", Name = "GetVenta")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Venta>> GetVenta(int id){
            if (id == 0)
            {
                _logger.LogError("Debe enviar el ID");
                _response.Mensaje = "Debe enviarse el ID";
                _response.IsExitoso = false;
                return BadRequest(_response);
            }

            var venta= await _db.Venta.FindAsync(id);

            if (venta == null)
            {
                _logger.LogError("La venta no existe");
                _response.Mensaje = "La venta no existe!";
                _response.IsExitoso = false;
                return BadRequest(_response);
            }

            _response.Resultado = venta;
            _response.Mensaje = "Datos de la venta" + venta?.Id;
            return Ok(_response); // Status code = 200
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public async Task<ActionResult<Venta>> PostVenta([FromBody] VentaDto ventaDto){
            if (ventaDto == null)
            {
                _response.Mensaje = "Informaicón Incorrecta";
                _response.IsExitoso = false;
                return BadRequest(_response);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ventaExiste = await _db.Venta.FirstOrDefaultAsync
                                        (v => v.Id == ventaDto.Id);

            if (ventaExiste != null)
            {
                ModelState.AddModelError("VentaDuplicado", "La venta ya existe");
                return BadRequest(ModelState);
            }

            Venta venta = _mapper.Map<Venta>(ventaDto);

        
            await _db.Venta.AddAsync(venta);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetVenta", new {id = ventaDto.Id}, venta); //Status Code = 201
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PutVenta(int id, [FromBody] VentaDto ventaDto){
            if (id != ventaDto.Id)
            {
                return BadRequest("Id de compania con coincidie");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ventaExiste = await _db.Venta.FirstOrDefaultAsync(v => v.Id == ventaDto.Id);

            if (ventaExiste != null)
            {
                ModelState.AddModelError("VentaDuplicada", "La venta ya existe");
                return BadRequest(ModelState);
            }


            Venta venta = _mapper.Map<Venta>(ventaDto);

            _db.Update(venta);
            await _db.SaveChangesAsync();
            return Ok(venta);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteVenta(int id){
            var venta = await _db.Venta.FindAsync(id);
            if( venta == null){
                return NotFound();
            }
            _db.Venta.Remove(venta);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}