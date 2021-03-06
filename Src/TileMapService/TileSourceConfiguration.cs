﻿using System.Text.Json.Serialization;

namespace TileMapService
{
    /// <summary>
    /// Represents tile source configuration and properties.
    /// </summary>
    public class TileSourceConfiguration
    {
        /// <summary>
        /// String identifier of tile source (case-sensitive).
        /// </summary>
        [JsonPropertyName("id")] // TODO: ! JsonPropertyName("...") actually ignored
        public string Id { get; set; }

        /// <summary>
        /// Name of tiles format (jpg, png).
        /// </summary>
        [JsonPropertyName("format")]
        public string Format { get; set; }

        /// <summary>
        /// User-friendly title (displayed name) of tile source.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Location of tiles in URL-like format.
        /// </summary>
        [JsonPropertyName("location")]
        public string Location { get; set; }

        /// <summary>
        /// TMS type Y coordinate (true: Y going from bottom to top; false: from top to bottom, like in OSM tiles).
        /// </summary>
        [JsonPropertyName("tms")]
        public bool? Tms { get; set; }

        [JsonIgnore]
        public string ContentType { get; set; }

        [JsonIgnore] // TODO: JsonPropertyName("minzoom")
        public int? MinZoom { get; set; }

        [JsonIgnore] // TODO: JsonPropertyName("maxzoom")
        public int? MaxZoom { get; set; }

        // TODO: bounds, center, attribution,.. (MBTiles metadata as example).
    }
}
