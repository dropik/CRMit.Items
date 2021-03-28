using CRMit.Items.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRMit.Items.Controllers
{
    [Route("crmit/v1/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemsDbContext context;

        public ItemsController(ItemsDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Create a new item.
        /// </summary>
        /// <param name="item">Item data to be used for new item.</param>
        /// <response code="201">Item created and standard Created response returned with new item.</response>
        [HttpPost("", Name = "CreateItem")]
        [ProducesResponseType(typeof(Item), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Item>> CreateItemAsync([FromBody, Required] ItemInput item)
        {
            var newItem = new Item(item);

            try
            {
                await context.AddAsync(newItem);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetItem", new { id = newItem.Id }, newItem);
        }

        /// <summary>
        /// Get a list containing all items.
        /// </summary>
        /// <response code="200">List with all items.</response>
        [HttpGet("", Name = "GetItemsList")]
        [ProducesResponseType(typeof(IEnumerable<Item>), 200)]
        public async Task<ActionResult<IEnumerable<Item>>> GetListAsync()
        {
            var item = await context.Items.ToListAsync();
            return new JsonResult(item);
        }

        /// <summary>
        /// Get an item by id.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <response code="200">Found item.</response>
        [HttpGet("{id}", Name = "GetItem")]
        [ProducesResponseType(typeof(Item), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Item>> GetItemAsync(long id)
        {
            var result = await context.FindAsync<Item>(id);
            if (result == null)
            {
                return NotFound();
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Update an item given id.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="item">Item data to update with.</param>
        /// <response code="200">Item updated.</response>
        [HttpPut("{id}", Name = "EditItem")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateItemAsync(long id, [FromBody, Required] ItemInput item)
        {
            if (context.Items.Any(c => c.Id == id))
            {
                var updatedItem = new Item(item) { Id = id };
                try
                {
                    context.Update(updatedItem);
                    await context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            else
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        /// Delete an item given id.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <response code="200">Item deleted.</response>
        [HttpDelete("{id}", Name = "DeleteItem")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteItemAsync(long id)
        {
            var item = await context.FindAsync<Item>(id);
            if (item == null)
            {
                return NotFound();
            }

            context.Remove(item);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
