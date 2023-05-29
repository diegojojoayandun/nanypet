using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NanyPet.Models;
using NanyPet.Models.Dto.Herder;
using NanyPet.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace NanyPet.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HerderController : ControllerBase
    {
        private readonly ILogger<HerderController> _logger;
        private readonly IHerderRepository _herderRepository;
        private readonly IMapper _mapper;

        public HerderController(IHerderRepository herderRepository, ILogger<HerderController> logger, IMapper mapper)
        {
            _logger = logger;
            _herderRepository = herderRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a list with all Herders
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Herder's list retrieved</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(
            Summary = "Obtiene un listado de todos los cuidadores registrados",
            Description = "Obtiene un listado de todos los cuidadores registrados",
            OperationId = "GetAllHerders",
            Tags = new[] { "Cuidadores" })]
        public async Task<ActionResult<IEnumerable<HerderDto>>> GetAllHerders()
        {
            _logger.LogInformation("Obteniendo lista de cuidadores"); // log -> show information on VS terminal
            IEnumerable<Herder> herderList = await _herderRepository.GetAllHerders();
            return Ok(_mapper.Map<IEnumerable<HerderDto>>(herderList));
        }

        /// <summary>
        /// Retrieves a specific Herder by unique id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param id="id" id="123">The herder id</param>
        /// <response code="200">herder retrieved</response>
        /// <response code="400">bad request</response>
        /// <response code="404">Product not found</response>
        [HttpGet("{id}", Name = "GetHerder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtiene cuidador por Id",
            Description = "Obtiene cuidador por Id",
            OperationId = "GetHerderById",
            Tags = new[] { "Cuidadores" })]
        public async Task<ActionResult<HerderDto>> GetHerderById(int id)
        {
            if (id == 0)
                return BadRequest();

            var herder = await _herderRepository.GetHerderById(v => v.Id == id);

            if (herder == null)
            {
                _logger.LogError("No hay datos asociados a ese Id");
                return NotFound();
            }

            return Ok(_mapper.Map<HerderDto>(herder));

        }

        /// <summary>
        /// Create a Herder in the database
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Crea un nuevo cuidador en la Base de datos",
            Description = "Crea un nuevo cuidador en la Base de datos",
            OperationId = "CreateHerder",
            Tags = new[] { "Cuidadores" })]
        public async Task<ActionResult<HerderDto>> CreateHerder([FromBody] HerderCreateDto createHerderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _herderRepository.GetHerderById(v => v.Email == createHerderDto.Email) != null)
            {
                ModelState.AddModelError("Cuidador ya existe", "Ya hay registrado un cuidador con ese Id!");
                return BadRequest(ModelState);
            }

            if (createHerderDto == null)
            {
                return BadRequest(createHerderDto);
            }

            Herder modelHerder = _mapper.Map<Herder>(createHerderDto);

            await _herderRepository.CreateHerder(modelHerder);


            return CreatedAtRoute("GetHerder", new { id = modelHerder.Id }, modelHerder);

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
            Summary = "Actualiza datos de cuidador",
            Description = "Actualiza datos de cuidador",
            OperationId = "UpdateHerder",
            Tags = new[] { "Cuidadores" })]
        public async Task<IActionResult> UpdateHerder(int id, [FromBody] HerderUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.Id)
            {
                return BadRequest();
            }

            Herder modelHerder = _mapper.Map<Herder>(updateDto);

            await _herderRepository.UpdateHerder(modelHerder);

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
            Summary = "Elimina los datos de cuidador por su id",
            Description = "Elimina los datos de cuidador por su id",
            OperationId = "DeleteHerder",
            Tags = new[] { "Cuidadores" })]
        public async Task<IActionResult> DeleteHerder(int id)
        {
            if (id == 0)
                return BadRequest();

            var herder = await _herderRepository.GetHerderById(v => v.Id == id);

            if (herder == null)
                return NotFound();

            await _herderRepository.DeleteHerder(herder);
            return NoContent();
        }
    }
}
