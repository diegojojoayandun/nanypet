using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NanyPet.Api.Models;
using NanyPet.Api.Models.Dto.User;
using NanyPet.Api.Utils;
using NanyPet.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace NanyPet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger, IMapper mapper, IConfiguration configuration)
        {
            _logger = logger;
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        /// Retrieves a list with all users
        /// </summary>
        /// <response code="200">User's list retrieved Successful</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene un listado de los usuarios registrados",
            Description = "Obtiene un listado de los usuarios registrados",
            OperationId = "GetAllUsers",
            Tags = new[] { "Usuarios" })]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            _logger.LogInformation("Obteniendo lista de usuarios"); // log -> show information on VS terminal
            IEnumerable<User> userList = await _userRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(userList));
        }

        /// <summary>
        /// Retrieves a specific User by Id
        /// </summary>
        /// <param id="1">The herder id</param>
        /// <response code="200">herder retrieved</response>
        /// <response code="400">bad request</response>
        /// <response code="404">user not found</response>
        [HttpGet("id/{id}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtiene usuario por Id",
            Description = "Obtiene usuario por Id",
            OperationId = "GetUserById",
            Tags = new[] { "Usuarios" })]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            if (id == 0)
                return BadRequest();

            var user = await _userRepository.GetById(v => v.Id == id);

            if (user == null)
            {
                _logger.LogError("No hay datos asociados a ese Id");
                return NotFound();
            }

            return Ok(_mapper.Map<UserDto>(user));

        }

        /// <summary>
        /// Retrieves a specific user by email address
        /// </summary>
        /// <param email="mail@sample.com">user's email</param>
        /// <response code="200">User retrieved</response>
        /// <response code="400">bad request</response>
        /// <response code="404">user not found</response>
        [HttpGet("email/{email}", Name = "GetUserEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtiene usuario por email",
            Description = "Obtiene usuario por email",
            OperationId = "GetUserByEmail",
            Tags = new[] { "Usuarios" })]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
        {
            if (email == null)
                return BadRequest();

            var user = await _userRepository.GetUserByEmail(v => v.Email == email);

            if (user == null)
            {
                _logger.LogError("No hay datos asociados a ese Id");
                return NotFound();
            }

            return Ok(_mapper.Map<UserDto>(user));
        }

        /// <summary>
        /// Create an User in the DB
        /// </summary>
        /// <param email="mail@sample.com">user's email</param>
        /// <response code="201">User Created</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Cannot create user</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Crea un nuevo usuario en la Base de datos",
            Description = "Crea un nuevo usuario en la Base de datos",
            OperationId = "CreateUser",
            Tags = new[] { "Usuarios" })]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserCreateDto createUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _userRepository.GetUserByEmail(v => v.Email == createUserDto.Email) != null)
            {
                ModelState.AddModelError("Usuario ya existe", "Ya existe un usuario registrado con ese Email!");
                return BadRequest(ModelState);
            }

            if (createUserDto == null)
                return BadRequest(createUserDto);

            createUserDto.Password = PasswordHasher.HashPassword(createUserDto.Password);

            User modelUser = _mapper.Map<User>(createUserDto);

            await _userRepository.Create(modelUser);


            return CreatedAtRoute("GetUser", new { id = modelUser.Id }, modelUser);

        }

        /// <summary>
        /// Update a specific Herder by unique id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param id="id" id="123">The herder id</param>
        /// <response code="200">herder retrieved</response>
        /// <response code="400">bad request</response>
        /// <response code="404">Product not found</response>
        //[HttpPut]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[SwaggerOperation(
        //    Summary = "Actualiza datos de Propietario de la mascota",
        //    Description = "Actualiza datos de Propietario de la mascota",
        //    OperationId = "UpdateOwner",
        //    Tags = new[] { "Propietarios" })]
        //public async Task<IActionResult> UpdateOwner(int id, [FromBody] OwnerUpdateDto updateDto)
        //{
        //    if (updateDto == null || id != updateDto.Id)
        //    {
        //        return BadRequest();
        //    }

        //    Owner modelOwner = _mapper.Map<Owner>(updateDto);

        //    await _ownerRepository.UpdateOwner(modelOwner);

        //    return NoContent();
        //}

        ///// <summary>
        ///// Delete a specific Herder by unique id
        ///// </summary>
        ///// <remarks>Awesomeness!</remarks>
        ///// <param id="id" id="123">The herder id</param>
        ///// <response code="200">herder retrieved</response>
        ///// <response code="400">bad request</response>
        ///// <response code="404">Product not found</response>
        //[HttpDelete("{id:int}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[SwaggerOperation(
        //    Summary = "Elimina los datos de propietario por su id",
        //    Description = "Elimina los datos de propietario por su id",
        //    OperationId = "DeleteOwner",
        //    Tags = new[] { "Propietarios" })]
        //public async Task<IActionResult> DeleteOwner(int id)
        //{
        //    if (id == 0)
        //        return BadRequest();

        //    var owner = await _ownerRepository.GetOwnerById(v => v.Id == id);

        //    if (owner == null)
        //        return NotFound();

        //    await _ownerRepository.DeleteOwner(owner);
        //    return NoContent();

        //}


    }
}