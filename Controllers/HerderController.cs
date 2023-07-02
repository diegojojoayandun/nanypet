using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NanyPet.Api.Models;
using NanyPet.Api.Models.Specifications;
using NanyPet.Api.Repositories.IRepository;
using NanyPet.Models.Dto.Herder;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Reflection.Metadata;

namespace NanyPet.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HerderController : ControllerBase
    {
        private readonly ILogger<HerderController> _logger;
        private readonly IHerderRepository _herderRepository;
        private readonly IMapper _mapper;
        protected APIResponse _apiResponse;

        public HerderController(
            IHerderRepository herderRepository,
            ILogger<HerderController> logger,
            IMapper mapper)
        {
            _logger = logger;
            _herderRepository = herderRepository;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }


        /// <summary>
        /// Retieves a List with all registered herders
        /// </summary>
        /// <response code="200">Herder's list retrieved</response>
        [HttpGet]
        [ResponseCache(Duration = 60)]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(
            Summary = "Obtiene un listado de todos los cuidadores registrados",
            Description = "Obtiene un listado de todos los cuidadores registrados",
            OperationId = "GetAllHerders",
            Tags = new[] { "Cuidadores" })]
        public async Task<ActionResult<APIResponse>> GetAllHerders()
        {
            try
            {
                _logger.LogInformation("Obteniendo lista de cuidadores"); // log -> show information on VS terminal
                IEnumerable<Herder> herderList = await _herderRepository.GetAll();
                _apiResponse.Result = _mapper.Map<IEnumerable<HerderDto>>(herderList);
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
        /// Retieves a List with all registered herders paginated
        /// </summary>
        /// <response code="200">Paginated Herder's list retrieved</response>
        [HttpGet("HerderPaginated")]
        [ResponseCache(Duration = 60)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(
            Summary = "Obtiene un listado de todos los cuidadores registrados",
            Description = "Obtiene un listado de todos los cuidadores registrados",
            OperationId = "GetAllHerdersPaginated",
            Tags = new[] { "Cuidadores" })]
        public ActionResult<APIResponse> GetAllHerdersPaginated([FromQuery] Parameters parameters)
        {
            try
            {
                _logger.LogInformation("Obteniendo lista de cuidadores"); // log -> show information on VS terminal
                var herderList =  _herderRepository.GetAllPaginated(parameters);
                _apiResponse.Result = _mapper.Map<IEnumerable<HerderDto>>(herderList);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.TotalPages = herderList.MetaData.TotalPages;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { ex.Message.ToString() };
            }

            return _apiResponse;

        }


        /// <summary>
        /// Retrieves a specific Herder by unique id
        /// </summary>
        /// <param id="herder id">The herder id</param>
        /// <response code="200">herder retrieved</response>
        /// <response code="400">bad request</response>
        /// <response code="404">herder not found</response>
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
        public async Task<ActionResult<APIResponse>> GetHerdersById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al buscar con Id " + id);
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                var herder = await _herderRepository.GetById(v => v.Id == id);

                if (herder == null)
                {
                    _logger.LogError("No hay datos asociados a ese Id " + id);
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_apiResponse);
                }

                _apiResponse.Result = _mapper.Map<HerderDto>(herder);
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
        /// Retrieves a specific Herder by unique email
        /// </summary>
        /// <param email="user@example.com">The herder email</param>
        /// <response code="200">herder retrieved</response>
        /// <response code="400">bad request</response>
        /// <response code="404">herder not found</response>
        [HttpGet("email/{email}", Name = "GetByMail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtiene cuidador por mail",
            Description = "Obtiene cuidador por mail",
            OperationId = "GetHerderByEmail",
            Tags = new[] { "Cuidadores" })]

        public async Task<ActionResult<APIResponse>> GetUserByEmail(string email)
        {
            try
            {
                if (email == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                var herder = await _herderRepository.GetByEmail(v => v.EmailUser == email);

                if (herder == null)
                {
                    _logger.LogError("No hay datos asociados al Correo " + email);
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_apiResponse);
                }

                _apiResponse.Result = _mapper.Map<HerderDto>(herder);
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
        public async Task<ActionResult<APIResponse>> CreateHerder([FromBody] HerderCreateDto createHerderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _herderRepository.GetByEmail(v => v.EmailUser == createHerderDto.EmailUser) != null)
                {
                    ModelState.AddModelError("Error Usuario", "No hay usuario asociado a ese email!");
                    return BadRequest(ModelState);
                }

                if (createHerderDto == null)
                {
                    return BadRequest(createHerderDto);
                }

                Herder modelHerder = _mapper.Map<Herder>(createHerderDto);

                await _herderRepository.Create(modelHerder);
                _apiResponse.Result = modelHerder;
                _apiResponse.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetHerder", new { id = modelHerder.Id }, _apiResponse);

            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { ex.Message.ToString() };
            }

            return _apiResponse;
        }

        /// <summary>
        /// Update a specific Herder by unique id
        /// </summary>
        /// <param id="herder Id">The herder id</param>
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
            try
            {
                if (updateDto == null || id != updateDto.Id)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                Herder modelHerder = _mapper.Map<Herder>(updateDto);

                await _herderRepository.Update(modelHerder);
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
        /// Delete a specific Herder by unique id
        /// </summary>
        /// <param id="herder Id">The herder id</param>
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
            try
            {
                if (id == 0)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                var herder = await _herderRepository.GetById(v => v.Id == id);

                if (herder == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_apiResponse);
                }

                await _herderRepository.Delete(herder);

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
