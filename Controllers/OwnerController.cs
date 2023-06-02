using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NanyPet.Api.Models;
using NanyPet.Models;
using NanyPet.Models.Dto.Owner;
using NanyPet.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace NanyPet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly ILogger<OwnerController> _logger;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, ILogger<OwnerController> logger, IMapper mapper)
        {
            _logger = logger;
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a list with all Herders
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Herder's list retrieved</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene un listado de todos los propietarios registrados",
            Description = "Obtiene un listado de todos los propietarios registrados",
            OperationId = "GetAllOwners",
            Tags =  new[] { "Propietarios" })]
        public async Task<ActionResult<IEnumerable<OwnerDto>>> GetAllOwners()
        {
            _logger.LogInformation("Obteniendo lista de propietarios"); // log -> show information on VS terminal
            IEnumerable<Owner> ownerList = await _ownerRepository.GetAllOwners();
            return Ok(_mapper.Map<IEnumerable<OwnerDto>>(ownerList));
        }

        /// <summary>
        /// Retrieves a specific Herder by unique id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param id="id" id="123">The herder id</param>
        /// <response code="200">herder retrieved</response>
        /// <response code="400">bad request</response>
        /// <response code="404">Product not found</response>
        [HttpGet("{id}", Name = "GetOwner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtiene cuidador por Id",
            Description = "Obtiene cuidador por Id",
            OperationId = "GetOwnerById",
            Tags = new[] { "Propietarios" })]
        public async Task<ActionResult<OwnerDto>> GetOwnerById(int id)
        {
            if (id == 0)
                return BadRequest();

            var owner = await _ownerRepository.GetOwnerById(v => v.Id == id);

            if (owner == null) 
            {
                _logger.LogError("No hay datos asociados a ese Id");
                return NotFound();
            }

            return Ok(_mapper.Map<OwnerDto>(owner));

        }

        /// <summary>
        /// Create a Herder in the database
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Crea un nuevo propietario en la Base de datos",
            Description = "Crea un nuevo propietario en la Base de datos",
            OperationId = "CreateOwner",
            Tags = new[] { "Propietarios" })]
        public async Task<ActionResult<OwnerDto>> CreateOwner([FromBody] OwnerCreateDto createOwnerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _ownerRepository.GetOwnerById(v => v.EmailUser == createOwnerDto.EmailUser) != null)
            {
                ModelState.AddModelError("Propietario ya existe", "Ya hay registrado un propietario con ese Id!");
                return BadRequest(ModelState);
            }

            if (createOwnerDto == null)
            {
                return BadRequest(createOwnerDto);
            }

            Owner modelOwner = _mapper.Map<Owner>(createOwnerDto);

            await _ownerRepository.CreateOwner(modelOwner);


            return CreatedAtRoute("GetHerder", new { id = modelOwner.Id }, modelOwner);

        }

        /// <summary>
        /// Update a specific Herder by unique id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param id="id" id="123">The herder id</param>
        /// <response code="200">herder retrieved</response>
        /// <response code="400">bad request</response>
        /// <response code="404">Product not found</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Actualiza datos de Propietario de la mascota",
            Description = "Actualiza datos de Propietario de la mascota",
            OperationId = "UpdateOwner",
            Tags = new[] { "Propietarios" })]
        public async Task<IActionResult> UpdateOwner(int id, [FromBody] OwnerUpdateDto updateDto)
        {
            if (updateDto == null || id!= updateDto.Id)
            {
                return BadRequest();
            }

            Owner modelOwner = _mapper.Map<Owner>(updateDto);

            await _ownerRepository.UpdateOwner(modelOwner);

            return NoContent();
        }

        /// <summary>
        /// Delete a specific Herder by unique id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param id="id" id="123">The herder id</param>
        /// <response code="200">herder retrieved</response>
        /// <response code="400">bad request</response>
        /// <response code="404">Product not found</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Elimina los datos de propietario por su id",
            Description = "Elimina los datos de propietario por su id",
            OperationId = "DeleteOwner",
            Tags = new[] { "Propietarios" })]
        public async Task<IActionResult> DeleteOwner(int id)
        {
            if (id == 0)
                return BadRequest();

            var owner = await _ownerRepository.GetOwnerById(v => v.Id == id);

            if (owner == null)
                return NotFound();

            await _ownerRepository.DeleteOwner(owner);
            return NoContent();
        }
    }
}
