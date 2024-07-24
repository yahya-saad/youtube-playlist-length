using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

[ApiController]
[Route("[controller]")]
public class PlaylistController : ControllerBase
{
    private readonly YouTubeApiService _youTubeService;

    public PlaylistController(YouTubeApiService youTubeService)
    {
        _youTubeService = youTubeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlaylistDuration(string playlistUrl)
    {
        var playlistId = ExtractPlaylistIdFromUrl(playlistUrl);
        if (string.IsNullOrEmpty(playlistId))
        {
            return BadRequest("Invalid playlist URL.");
        }

        var playlist = await _youTubeService.GetPlaylistAsync(playlistId);
        if (playlist == null)
        {
            return NotFound("Playlist not found.");
        }

        var response = new
        {
            Title = playlist.Title,
            NumberOfVideos = playlist.Videos.Count,
            AverageLengthOfVideo = FormatTimeSpan(playlist.AverageVideoDuration),
            TotalLengthOfPlaylist = FormatTimeSpan(playlist.TotalDuration),
            At1_25x = FormatTimeSpan(playlist.GetDurationAtSpeed(1.25)),
            At1_50x = FormatTimeSpan(playlist.GetDurationAtSpeed(1.50)),
            At1_75x = FormatTimeSpan(playlist.GetDurationAtSpeed(1.75)),
            At2_00x = FormatTimeSpan(playlist.GetDurationAtSpeed(2.00))
        };

        return Ok(response);
    }

    private string ExtractPlaylistIdFromUrl(string url)
    {
        var regex = new Regex(@"[?&]list=([^&]+)");
        var match = regex.Match(url);
        return match.Success ? match.Groups[1].Value : null;
    }

    private string FormatTimeSpan(TimeSpan timeSpan)
    {
        if (timeSpan.Hours > 0)
        {
            return string.Format("{0:D2} hours, {1:D2} minutes, {2:D2} seconds",
                timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
        else
        {
            return string.Format("{0:D2} minutes, {1:D2} seconds",
                timeSpan.Minutes, timeSpan.Seconds);
        }
    }

}
