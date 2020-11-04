using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Assignment5.Models;
using System.Security.Claims;

namespace Assignment5.Controllers
{
    public class Manager
    {
        // Reference to the data context
        private ApplicationDbContext ds = new ApplicationDbContext();

        // AutoMapper instance
        public IMapper mapper;

        // Request user property...

        // Backing field for the property
        private RequestUser _user;

        // Getter only, no setter
        public RequestUser User
        {
            get
            {
                // On first use, it will be null, so set its value
                if (_user == null)
                {
                    _user = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);
                }
                return _user;
            }
        }

        // Default constructor...
        public Manager()
        {
            // If necessary, add constructor code here

            // Configure the AutoMapper components
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Album, AlbumBaseViewModel>();
                cfg.CreateMap<Album, AlbumWithDetailViewModel>();
                cfg.CreateMap<Album, AlbumAddFormViewModel>();
                cfg.CreateMap<AlbumAddViewModel, Album>();

                cfg.CreateMap<Artist, ArtistBaseViewModel>();
                cfg.CreateMap<Artist, ArtistWithDetailViewModel>();
                cfg.CreateMap<Artist, ArtistWithMediaViewModel>();
                cfg.CreateMap<ArtistAddViewModel, Artist>();

                cfg.CreateMap<Track, TrackBaseViewModel>();
                cfg.CreateMap<Track, TrackWithDetailsViewModel>();
                cfg.CreateMap<TrackAddViewModel, Track>();
                cfg.CreateMap<Track, TrackAudioViewModel>();

                cfg.CreateMap<MediaItem, MediaItemBaseViewModel>();
                cfg.CreateMap<MediaItem, MediaItemWithDetailViewModel>();
                cfg.CreateMap<MediaItemAddViewModel, MediaItem>();


                cfg.CreateMap<Genre, GenreBaseViewModel>();

                cfg.CreateMap<Models.RegisterViewModel, Models.RegisterViewModelForm>();
            });

            mapper = config.CreateMapper();

            // Turn off the Entity Framework (EF) proxy creation features
            // We do NOT want the EF to track changes - we'll do that ourselves
            ds.Configuration.ProxyCreationEnabled = false;

            // Also, turn off lazy loading...
            // We want to retain control over fetching related objects
            ds.Configuration.LazyLoadingEnabled = false;
        }

        // ############################################################
        // RoleClaim

        public List<string> RoleClaimGetAllStrings()
        {
            return ds.RoleClaims.OrderBy(r => r.Name).Select(r => r.Name).ToList();
        }



        //GENRE

        public IEnumerable<GenreBaseViewModel> GetAllGenres()
        {
            var obj = ds.Genres.OrderBy(c => c.Name);
            return mapper.Map<IEnumerable<Genre>, IEnumerable<GenreBaseViewModel>>(obj);
        }



        //ARTIST

        public IEnumerable<ArtistBaseViewModel> GetAllArtists()
        {
            var obj = ds.Artists.OrderBy(c => c.Name);
            return mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistBaseViewModel>>(obj);
        }

        public ArtistWithMediaViewModel ArtistGetById(int id)
        {
            var o = ds.Artists.Include("Albums").Include("MediaItems").SingleOrDefault(p => p.Id == id);
            return (o == null) ? null : mapper.Map<Artist, ArtistWithMediaViewModel>(o);
        }

        public ArtistBaseViewModel AddNewArtist(ArtistAddViewModel newArtist)
        {
            var addedArtist = ds.Artists.Add(mapper.Map<ArtistAddViewModel, Artist>(newArtist));

            addedArtist.Executive = User.Name;

            ds.SaveChanges();
            return mapper.Map<Artist, ArtistBaseViewModel>(addedArtist);
        }


        //ALBUM

        public IEnumerable<AlbumBaseViewModel> GetAllAlbums()
        {
            var obj = ds.Albums.OrderBy(c => c.Name);
            return mapper.Map<IEnumerable<Album>, IEnumerable<AlbumBaseViewModel>>(obj);
        }

        public AlbumWithDetailViewModel AlbumGetById(int id)
        {
            var obj = ds.Albums.Include("Artists").Include("Tracks").SingleOrDefault(t => t.Id == id);
            return obj == null ? null : mapper.Map<Album, AlbumWithDetailViewModel>(obj);
        }

        public AlbumWithDetailViewModel AlbumAdd(AlbumAddViewModel newItem)
        {
            var artistList = new List<Artist>();
            var trackList = new List<Track>();
            var addedItem = ds.Albums.Add(mapper.Map<AlbumAddViewModel, Album>(newItem));

            if (newItem.ArtistIds != null)
            {
                foreach (var artistIds in newItem.ArtistIds)
                {
                    var obj = ds.Artists.Find(artistIds);
                    if (obj != null)
                    {
                        artistList.Add(obj);
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            if (newItem.TrackIds != null)
            {
                foreach (var trackIds in newItem.TrackIds)
                {
                    var obj = ds.Tracks.Find(trackIds);
                    if (obj != null)
                    {
                        trackList.Add(obj);
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            foreach (var item in newItem.ArtistIds)
            {
                var obj = ds.Artists.Find(item);
                addedItem.Artists.Add(obj);
                obj.Albums.Add(addedItem);
            }

            foreach (var item in newItem.TrackIds)
            {
                var obj = ds.Tracks.Find(item);
                addedItem.Tracks.Add(obj);
                obj.Albums.Add(addedItem);
            }

            addedItem.Artists = artistList;
            addedItem.Tracks = trackList;
            addedItem.Coordinator = User.Name;

            ds.SaveChanges();

            return mapper.Map<Album, AlbumWithDetailViewModel>(addedItem);

        }


        //TRACKS

        public IEnumerable<TrackBaseViewModel> GetAllTracks()
        {
            var obj = ds.Tracks.OrderBy(c => c.Name);
            return mapper.Map<IEnumerable<Track>, IEnumerable<TrackBaseViewModel>>(obj);
        }



        public TrackWithDetailsViewModel GetTrackById(int id)
        {
            var obj = ds.Tracks.Include("Albums.Artists").SingleOrDefault(c => c.Id == id);

            if (obj == null)
            {
                return null;
            }
            else
            {
                var result = mapper.Map<Track, TrackWithDetailsViewModel>(obj);
                result.AlbumNames = obj.Albums.Select(c => c.Name);
                return result;

            }
        }


        public TrackWithDetailsViewModel AddNewTrack(TrackAddViewModel temp, int id)
        {
            var album = ds.Albums.Find(id);
            var newItem = ds.Tracks.Add(mapper.Map<TrackAddViewModel, Track>(temp));

            newItem.Albums.Add(album);
            newItem.Clerk = User.Name;
            newItem.AudioContentType = temp.ClipUpload.ContentType;

            byte[] foo = new byte[temp.ClipUpload.ContentLength];
            temp.ClipUpload.InputStream.Read(foo, 0, temp.ClipUpload.ContentLength);
            newItem.Audio = foo;
            ds.SaveChanges();
            return (newItem == null) ? null : mapper.Map<Track, TrackWithDetailsViewModel>(newItem);
        }
        


        public IEnumerable<TrackBaseViewModel> TrackGetAllByArtistId(int id)
        {
            var o = ds.Artists.Include("Albums.Tracks").SingleOrDefault(a => a.Id == id);

            if (o == null) { return null; }

            var c = new List<Track>();

            foreach (var album in o.Albums)
            {
                c.AddRange(album.Tracks);
            }
            c = c.Distinct().ToList();

            return mapper.Map<IEnumerable<Track>, IEnumerable<TrackBaseViewModel>>
                (c.OrderBy(t => t.Name));

        }


        // TRACK AUDIO

        public TrackAudioViewModel TrackAudioGetById(int id)
        {
            var o = ds.Tracks.Find(id);

            return (o == null) ? null : mapper.Map<Track, TrackAudioViewModel>(o);
        }


        public TrackWithDetailsViewModel TrackEdit(TrackEditViewModel newItem)
        {

            var o = ds.Tracks.Find(newItem.Id);

            if (o == null)
            {
                return null;
            }
            else
            {

                o.AudioContentType = newItem.TrackUpload.ContentType;
                byte[] audio = new byte[newItem.TrackUpload.ContentLength];
                newItem.TrackUpload.InputStream.Read(audio, 0, newItem.TrackUpload.ContentLength);
                o.Audio = audio;

                ds.SaveChanges();

         
                return mapper.Map<Track, TrackWithDetailsViewModel>(o);
            }
        }

        public bool TrackDelete(int id)
        {
       
            var itemToDelete = ds.Tracks.Find(id);

            if (itemToDelete == null)
            {
                return false;
            }
            else
            {
                ds.Tracks.Remove(itemToDelete);
                ds.SaveChanges();

                return true;
            }
        }


        // MEDIA ITEM

        public ArtistWithMediaViewModel AddMediaItem(MediaItemAddViewModel newItem)
        {
            var artist = ds.Artists.Find(newItem.ArtistId);
            if (artist == null)
            {
                return null;
            }
            else
            {
                var obj = ds.MediaItems.Add(mapper.Map<MediaItemAddViewModel, MediaItem>(newItem));
                byte[] b = new byte[newItem.Upload.ContentLength];

                newItem.Upload.InputStream.Read(b, 0, newItem.Upload.ContentLength);

                obj.Content = b;
                obj.ContentType = newItem.Upload.ContentType;

                artist.MediaItems.Add(obj);

                ds.SaveChanges();

                return (artist == null) ? null : mapper.Map<Artist, ArtistWithMediaViewModel>(artist);
            }
        }

        public MediaItemWithDetailViewModel MediaGetById(int id)
        {
            var o = ds.MediaItems.Find(id);
            return (o == null) ? null : mapper.Map<MediaItem, MediaItemWithDetailViewModel>(o);
        }



        public bool LoadData()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Monitor the progress
            bool done = false;

            // ############################################################
            // Role claims

            if (ds.RoleClaims.Count() == 0)
            {
                ds.RoleClaims.Add(new RoleClaim { Name = "Executive" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Coordinator" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Clerk" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Staff" });

                done = true;
            }

                // ############################################################
                // Genre

                if (ds.Genres.Count() == 0)
            {
                // Add genres

                ds.Genres.Add(new Genre { Name = "Alternative" });
                ds.Genres.Add(new Genre { Name = "Classical" });
                ds.Genres.Add(new Genre { Name = "Country" });
                ds.Genres.Add(new Genre { Name = "Easy Listening" });
                ds.Genres.Add(new Genre { Name = "Hip-Hop/Rap" });
                ds.Genres.Add(new Genre { Name = "Jazz" });
                ds.Genres.Add(new Genre { Name = "Pop" });
                ds.Genres.Add(new Genre { Name = "R&B" });
                ds.Genres.Add(new Genre { Name = "Rock" });
                ds.Genres.Add(new Genre { Name = "Soundtrack" });

                ds.SaveChanges();
                done = true;
            }

            // ############################################################
            // Artist

            if (ds.Artists.Count() == 0)
            {
                // Add artists

                ds.Artists.Add(new Artist
                {
                    Name = "The Beatles",
                    BirthOrStartDate = new DateTime(1962, 8, 15),
                    Executive = user,
                    Genre = "Pop",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/9/9f/Beatles_ad_1965_just_the_beatles_crop.jpg"
                });

                ds.Artists.Add(new Artist
                {
                    Name = "Adele",
                    BirthName = "Adele Adkins",
                    BirthOrStartDate = new DateTime(1988, 5, 5),
                    Executive = user,
                    Genre = "Pop",
                    UrlArtist = "http://www.billboard.com/files/styles/article_main_image/public/media/Adele-2015-close-up-XL_Columbia-billboard-650.jpg"
                });

                ds.Artists.Add(new Artist
                {
                    Name = "Bryan Adams",
                    BirthOrStartDate = new DateTime(1959, 11, 5),
                    Executive = user,
                    Genre = "Rock",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/7/7e/Bryan_Adams_Hamburg_MG_0631_flickr.jpg"
                });

                ds.SaveChanges();
                done = true;
            }

            // ############################################################
            // Album

            if (ds.Albums.Count() == 0)
            {
                // Add albums

                // For Bryan Adams
                var bryan = ds.Artists.SingleOrDefault(a => a.Name == "Bryan Adams");

                ds.Albums.Add(new Album
                {
                    Artists = new List<Artist> { bryan },
                    Name = "Reckless",
                    ReleaseDate = new DateTime(1984, 11, 5),
                    Coordinator = user,
                    Genre = "Rock",
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/5/56/Bryan_Adams_-_Reckless.jpg"
                });

                ds.Albums.Add(new Album
                {
                    Artists = new List<Artist> { bryan },
                    Name = "So Far So Good",
                    ReleaseDate = new DateTime(1993, 11, 2),
                    Coordinator = user,
                    Genre = "Rock",
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/pt/a/ab/So_Far_so_Good_capa.jpg"
                });

                ds.SaveChanges();
                done = true;
            }

            // ############################################################
            // Track

            if (ds.Tracks.Count() == 0)
            {
                // Add tracks

                // For Reckless
                var reck = ds.Albums.SingleOrDefault(a => a.Name == "Reckless");

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Run To You",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Heaven",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Somebody",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Summer of '69",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Kids Wanna Rock",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                // For Reckless
                var so = ds.Albums.SingleOrDefault(a => a.Name == "So Far So Good");

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "Straight from the Heart",
                    Composers = "Bryan Adams, Eric Kagna",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "It's Only Love",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "This Time",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "(Everything I Do) I Do It for You",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "Heat of the Night",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.SaveChanges();
                done = true;
            }

            return done;
        }

        public bool RemoveData()
        {
            try
            {
                foreach (var e in ds.RoleClaims)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Tracks)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Albums)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Artists)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Genres)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveDatabase()
        {
            try
            {
                return ds.Database.Delete();
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

    // New "RequestUser" class for the authenticated user
    // Includes many convenient members to make it easier to render user account info
    // Study the properties and methods, and think about how you could use it

    // How to use...

    // In the Manager class, declare a new property named User
    //public RequestUser User { get; private set; }

    // Then in the constructor of the Manager class, initialize its value
    //User = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);

    public class RequestUser
    {
        // Constructor, pass in the security principal
        public RequestUser(ClaimsPrincipal user)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                Principal = user;

                // Extract the role claims
                RoleClaims = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                // User name
                Name = user.Identity.Name;

                // Extract the given name(s); if null or empty, then set an initial value
                string gn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
                if (string.IsNullOrEmpty(gn)) { gn = "(empty given name)"; }
                GivenName = gn;

                // Extract the surname; if null or empty, then set an initial value
                string sn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname).Value;
                if (string.IsNullOrEmpty(sn)) { sn = "(empty surname)"; }
                Surname = sn;

                IsAuthenticated = true;
                // You can change the string value in your app to match your app domain logic
                IsAdmin = user.HasClaim(ClaimTypes.Role, "Admin") ? true : false;
            }
            else
            {
                RoleClaims = new List<string>();
                Name = "anonymous";
                GivenName = "Unauthenticated";
                Surname = "Anonymous";
                IsAuthenticated = false;
                IsAdmin = false;
            }

            // Compose the nicely-formatted full names
            NamesFirstLast = $"{GivenName} {Surname}";
            NamesLastFirst = $"{Surname}, {GivenName}";
        }

        // Public properties
        public ClaimsPrincipal Principal { get; private set; }
        public IEnumerable<string> RoleClaims { get; private set; }

        public string Name { get; set; }

        public string GivenName { get; private set; }
        public string Surname { get; private set; }

        public string NamesFirstLast { get; private set; }
        public string NamesLastFirst { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public bool IsAdmin { get; private set; }

        public bool HasRoleClaim(string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(ClaimTypes.Role, value) ? true : false;
        }

        public bool HasClaim(string type, string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(type, value) ? true : false;
        }
    }

}