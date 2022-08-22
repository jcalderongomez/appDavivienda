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
    public class ProveedorController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ResponseDto _response;
        private readonly ILogger<ProveedorController> _logger;
        private readonly IMapper _mapper;

        public ProveedorController(ApplicationDbContext db, ILogger<ProveedorController> logger,
        IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _db = db;
            _response = new ResponseDto();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Proveedor>>> GetProveedores(){
            _logger.LogInformation("");
            var lista = await _db.Proveedor.ToListAsync();
            _response.Resultado = lista;
            _response.Mensaje = "Listado de Proveedores";
            return Ok(_response);
        }
        
        [HttpGet("{id}", Name = "GetProveedor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Proveedor>> GetProveedor(int id){
            if (id == 0)
            {
                _logger.LogError("Debe enviar el ID");
                _response.Mensaje = "Debe enviarse el ID";
                _response.IsExitoso = false;
                return BadRequest(_response);
            }

            var prov = await _db.Proveedor.FindAsync(id);

            if (prov == null)
            {
                _logger.LogError("El proveedor no existe");
                _response.Mensaje = "El proveedor no existe!";
                _response.IsExitoso = false;
                return BadRequest(_response);
            }

            _response.Resultado = prov;
            _response.Mensaje = "Datos del proveedor " + prov?.Id;
            return Ok(_response); // Status code = 200
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Proveedor>> PostProveedor([FromBody] ProveedorDto proveedorDto){
            if (proveedorDto == null)
            {
                _response.Mensaje = "Informaicón Incorrecta";
                _response.IsExitoso = false;
                return BadRequest(_response);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var proveedorExiste = await _db.Proveedor.FirstOrDefaultAsync
                                        (p => p.Nombre.ToLower() == proveedorDto.Nombre.ToLower());

            if (proveedorExiste != null)
            {
                ModelState.AddModelError("NombreDuplicado", "Nombre del proveedor ya existe");
                return BadRequest(ModelState);
            }

            Proveedor proveedor = _mapper.Map<Proveedor>(proveedorDto);

            await _db.Proveedor.AddAsync(proveedor);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetProveedor", new {id = proveedor.Id}, proveedor); //Status Code = 201
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PutProveedor(int id, [FromBody] ProveedorDto proveedorDto){
            if(id != proveedorDto.Id){
                return BadRequest("Id del proveedor no coincide");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var proveedorExiste = await _db.Proveedor.FirstOrDefaultAsync(
                p => p.Nombre.ToLower() == proveedorDto.Nombre.ToLower()
                && p.Id != proveedorDto.Id);

            if (proveedorExiste != null)
            {
                ModelState.AddModelError("NombreDuplicado", "Nombre del proveedor ya existe");
                return BadRequest(ModelState);
            }


            Proveedor proveedor = _mapper.Map<Proveedor>(proveedorDto);

            _db.Update(proveedor);
            await _db.SaveChangesAsync();
            return Ok(proveedor);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProveedor(int id){
            var proveedor = await _db.Proveedor.FindAsync(id);
            if( proveedor == null){
                return NotFound();
            }
            _db.Proveedor.Remove(proveedor);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}