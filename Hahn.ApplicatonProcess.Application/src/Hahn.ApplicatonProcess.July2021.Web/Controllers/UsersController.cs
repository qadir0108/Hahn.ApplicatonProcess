using AutoMapper;
using Hahn.ApplicatonProcess.July2021.Data.Entities;
using Hahn.ApplicatonProcess.July2021.Data.Remote;
using Hahn.ApplicatonProcess.July2021.Domain.Command;
using Hahn.ApplicatonProcess.July2021.Domain.Models;
using Hahn.ApplicatonProcess.July2021.Domain.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.July2021.Web.ApiControllers
{
    /// <summary>
    /// Users Controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public UsersController(IMediator mediator, IMapper mapper)
        {
            this._mediator = mediator;
            this._mapper = mapper;
        }

        // POST api/v1/<UserController>
        /// <summary>
        /// CREATE new user
        /// </summary>
        /// <param name="vm">Model to create a new user</param>
        /// <returns>Returns the created user</returns>
        /// <response code="201">Returned if the user was created</response>
        /// <response code="400">Returned if the model couldn't be parsed or the user couldn't be saved</response>
        /// <response code="422">Returned when the validation failed</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost]
        public async Task<ActionResult<UserVm>> Post([FromBody] UserVm vm)
        {
            try
            {
                var userVm = _mapper.Map<UserVm>(await _mediator.Send(new CreateUserCommand()
                {
                    User = _mapper.Map<User>(vm)
                }));
                return CreatedAtAction(nameof(GetById), new { id = userVm.Id }, userVm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: /api/<UserController>
        /// <summary>
        /// GET ALL Users
        /// </summary>
        /// <returns>Returns a list of all users</returns>
        /// <response code="200">Returned if the data loaded</response>
        /// <response code="400">Returned if the data couldn't be loaded</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<ActionResult<List<UserVm>>> Get()
        {
            try
            {
                return _mapper.Map<List<UserVm>>(await _mediator.Send(new GetUsersQuery()));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/<UserController>/5
        /// <summary>
        /// GET User by id
        /// </summary>
        /// <returns>Returns a user</returns>
        /// <response code="200">Returned if the data loaded</response>
        /// <response code="400">Returned if the data couldn't be loaded</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserVm>> GetById(int id)
        {
            try
            {
                return _mapper.Map<UserVm>(await _mediator.Send(new GetUserByIdQuery() { Id = id }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/<UserController>/5
        /// <summary>
        /// UPDATE existing user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm">Model to update an existing user</param>
        /// <returns>Returns the updated user</returns>
        /// <response code="200">Returned if the user was updated</response>
        /// <response code="400">Returned if the model couldn't be parsed or the user couldn't be found</response>
        /// <response code="422">Returned when the validation failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserVm>> Put(int id, [FromBody] UserVm vm)
        {
            try
            {
                return _mapper.Map<UserVm>(await _mediator.Send(new UpdateUserCommand()
                {
                    User = _mapper.Map<User>(vm)
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/v1/<UserController>/5
        /// <summary>
        /// DELETE user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the DELETED user</returns>
        /// <response code="200">Returned if the user was deleted</response>
        /// <response code="400">Returned if the model couldn't be parsed or the user couldn't be found</response>
        /// <response code="422">Returned when the validation failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserVm>> Delete(int id)
        {
            try
            {
                return _mapper.Map<UserVm>(await _mediator.Send(new DeleteUserCommand()
                {
                    Id = id
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// TRACK ASSET for a given user id and asset id with flag to either track or un-track
        /// </summary>
        /// <param name="vm">userId, assetId, trackUntrack</param>
        /// <remarks>trackUntrack is bool value, for tracking trackUntrack=true, it will start tracking </remarks>
        /// <returns>Returns the created user</returns>
        /// <response code="200">Returned if the assets tracked</response>
        /// <response code="400">Returned if the model couldn't be parsed or the user couldn't be saved</response>
        /// <response code="422">Returned when the validation failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPost("trackasset")]
        public async Task<ActionResult> Track([FromBody] TrackDataVm vm)
        {
            try
            {
                await _mediator.Send(new TrackAssetCommand()
                {
                    UserId = vm.userId,
                    AssetId = vm.assetId,
                    TrackUntrack = vm.trackUntrack
                });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// GET LIVE ASSETS - get the live data from the saved assets of user with given UserId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a list of assets of a user</returns>
        /// <response code="200">Returned if the data loaded</response>
        /// <response code="400">Returned if the data couldn't be loaded</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}/liveassets")]
        public async Task<ActionResult<List<RemoteAsset>>> GetLiveAssets(int id)
        {
            try
            {
                return await _mediator.Send(new GetAssetQuery() { UserId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
