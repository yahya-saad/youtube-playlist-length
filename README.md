# YouTube Playlist Calculator

A .NET Core Web API service that calculates the total length of YouTube playlists.

## Project Overview

Many people often need to know the total length of playlists to manage their time effectively before starting to watch courses or other video content. This service implements that idea using ASP.NET Core and the `Google.Apis.YouTube.v3` library.

## Features

- Calculates the total duration of a YouTube playlist.
- Supports different playback speeds.
- Provides detailed information about the playlist, including title, number of videos, average video length, and total length at various playback speeds.

## Response
![playlistcalculator](https://github.com/user-attachments/assets/001871c1-cc86-41af-9344-65de6f5ea70c)

```json
{
  "title": "C# Fundamentals",
  "numberOfVideos": 59,
  "averageLengthOfVideo": "17 minutes, 02 seconds",
  "totalLengthOfPlaylist": "16 hours, 45 minutes, 28 seconds",
  "at1_25x": "13 hours, 24 minutes, 22 seconds",
  "at1_50x": "11 hours, 10 minutes, 18 seconds",
  "at1_75x": "09 hours, 34 minutes, 33 seconds",
  "at2_00x": "08 hours, 22 minutes, 44 seconds"
}
```
