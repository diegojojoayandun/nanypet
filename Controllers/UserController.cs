using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NanyPet.Api.Models;
using NanyPet.Api.Models.Dto.Login;
using NanyPet.Api.Repositories.IRepository;
using NanyPet.Api.Utils;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace NanyPet.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        protected APIResponse _apiResponse;

        public UserController(
            IUserRepository userRepository, 
            IMapper mapper, 
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
            _apiResponse = new APIResponse();
        }

        /// <summary>
        /// Login for a registered user
        /// </summary>
        /// <response code=200>Successful login</response>
        /// <response code=400>Bad Request</response>
        /// <response code=500>Internal Server Error</response>
        [HttpPost("signin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Login for a registered user",
            Description = "Login for a registered user",
            OperationId = "signin",
            Tags = new[] { "Usuarios" })]
        public async Task<IActionResult> SignIn([FromBody] LoginRequestDTO model)
        { 
            var loginResponse = await _userRepository.SignIn(model);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            { 
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("Usuario o Password son Icorrectos!");
                return BadRequest(_apiResponse);
            }
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.Result = loginResponse;
            return Ok(_apiResponse);
        }

        /// <summary>
        /// User Registration
        /// </summary>
        /// <response code=200>Successful login</response>
        /// <response code=400>Bad Request</response>
        /// <response code=500>Internal Server Error</response>
        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "User Registration",
            Description = "User Registration",
            OperationId = "signup",
            Tags = new[] { "Usuarios" })]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequestDTO model)
        {
            bool isUniqueUser = _userRepository.IsUniqueUser(model.Email);

            if (!isUniqueUser) 
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("Usuario ya existe!");
                return BadRequest(_apiResponse);
            }
            var user = await _userRepository.SignUp(model);

            if (user == null) 
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("Error al registrar usuario!");
                return BadRequest(_apiResponse);
            }
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }



        ///// <summary>
        ///// Retrieves a list with all users
        ///// </summary>
        ///// <response code="200">User's list retrieved Successful</response>
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[SwaggerOperation(
        //    Summary = "Obtiene un listado de los usuarios registrados",
        //    Description = "Obtiene un listado de los usuarios registrados",
        //    OperationId = "GetAllUsers",
        //    Tags = new[] { "Usuarios" })]
        //public async Task<ActionResult<APIResponse>> GetAllUsers()
        //{
        //    try
        //    {
        //        _logger.LogInformation("Obteniendo lista de usuarios");
        //        IEnumerable<User> userList = await _userRepository.GetAll();
        //        _apiResponse.Result = _mapper.Map<IEnumerable<UserDto>>(userList);
        //        _apiResponse.StatusCode = HttpStatusCode.OK;
        //        return Ok(_apiResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        _apiResponse.IsSuccess = false;
        //        _apiResponse.ErrorMessages = new List<string> { ex.ToString() };
        //    }
        //    return _apiResponse;
        //}

        ///// <summary>
        ///// Retrieves a specific User by Id
        ///// </summary>
        ///// <param id="1">The User id</param>
        ///// <response code="200">herder retrieved</response>
        ///// <response code="400">bad request</response>
        ///// <response code="404">user not found</response>
        //[HttpGet("id/{id}", Name = "GetUser")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[SwaggerOperation(
        //    Summary = "Obtiene usuario por Id",
        //    Description = "Obtiene usuario por Id",
        //    OperationId = "GetUserById",
        //    Tags = new[] { "Usuarios" })]
        //public async Task<ActionResult<APIResponse>> GetUserById(int id)
        //{
        //    try
        //    {
        //        if (id == 0)
        //        {
        //            _apiResponse.IsSuccess = false;
        //            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
        //            return BadRequest(_apiResponse);
        //        }

        //        var user = await _userRepository.GetById(v => v.Id == id);

        //        if (user == null)
        //        {
        //            _logger.LogError("No hay datos asociados a ese Id");
        //            _apiResponse.IsSuccess = false;
        //            _apiResponse.StatusCode = HttpStatusCode.NotFound;
        //            return NotFound(_apiResponse);
        //        }

        //        _apiResponse.Result = _mapper.Map<UserDto>(user);
        //        _apiResponse.StatusCode = HttpStatusCode.OK;
        //        return Ok(_apiResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        _apiResponse.IsSuccess = false;
        //        _apiResponse.ErrorMessages = new List<string> { ex.Message };
        //    }
        //    return _apiResponse;

        //}

        ///// <summary>
        ///// Retrieves a specific user by email address
        ///// </summary>
        ///// <param email="mail@sample.com">user's email</param>
        ///// <response code="200">User retrieved</response>
        ///// <response code="400">bad request</response>
        ///// <response code="404">user not found</response>
        //[HttpGet("email/{email}", Name = "GetUserEmail")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[SwaggerOperation(
        //    Summary = "Obtiene usuario por email",
        //    Description = "Obtiene usuario por email",
        //    OperationId = "GetUserByEmail",
        //    Tags = new[] { "Usuarios" })]
        //public async Task<ActionResult<APIResponse>> GetUserByEmail(string email)
        //{
        //    try
        //    {
        //        if (email == null)
        //        {
        //            _apiResponse.IsSuccess = false;
        //            _apiResponse.StatusCode = HttpStatusCode.BadRequest;
        //            return BadRequest(_apiResponse);
        //        }

        //        var user = await _userRepository.GetUserByEmail(v => v.Email == email);

        //        if (user == null)
        //        {
        //            _logger.LogError("No hay datos asociados a ese Id");
        //            _apiResponse.IsSuccess = false;
        //            _apiResponse.StatusCode = HttpStatusCode.NotFound;
        //            return NotFound(_apiResponse);
        //        }

        //        _apiResponse.Result = _mapper.Map<UserDto>(user);
        //        _apiResponse.StatusCode = HttpStatusCode.OK;
        //        return Ok(_apiResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        _apiResponse.IsSuccess = false;
        //        _apiResponse.ErrorMessages = new List<string> { ex.ToString() };
        //    }
        //    return _apiResponse;
        //}

        ///// <summary>
        ///// Create an User in the DB
        ///// </summary>
        ///// <param email="mail@sample.com">user's email</param>
        ///// <response code="201">User Created</response>
        ///// <response code="400">Bad request</response>
        ///// <response code="500">Cannot create user</response>
        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[SwaggerOperation(
        //    Summary = "Crea un nuevo usuario en la Base de datos",
        //    Description = "Crea un nuevo usuario en la Base de datos",
        //    OperationId = "CreateUser",
        //    Tags = new[] { "Usuarios" })]
        //public async Task<ActionResult<APIResponse>> CreateUser([FromBody] UserCreateDto createUserDto)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //            return BadRequest(ModelState);

        //        if (await _userRepository.GetUserByEmail(v => v.Email == createUserDto.Email) != null)
        //        {
        //            ModelState.AddModelError("Usuario ya existe", "Ya existe un usuario registrado con email " + createUserDto.Email);
        //            return BadRequest(ModelState);
        //        }

        //        if (createUserDto == null)
        //            return BadRequest(createUserDto);

        //        createUserDto.Password = PasswordHasher.HashPassword(createUserDto.Password);

        //        User modelUser = _mapper.Map<User>(createUserDto);

        //        await _userRepository.Create(modelUser);

        //        _apiResponse.Result = modelUser;
        //        _apiResponse.StatusCode = HttpStatusCode.Created;
        //        return CreatedAtRoute("GetUser", new { id = modelUser.Id }, _apiResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        _apiResponse.IsSuccess = false;
        //        _apiResponse.ErrorMessages = new List<string> { ex.ToString() };
        //    }
        //    return _apiResponse;
        //}

        ///// <summary>
        ///// Update a specific Herder by unique id
        ///// </summary>
        ///// <param id="id">The User id</param>
        ///// <response code="200">herder retrieved</response>
        ///// <response code="400">bad request</response>
        ///// <response code="404">User not found</response>
        ////[HttpPut]
        ////[ProducesResponseType(StatusCodes.Status204NoContent)]
        ////[ProducesResponseType(StatusCodes.Status400BadRequest)]
        ////[ProducesResponseType(StatusCodes.Status404NotFound)]
        ////[SwaggerOperation(
        ////    Summary = "Actualiza datos de Usuario",
        ////    Description = "Actualiza datos de Usario",
        ////    OperationId = "UpdateUser",
        ////    Tags = new[] { "Usuarios" })]
        ////public async Task<IActionResult> UpdateOwner(int id, [FromBody] OwnerUpdateDto updateDto)
        ////{
        ////    if (updateDto == null || id != updateDto.Id)
        ////    {
        ////        return BadRequest();
        ////    }

        ////    Owner modelOwner = _mapper.Map<Owner>(updateDto);

        ////    await _ownerRepository.UpdateOwner(modelOwner);

        ////    return NoContent();
        ////}

        /////// <summary>
        /////// Delete a specific Herder by unique id
        /////// </summary>
        /////// <remarks>Awesomeness!</remarks>
        /////// <param id="id" id="123">The herder id</param>
        /////// <response code="200">herder retrieved</response>
        /////// <response code="400">bad request</response>
        /////// <response code="404">Product not found</response>
        ////[HttpDelete("{id:int}")]
        ////[ProducesResponseType(StatusCodes.Status204NoContent)]
        ////[ProducesResponseType(StatusCodes.Status400BadRequest)]
        ////[ProducesResponseType(StatusCodes.Status404NotFound)]
        ////[SwaggerOperation(
        ////    Summary = "Elimina los datos de propietario por su id",
        ////    Description = "Elimina los datos de propietario por su id",
        ////    OperationId = "DeleteOwner",
        ////    Tags = new[] { "Propietarios" })]
        ////public async Task<IActionResult> DeleteOwner(int id)
        ////{
        ////    if (id == 0)
        ////        return BadRequest();

        ////    var owner = await _ownerRepository.GetOwnerById(v => v.Id == id);

        ////    if (owner == null)
        ////        return NotFound();

        ////    await _ownerRepository.DeleteOwner(owner);
        ////    return NoContent();

        ////}


    }
}