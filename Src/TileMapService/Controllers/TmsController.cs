﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace TileMapService.Controllers
{
    /// <summary>
    /// Serving tiles using Tile Map Service (TMS) protocol.
    /// </summary>
    [Route("tms")]
    public class TmsController : Controller
    {
        private readonly ITileSourceFabric tileSourceFabric;

        public TmsController(ITileSourceFabric tileSourceFabric)
        {
            this.tileSourceFabric = tileSourceFabric;
        }

        [HttpGet("")]
        public IActionResult GetCapabilitiesServices()
        {
            // TODO: services/root.xml
            var xmlDoc = new Tms.CapabilitiesDocumentBuilder(this.BaseUrl, this.tileSourceFabric).GetServices();

            return File(xmlDoc.ToUTF8ByteArray(), Utils.MediaTypeNames.Text.Xml);
        }

        [HttpGet("1.0.0")]
        public IActionResult GetCapabilitiesTileMaps()
        {
            // TODO: services/tilemapservice.xml
            var xmlDoc = new Tms.CapabilitiesDocumentBuilder(this.BaseUrl, this.tileSourceFabric).GetTileMaps();

            return File(xmlDoc.ToUTF8ByteArray(), Utils.MediaTypeNames.Text.Xml);
        }

        [HttpGet("1.0.0/{tileset}")]
        public IActionResult GetCapabilitiesTileSets(string tileset)
        {
            // TODO: services/basemap.xml
            var xmlDoc = new Tms.CapabilitiesDocumentBuilder(this.BaseUrl, this.tileSourceFabric).GetTileSets(tileset);

            return File(xmlDoc.ToUTF8ByteArray(), Utils.MediaTypeNames.Text.Xml);
        }

        /// <summary>
        /// Get tile from tileset with specified coordinates.
        /// </summary>
        /// <param name="tileset">Tileset (source) name.</param>
        /// <param name="x">Tile X coordinate (column).</param>
        /// <param name="y">Tile Y coordinate (row), Y axis goes up from the bottom.</param>
        /// <param name="z">Tile Z coordinate (zoom level).</param>
        /// <param name="extension">File extension.</param>
        /// <returns>Response with tile contents.</returns>
        [HttpGet("1.0.0/{tileset}/{z}/{x}/{y}.{extension}")]
        public async Task<IActionResult> GetTileAsync(string tileset, int x, int y, int z, string extension)
        {
            // TODO: z can be a string, not integer number
            if (String.IsNullOrEmpty(tileset) || String.IsNullOrEmpty(extension))
            {
                return BadRequest();
            }

            if (this.tileSourceFabric.Contains(tileset))
            {
                // TODO: check extension == tileset.Configuration.Format
                var tileSource = this.tileSourceFabric.Get(tileset);
                var data = await tileSource.GetTileAsync(x, y, z);
                if (data != null)
                {
                    return File(data, tileSource.Configuration.ContentType);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound($"Specified tileset '{tileset}' not found");
            }
        }

        private string BaseUrl
        {
            get
            {
                return $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            }
        }
    }
}
