// Services/YouTubeApiService.cs
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using PlaylistLengthCalculator.Models;
using System.Xml;

public class YouTubeApiService
{
    private readonly YouTubeService _youTubeService;

    public YouTubeApiService(string apiKey)
    {
        _youTubeService = new YouTubeService(new BaseClientService.Initializer
        {
            ApiKey = apiKey,
        });
    }

    public async Task<Playlist> GetPlaylistAsync(string playlistId)
    {
        var playlistRequest = _youTubeService.Playlists.List("snippet");
        playlistRequest.Id = playlistId;

        var playlistResponse = await playlistRequest.ExecuteAsync();

        var playlist = new Playlist
        {
            Id = playlistId,
            Title = playlistResponse.Items?.FirstOrDefault()?.Snippet?.Title ?? "Unknown Playlist",
            Videos = new List<Video>()
        };

        var nextPageToken = string.Empty;
        while (nextPageToken != null)
        {
            var playlistItemsRequest = _youTubeService.PlaylistItems.List("snippet,contentDetails");
            playlistItemsRequest.PlaylistId = playlistId;
            playlistItemsRequest.MaxResults = 100;
            playlistItemsRequest.PageToken = nextPageToken;

            var playlistItemsResponse = await playlistItemsRequest.ExecuteAsync();


            var videoIds = playlistItemsResponse.Items?
                .Where(item => item.ContentDetails?.VideoId != null)
                .Select(item => item.ContentDetails.VideoId)
                .ToList() ?? new List<string>();

            if (videoIds.Any())
            {
                var videosRequest = _youTubeService.Videos.List("contentDetails");
                videosRequest.Id = string.Join(",", videoIds);

                var videosResponse = await videosRequest.ExecuteAsync();
                foreach (var video in videosResponse.Items)
                {
                    if (video.ContentDetails != null)
                    {
                        var duration = XmlConvert.ToTimeSpan(video.ContentDetails.Duration);
                        playlist.Videos.Add(new Video
                        {
                            Title = video.Snippet?.Title ?? "Unknown Title",
                            Duration = duration
                        });
                    }
                }
            }

            nextPageToken = playlistItemsResponse.NextPageToken;
        }

        return playlist;
    }
}
