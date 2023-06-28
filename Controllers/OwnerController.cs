using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NanyPet.Api.Models;
using NanyPet.Api.Repositories.IRepository;
using NanyPet.Models.Dto.Owner;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace NanyPet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly ILogger<OwnerController> _logger;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        protected APIResponse _apiResponse;

        public OwnerController(
            IOwnerRepository ownerRepository,
            ILogger<OwnerController> logger,
            IMapper mapper)
        {
            _logger = logger;
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _apiResponse = new APIResponse();

        }

        /// <summary>
        /// Retrieves a list with all Owners registered
        /// </summary>
        /// <response code="200">Herder's list retrieved</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene un listado de todos los propietarios registrados",
            Description = "Obtiene un listado de todos los propietarios registrados",
            OperationId = "GetAllOwners",
            Tags = new[] { "Propietarios" })]
        public async Task<ActionResult<APIResponse>> GetAllOwners()
        {
            try
            {
                _logger.LogInformation("Obteniendo lista de propietarios"); // log -> show information on VS terminal
                IEnumerable<Owner> ownerList = await _ownerRepository.GetAll();
                _apiResponse.Result = _mapper.Map<IEnumerable<OwnerDto>>(ownerList);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _apiResponse;
        }

        /// <summary>
        /// Retrieves a specific Owner by unique id
        /// </summary>
        /// <param id="id">The herder id</param>
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
        public async Task<ActionResult<APIResponse>> GetOwnerById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                var owner = await _ownerRepository.GetById(v => v.Id == id);

                if (owner == null)
                {
                    _logger.LogError("No hay datos asociados a ese Id");
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_apiResponse);
                }

                _apiResponse.Result = _mapper.Map<OwnerDto>(owner);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _apiResponse;

        }

        /// <summary>
        /// Create a Owner in the database
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
        public async Task<ActionResult<APIResponse>> CreateOwner([FromBody] OwnerCreateDto createOwnerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _ownerRepository.GetByEmail(v => v.EmailUser == createOwnerDto.EmailUser) != null)
                {
                    ModelState.AddModelError("Propietario ya existe", "Ya hay registrado un propietario con ese Id!");
                    return BadRequest(ModelState);
                }

                if (createOwnerDto == null)
                {
                    return BadRequest(createOwnerDto);
                }

                Owner modelOwner = _mapper.Map<Owner>(createOwnerDto);

                await _ownerRepository.Create(modelOwner);

                _apiResponse.Result = modelOwner;
                _apiResponse.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetOwner", new { id = modelOwner.Id }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _apiResponse;
        }

        /// <summary>
        /// Update a specific Owner by unique id
        /// </summary>
        /// <param id="id">The herder id</param>
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
            try
            {
                if (updateDto == null || id != updateDto.Id)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                Owner modelOwner = _mapper.Map<Owner>(updateDto);

                await _ownerRepository.Update(modelOwner);
                _apiResponse.StatusCode = HttpStatusCode.NoContent;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_apiResponse);
        }

        /// <summary>
        /// Delete a specific Owner by unique id
        /// </summary>
        /// <param id="id">The herder id</param>
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
            try
            {
                if (id == 0)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }


                var owner = await _ownerRepository.GetById(v => v.Id == id);

                if (owner == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_apiResponse);
                }

                await _ownerRepository.Delete(owner);
                _apiResponse.StatusCode = HttpStatusCode.NoContent;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_apiResponse);
        }
    }
}
