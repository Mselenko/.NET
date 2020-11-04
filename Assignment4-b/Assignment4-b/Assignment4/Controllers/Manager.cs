using Assignment4.Models;
// new...
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Assignment4.Controllers
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
                cfg.CreateMap<AlbumAddViewModel, Album>();

                cfg.CreateMap<Artist, ArtistBaseViewModel>();
                cfg.CreateMap<Artist, ArtistWithDetailViewModel>();
                cfg.CreateMap<ArtistAddViewModel, Artist>();

                cfg.CreateMap<Track, TrackBaseViewModel>();
                cfg.CreateMap<Track, TrackWithDetailsViewModel>();
                cfg.CreateMap<TrackAddViewModel, Track>();

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

        //ARTIST

        public IEnumerable<ArtistBaseViewModel> GetAllArtists()
        {
            var obj = ds.Artists.OrderBy(c => c.Name);
            return mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistBaseViewModel>>(obj);
        }

        public ArtistWithDetailViewModel ArtistGetById(int id)
        {
            var obj = ds.Artists.Include("Albums").SingleOrDefault(c => c.Id == id);
            return obj == null ? null : mapper.Map<Artist, ArtistWithDetailViewModel>(obj);
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
            var addedItem = ds.Albums.Add(mapper.Map<AlbumAddViewModel, Album>(newItem));


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

            var trackList = new List<Track>();

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


        public TrackWithDetailsViewModel AddNewTrack(TrackAddViewModel newItem)
        {
            var album = ds.Albums.Find(newItem.AlbumId);

            if (album == null)
            {
                return null;
            }
            else
            {
                var addedTrack = ds.Tracks.Add(mapper.Map<TrackAddViewModel, Track>(newItem));
                addedTrack.Albums.Add(album);
                addedTrack.Clerk = User.Name;

                ds.SaveChanges();
                return (addedTrack == null) ? null : mapper.Map<Track, TrackWithDetailsViewModel>(addedTrack);
            }
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



        //GENRE

        public IEnumerable<GenreBaseViewModel> GetAllGenres()
        {
            var obj = ds.Genres.OrderBy(c => c.Name);
            return mapper.Map<IEnumerable<Genre>, IEnumerable<GenreBaseViewModel>>(obj);
        }



        public bool LoadData()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Monitor the progress
            bool done = false;

            // Role claims

            if (ds.RoleClaims.Count() == 0)
            {
                ds.RoleClaims.Add(new RoleClaim { Name = "Executive" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Coordinator" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Clerk" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Staff" });

                done = true;
            }

            //Genre

            if (ds.Genres.Count() == 0)
            {
                ds.Genres.Add(new Genre { Name = "Rock" });
                ds.Genres.Add(new Genre { Name = "Pop" });
                ds.Genres.Add(new Genre { Name = "Jazz" });
                ds.Genres.Add(new Genre { Name = "Blues" });
                ds.Genres.Add(new Genre { Name = "House" });
                ds.Genres.Add(new Genre { Name = "Rap" });
                ds.Genres.Add(new Genre { Name = "Lo-Fi" });
                ds.Genres.Add(new Genre { Name = "Electro" });
                ds.Genres.Add(new Genre { Name = "Indie" });
                ds.Genres.Add(new Genre { Name = "Classical" });

                done = true;
            }


            if (ds.Artists.Count() == 0)
            {
                ds.Artists.Add(new Artist
                {
                    Name = "Queen",
                    BirthName = "",
                    BirthOrStartDate = new DateTime(1970, 01, 01),
                    Executive = "exec@example.com",
                    Genre = "Rock",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/3/33/Queen_%E2%80%93_montagem_%E2%80%93_new.png"
                });

                ds.Artists.Add(new Artist
                {
                    Name = "Rick Astley",
                    BirthName = "Richard Paul Astley",
                    BirthOrStartDate = new DateTime(1966, 02, 06),
                    Executive = "exec@example.com",
                    Genre = "Pop",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/4/41/Rick_Astley_Dallas.jpg"
                });

                ds.Artists.Add(new Artist
                {
                    Name = "B.B. King",
                    BirthName = "Riley B. King",
                    BirthOrStartDate = new DateTime(1925, 09, 16),
                    Executive = "exec@example.com",
                    Genre = "Jazz",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/5/55/B.B._King_%2846264650642%29.jpg"
                });

            }


            //Albums

            if (ds.Albums.Count() == 0)
            {

                ICollection<Artist> queen = new List<Artist>();
                queen.Add(ds.Artists.FirstOrDefault(a => a.Id == 1));

                ds.Albums.Add(new Album
                {
                    Artists = queen,
                    Coordinator = "coord@example.com",
                    Genre = "Rock",
                    Name = "Queen",
                    ReleaseDate = new DateTime(1973, 07, 13),
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/0/03/Queen_Queen.png"
                });

                ds.Albums.Add(new Album
                {
                    Artists = queen,
                    Name = "Innuendo",
                    ReleaseDate = new DateTime(1991, 02, 05),
                    Coordinator = "coord@example.com",
                    Genre = "Rock",
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/f/f7/Queen_Innuendo.png"
                });



                ICollection<Artist> rick = new List<Artist>();
                rick.Add(ds.Artists.FirstOrDefault(a => a.Id == 2));

                ds.Albums.Add(new Album
                {
                    Artists = rick,
                    Name = "Beautiful Life",
                    ReleaseDate = new DateTime(2018, 07, 13),
                    Coordinator = "coord@example.com",
                    Genre = "Pop",
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/9/95/Beautiful_Life_Rick_Astley.jpg"
                });

                ds.Albums.Add(new Album
                {
                    Artists = rick,
                    Name = "Free",
                    ReleaseDate = new DateTime(1999, 01, 13),
                    Coordinator = "coord@example.com",
                    Genre = "Pop",
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/6/69/Free_%28Rick_Astley_album%29.jpg"
                });



                ICollection<Artist> king = new List<Artist>();
                king.Add(ds.Artists.FirstOrDefault(a => a.Id == 3));

                ds.Albums.Add(new Album
                {
                    Artists = king,
                    Name = "Live at the Regal",
                    ReleaseDate = new DateTime(1965, 05, 29),
                    Coordinator = "coord@example.com",
                    Genre = "Jazz",
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/2/2f/BB_King-Live_at_the_Regal_%28album_cover%29.jpg"
                });

                ds.Albums.Add(new Album
                {
                    Artists = king,
                    Name = "Lucille",
                    ReleaseDate = new DateTime(1968, 09, 20),
                    Coordinator = "coord@example.com",
                    Genre = "Jazz",
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/2/25/Lucille_%28B.B._King_album%29_cover.jpg"


                });

            }


            // Tracks
            if (ds.Tracks.Count() == 0)
            {

                ICollection<Album> innuendo = new List<Album>();
                innuendo.Add(ds.Albums.FirstOrDefault(a => a.Id == 16));

                ds.Tracks.Add(new Track
                {
                    Albums = innuendo,
                    Name = "Innuendo",
                    Composers = "David Richards",
                    Genre = "Rock",
                    Clerk = "clerk@example.com"

                });

                ds.Tracks.Add(new Track
                {
                    Albums = innuendo,
                    Name = "I'm going slightly mad",
                    Composers = "David Richards",
                    Genre = "Rock",
                    Clerk = "clerk@example.com"

                });


                ds.Tracks.Add(new Track
                {
                    Albums = innuendo,
                    Name = "Headlong",
                    Composers = "David Richards",
                    Genre = "Rock",
                    Clerk = "clerk@example.com"

                });



                ICollection<Album> free = new List<Album>();
                free.Add(ds.Albums.FirstOrDefault(a => a.Id == 18));



                ds.Tracks.Add(new Track
                {
                    Albums = free,
                    Name = "Free",
                    Composers = "Rick Astley",
                    Genre = "Pop",
                    Clerk = "clerk@example.com"

                });

                ds.Tracks.Add(new Track
                {
                    Albums = free,
                    Name = "Cry for Help",
                    Composers = "Rick Astley",
                    Genre = "Pop",
                    Clerk = "clerk@example.com"

                });


                ds.Tracks.Add(new Track
                {
                    Albums = free,
                    Name = "Move Right Out",
                    Composers = "Rick Astley",
                    Genre = "Pop",
                    Clerk = "clerk@example.com"

                });



                ICollection<Album> live = new List<Album>();
                live.Add(ds.Albums.FirstOrDefault(a => a.Id == 19));


                ds.Tracks.Add(new Track
                {
                    Albums = live,
                    Name = "Please Love Me",
                    Composers = "B.B. King",
                    Genre = "Jazz",
                    Clerk = "clerk@example.com"

                });

                ds.Tracks.Add(new Track
                {
                    Albums = live,
                    Name = "Sweet Little Angel",
                    Composers = "B.B. King",
                    Genre = "Jazz",
                    Clerk = "clerk@example.com"

                });


                ds.Tracks.Add(new Track
                {
                    Albums = live,
                    Name = "Worry, Worry",
                    Composers = "B.B. King",
                    Genre = "Jazz",
                    Clerk = "clerk@example.com"

                });

                ds.Tracks.Add(new Track
                {
                    Albums = live,
                    Name = "How Blue Can You Get?",
                    Composers = "B.B. King",
                    Genre = "Jazz",
                    Clerk = "clerk@example.com"

                });

            }

            ds.SaveChanges();

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