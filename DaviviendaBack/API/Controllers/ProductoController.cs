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
    public class ProductoController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ResponseDto _response;
        private readonly ILogger<ProductoController> _logger;
        private readonly IMapper _mapper;

        public ProductoController(ApplicationDbContext db, ILogger<ProductoController> logger,
        IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _db = db;
            _response = new ResponseDto();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos(){
            _logger.LogInformation("");
            var lista = await _db.Producto.Include(x=>x.Proveedor).ToListAsync();
            _response.Resultado = lista;
            _response.Mensaje = "Listado de Productos";
            return Ok(_response);
        }

        [HttpGet("{id}", Name = "GetProducto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Producto>> GetProducto(int id){
            if (id == 0)
            {
                _logger.LogError("Debe enviar el ID");
                _response.Mensaje = "Debe enviarse el ID";
                _response.IsExitoso = false;
                return BadRequest(_response);
            }

            var producto = await _db.Producto.FindAsync(id);

            if (producto == null)
            {
                _logger.LogError("El producto no existe");
                _response.Mensaje = "El producto  no existe!";
                _response.IsExitoso = false;
                return BadRequest(_response);
            }

            _response.Resultado = producto;
            _response.Mensaje = "Datos del producto " + producto?.Id;
            return Ok(_response); // Status code = 200
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Producto>> PostProducto([FromBody] ProductoDto productoDto){
            if(productoDto==null){
                _response.Mensaje="Información incorrecta";
                _response.IsExitoso=false;
                return BadRequest(_response);
            }

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var productoExiste = await _db.Producto.FirstOrDefaultAsync
                                (p=> p.Nombre.ToLower() == productoDto.Nombre.ToLower());

            if (productoExiste != null){
                ModelState.AddModelError("NombreDuplicado","El nombre del producto ya existe");
                return BadRequest(ModelState);
            }
            
            Producto producto = _mapper.Map<Producto>(productoDto);

            await _db.Producto.AddAsync(producto);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetProducto", new {id = producto.Id}, producto); //Status Code = 201
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PutProducto(int id, [FromBody] ProductoDto productoDto){
            if(id != productoDto.Id){
                return BadRequest("Id del producto no coincide");
            }

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var productoExiste = await _db.Producto.FirstOrDefaultAsync
                                (p=> p.Nombre.ToLower() == productoDto.Nombre.ToLower()&& p.Id != productoDto.Id);

            if (productoExiste != null){
                ModelState.AddModelError("NombreDuplicado","El nombre del producto ya existe");
                return BadRequest(ModelState);
            }
            
            Producto producto = _mapper.Map<Producto>(productoDto);
            
            _db.Update(producto);
            await _db.SaveChangesAsync();
            return Ok(producto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProducto(int id){
            var producto = await _db.Producto.FindAsync(id);
            if( producto == null){
                return NotFound();
            }
            _db.Producto.Remove(producto);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}