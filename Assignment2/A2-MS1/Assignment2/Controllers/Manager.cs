using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Assignment2.EntityModels;
using Assignment2.Models;

namespace Assignment2.Controllers
{
    public class Manager
    {
        // Reference to the data context
        private DataContext ds = new DataContext();

        // AutoMapper instance
        public IMapper mapper;

        public Manager()
        {
           
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Album, AlbumBaseViewModel>();
                cfg.CreateMap<Artist, ArtistBaseViewModel>();
                cfg.CreateMap<MediaType, MediaTypeBaseViewModel>();
                cfg.CreateMap<Track, TrackBaseViewModel>();
                cfg.CreateMap<Track, TrackWithDetailViewModel>();
                cfg.CreateMap<TrackAddViewModel, Track>();


            });

            mapper = config.CreateMapper();

          
            ds.Configuration.ProxyCreationEnabled = false;

            ds.Configuration.LazyLoadingEnabled = false;
        }


        public IEnumerable<AlbumBaseViewModel> AlbumGetAll()
        {
            return mapper.Map<IEnumerable<Album>, IEnumerable<AlbumBaseViewModel>>(ds.Albums.OrderBy(c => c.Title));
        }
 

        public AlbumBaseViewModel AlbumGetById(int id)
        {
            var obj = ds.Albums.Find(id);

            return obj == null ? null : mapper.Map<Album, AlbumBaseViewModel>(obj);
        }


        public IEnumerable<MediaTypeBaseViewModel> MediaTypeGetAll()
        {
            return mapper.Map<IEnumerable<MediaType>, IEnumerable<MediaTypeBaseViewModel>>(ds.MediaTypes.OrderBy(c => c.MediaTypeId));
        }

      
        public MediaTypeBaseViewModel MediaTypeGetById(int id)
        {
            var obj = ds.MediaTypes.Find(id);

            return obj == null ? null : mapper.Map<MediaType, MediaTypeBaseViewModel>(obj);
        }


        public IEnumerable<ArtistBaseViewModel> ArtistGetAll()
        {
            return mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistBaseViewModel>>(ds.Artists.OrderBy(c => c.ArtistId));
        }


        public IEnumerable<TrackWithDetailViewModel> TrackGetAllWithDetail()
        {
            var obj = ds.Tracks.Include("Album.Artist").Include("MediaType").OrderBy(c => c.Name);

            return mapper.Map<IEnumerable<Track>, IEnumerable<TrackWithDetailViewModel>>(obj);
        }


        public TrackWithDetailViewModel TrackGetByIdWithDetail(int id)
        {
            var obj = ds.Tracks.Include("Album.Artist").Include("MediaType").SingleOrDefault(c => c.TrackId == id);

            return obj == null ? null : mapper.Map<Track, TrackWithDetailViewModel>(obj);
        }

        public TrackWithDetailViewModel TrackAdd(TrackAddViewModel newItem)
        {
            var isAlbum = ds.Albums.Find(newItem.AlbumId);
            var isMedia = ds.MediaTypes.Find(newItem.MediaTypeId);

            if (isAlbum == null || isMedia == null)
            {
                return null;
            }
            else
            {
                var newObject = ds.Tracks.Add(mapper.Map<TrackAddViewModel, Track>(newItem));
                newObject.Album = isAlbum;
                newObject.MediaType = isMedia;
                ds.SaveChanges();

                return (newObject == null) ? null : mapper.Map<Track, TrackWithDetailViewModel>(newObject);
            }

        }
    }
}