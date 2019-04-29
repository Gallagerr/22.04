using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _22._04
{
  class MusicManager
  {
    const int MIN_RATING_SONG = 0;
    const int MAX_RATING_SONG = 10;

    List<Band> bands;
    List<Song> songs;

    public void Run()
    {
      bands = new List<Band>();
      songs = new List<Song>();

      bool flag = true;
      while (flag)
      {
        switch (MainMenu())
        {
          case 1:
            {
              AddGroup();
              break;
            }
          case 2:
            {
              AddSong();
              break;
            }
          case 3:
            {
              SearchBand();
              break;
            }
          case 4:
            {
              SearchSong();
              break;
            }
          case 5:
            {
              PrintSongsByRating();
              break;
            }
          case 6:
            {
              DeleteGroup();
              break;
            }
          case 7:
            {
              DeleteSong();
              break;
            }
        }
      }
    }
    private int MainMenu()
    {
      using (var context = new MusicContext())
      {
        bands = context.Bands.ToList();
        songs = context.Songs.ToList();
      }

      Console.WriteLine("Select an action: \n" + "1) Add a group\n" + "2) Add a song\n" + "3) Search by group\n"
          + "4) Search by song\n" + "5) Post songs by rating\n" + "6) Delete group\n" + "7) Delete song");
      if (int.TryParse(Console.ReadLine(), out int menu))
      {
        if (menu > 0 && menu <= 7)
        {
          return menu;
        }
      }
      return -1;
    }
    private void AddGroup()
    {
      string groupName;
      Console.WriteLine("Enter group name: ");
      groupName = Console.ReadLine();

      if (!string.IsNullOrWhiteSpace(groupName))
      {
        Band newBand = new Band();
        newBand.Name = groupName;

        using (var context = new MusicContext())
        {
          bands.Add(newBand);
          context.Bands.Add(newBand);
          context.SaveChanges();
        }
      }
      else
      {
        Console.WriteLine("Empty group name!");
      }
    }
    private void AddSong()
    {
      string songName, songWordsString, songBandName;
      long songDuration;
      int songRating;
      List<string> songWords = new List<string>();
      Console.WriteLine("Enter the name of the song: ");
      songName = Console.ReadLine();

      if (!string.IsNullOrWhiteSpace(songName))
      {

        if (bands.Count > 0)
        {
          Song newSong = new Song();
          newSong.Name = songName;
          Console.WriteLine("Enter the length of the song(in seconds): ");
          if (long.TryParse(Console.ReadLine(), out songDuration))
          {
            if (songDuration > 0)
            {
              newSong.DurationSeconds = songDuration;
              Console.WriteLine("Enter song rating: ");

              if (int.TryParse(Console.ReadLine(), out songRating))
              {
                if (songRating >= MIN_RATING_SONG && songRating <= MAX_RATING_SONG)
                {
                  newSong.Rating = songRating;

                  Console.WriteLine("Add words: ");
                  songWordsString = Console.ReadLine();
                  string[] words = songWordsString.Split(' ');

                  foreach (var word in words)
                  {
                    songWords.Add(word);
                  }
                  newSong.Words = songWords;

                  Console.WriteLine("Enter group name: ");
                  foreach (var band in bands)
                  {
                    Console.WriteLine($"{band.Name}");
                  }
                  songBandName = Console.ReadLine();

                  if (bands.Contains(bands.Where(band => band.Name.ToUpper() == songBandName.ToUpper()).FirstOrDefault()))
                  {
                    using (var context = new MusicContext())
                    {
                      newSong.Band = context.Bands.Where(band => band.Name.ToUpper() == songBandName.ToUpper()).FirstOrDefault();
                      context.Songs.Add(newSong);
                      context.SaveChanges();
                    }
                  }
                  else
                  {
                    Console.WriteLine("No such group!");
                  }
                }
                else
                {
                  Console.WriteLine($"Rating must be between {MIN_RATING_SONG} and {MAX_RATING_SONG} inclusive");
                }
              }
              else
              {
                Console.WriteLine("Wrong format!");
              }
            }
            else
            {
              Console.WriteLine("The song can not last less than 1 second!");
            }
          }
          else
          {
            Console.WriteLine("Wrong format!");
          }
        }
        else
        {
          Console.WriteLine("First you need to add groups!");
        }

      }
      else
      {
        Console.WriteLine("Empty song name!");
      }
    }

    private void SearchBand()
    {
      Console.WriteLine("Enter the name of the group you want to search.: ");
      string bandName = Console.ReadLine();

      if (!string.IsNullOrWhiteSpace(bandName))
      {
        List<Band> finded;
        finded = bands.FindAll(band => band.Name.ToLower() == bandName.ToLower());

        Console.WriteLine($"Found {finded.Count} groups: ");
        foreach (var band in finded)
        {
          band.Print();
          Console.WriteLine();
        }
      }
      else
      {
        Console.WriteLine("Empty string!");
      }
    }
    private void SearchSong()
    {
      Console.WriteLine("Enter the name of the song you want to find.: ");
      string songName = Console.ReadLine();

      if (!string.IsNullOrWhiteSpace(songName))
      {
        List<Song> finded;
        finded = songs.FindAll(song => song.Name.ToLower() == songName.ToLower());

        Console.WriteLine($"Found {finded.Count} songs");
        foreach (var song in finded)
        {
          song.Print();
          Console.WriteLine();
        }
      }
      else
      {
        Console.WriteLine("Empty line!");
      }
    }

    private void PrintSongsByRating()
    {
      Console.WriteLine("Descending Enter 0");
      Console.WriteLine("Ascending other");
      string style = Console.ReadLine();

      songs.Sort(new SongComparer());

      if (int.TryParse(style, out int dumpy))
      {
        if (dumpy == 0)
        {
          foreach (var song in songs)
          {
            song.Print();
            Console.WriteLine();
            return;
          }
        }
      }

      foreach (var song in songs)
      {
        for (int i = songs.Count - 1; i >= 0; i++)
        {
          songs[i].Print();
          Console.WriteLine();
        }
      }
    }
    private void DeleteGroup()
    {
      foreach (var band in bands)
      {
        band.Print();
        Console.WriteLine();
      }
      Console.WriteLine("Enter group name: ");
      string groupName = Console.ReadLine();

      if (!string.IsNullOrWhiteSpace(groupName))
      {
        if (bands.Contains(bands.Where(band => band.Name.ToUpper() == groupName.ToUpper()).FirstOrDefault()))
        {
          using (var context = new MusicContext())
          {
            var bandToDelete = context.Bands.Where(band => band.Name.ToUpper() == groupName.ToUpper()).FirstOrDefault();
            context.Entry(bandToDelete).Collection(s => s.Songs).Load();
            context.Bands.Remove(bandToDelete);
            context.SaveChanges();
          }
        }
        else
        {
          Console.WriteLine("There is no such group!");
        }
      }
      else
      {
        Console.WriteLine("Empty line!");
      }
    }
    private void DeleteSong()
    {
      foreach (var song in songs)
      {
        song.Print();
        Console.WriteLine();
      }

      Console.WriteLine("Введите название песни: ");
      string songName = Console.ReadLine();

      if (!string.IsNullOrWhiteSpace(songName))
      {
        if (songs.Contains(songs.Where(song => song.Name.ToUpper() == songName.ToUpper()).FirstOrDefault()))
        {
          using (var context = new MusicContext())
          {
            var songToDelete = context.Songs.Where(song => song.Name.ToUpper() == songName.ToUpper()).FirstOrDefault();
            context.Songs.Remove(songToDelete);
            context.SaveChanges();
          }
        }
        else
        {
          Console.WriteLine("There is no such song!");
        }
      }
      else
      {
        Console.WriteLine("Empty line!");
      }
    }
  }
}
