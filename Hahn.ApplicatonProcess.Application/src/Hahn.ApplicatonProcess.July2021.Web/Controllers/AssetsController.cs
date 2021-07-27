using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hahn.ApplicatonProcess.July2021.Data;
using Hahn.ApplicatonProcess.July2021.Data.Entities;
using AutoMapper;
using Hahn.ApplicatonProcess.July2021.Data.UnitOfWoork;
using Hahn.ApplicatonProcess.July2021.Domain.Models;

namespace Hahn.ApplicatonProcess.July2021.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AssetsController(IUnitOfWork uow, IMapper mapper)
        {
            this._uow = uow;
            this._mapper = mapper;
        }

        // POST: api/Assets
        /// <summary>
        /// Add New Local Asset
        /// </summary>
        /// <returns>Returns a list of all local assets</returns>
        /// <response code="201">Returned if the data loaded</response>
        /// <response code="400">Returned if the data couldn't be loaded</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AssetVm>> PostAsset(AssetVm asset)
        {
            try
            {
                var added = await _uow.AssetsRepository.AddAsync(_mapper.Map<Asset>(asset));

                await _uow.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAsset), new { id = added.Id }, _mapper.Map<AssetVm>(added));

            }
            catch (DbUpdateException)
            {
                if (await AssetExists(asset.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
        }

        // GET: api/Assets
        /// <summary>
        /// Get All Local Assets
        /// </summary>
        /// <returns>Returns a list of all local assets</returns>
        /// <response code="200">Returned if the data loaded</response>
        /// <response code="400">Returned if the data couldn't be loaded</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetVm>>> GetAssets()
        {
            return _mapper.Map<List<AssetVm>>(_uow.AssetsRepository.GetAll());
        }

        // GET: api/Assets/5
        /// <summary>
        /// Get Local Asset by Id
        /// </summary>
        /// <returns>Returns a list of all local assets</returns>
        /// <response code="200">Returned if the data loaded</response>
        /// <response code="400">Returned if the data couldn't be loaded</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult<AssetVm>> GetAsset(string id)
        {
            var asset = _mapper.Map<AssetVm>(await _uow.AssetsRepository.GetByIdAsync(id));

            if (asset == null)
            {
                return NotFound();
            }

            return asset;
        }

        // PUT: api/Assets/5
        /// <summary>
        /// Update Local Asset by Id
        /// </summary>
        /// <returns>Returns a list of all local assets</returns>
        /// <response code="200">Returned if updated successfully</response>
        /// <response code="400">Returned if the data couldn't be loaded</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<AssetVm>> PutAsset(string id, AssetVm asset)
        {
            if (id != asset.Id)
            {
                return BadRequest();
            }

            try
            {
                var updated = _uow.AssetsRepository.Update(_mapper.Map<Asset>(asset));

                await _uow.SaveChangesAsync();

                return _mapper.Map<AssetVm>(updated);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AssetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

        }

        // DELETE: api/Assets/5
        /// <summary>
        /// DELETE Asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Returned if the user was deleted</response>
        /// <response code="400">Returned if the model couldn't be parsed or the entity couldn't be found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsset(string id)
        {
            var asset = await _uow.AssetsRepository.GetByIdAsync(id);
            if (asset == null)
            {
                return NotFound();
            }

            _uow.AssetsRepository.Delete(asset);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> AssetExists(string id)
        {
            var assets = _uow.AssetsRepository.GetAll();
            return assets.Any(e => e.Id == id);
        }
    }
}
