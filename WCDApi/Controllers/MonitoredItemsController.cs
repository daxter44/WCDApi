﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WCDApi.DataBase.Entity;
using WCDApi.Helpers;
using WCDApi.Model.MonitoredItems;
using WCDApi.Services;

namespace WCDApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitoredItemsController: ControllerBase
    {
        private IMonitoredItemsService _itemsService;
        private IMapper _mapper;

        public MonitoredItemsController(IMonitoredItemsService itemsService,
            IMapper mapper)
        {
            _itemsService = itemsService;
            _mapper = mapper;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemsService.GetAll().ConfigureAwait(false);
            var model = _mapper.Map<IList<MonitoredItemModel>>(items);
            return Ok(model);
        }
        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<MonitoredItem>> AddMonitoredItemToCurrUser([FromBody]CreateItemModel model)
        {
            // only allow users show myDevices
            var currentUserId = Guid.Parse(User.Identity.Name);
            if (currentUserId == null)
                return Forbid();

            // map model to entity
            var item = _mapper.Map<MonitoredItem>(model);
            try
            { 
                await _itemsService.Create(currentUserId, item).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
            }
        [Authorize]
        [HttpPut("StartMonit")]
        public async Task<IActionResult> StartMonit([FromBody]MonitoredItemModel model)
        {
            // map model to entity and set id
            var item = _mapper.Map<MonitoredItem>(model);

            try
            {
                // update user 
                await _itemsService.StartMonit(item.MonitItemId).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize]
        [HttpPut("StopMonit")]
        public async Task<IActionResult> StopMonit([FromBody]MonitoredItemModel model)
        {
            // map model to entity and set id
            var item = _mapper.Map<MonitoredItem>(model);

            try
            {
                // update user 
                await _itemsService.StopMonit(item.MonitItemId).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody]MonitoredItemModel model)
        {
            // map model to entity and set id
            var item = _mapper.Map<MonitoredItem>(model);

            try
            {
                // update user 
                await _itemsService.Update(item).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize]
        [HttpPut("delete")]
        public async Task<IActionResult> Delete([FromBody]MonitoredItemModel model)
        {
            try
            {
                await _itemsService.Delete(model.MonitItemId).ConfigureAwait(false);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}