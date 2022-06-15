using AutoMapper;
using Input.ViewModels.Account;
using Input.ViewModels.Chapter;
using Input.ViewModels.Comment;
using Input.ViewModels.FanFiction;
using Input.ViewModels.Moderation;
using Input.ViewModels.Status;

namespace Input.ViewModels.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FandomViewModel, Models.Fandom>()
                .ForMember(b => b.Id, opt => opt.MapFrom(bvm => bvm.Id))
                .ForMember(b => b.Name, opt => opt.MapFrom(bvm => bvm.Name))
                .ReverseMap();
            CreateMap<ChapterViewModel, Models.Chapter>()
                .ForMember(b => b.Id, opt => opt.MapFrom(bvm => bvm.Id))
                .ForMember(b => b.Name, opt => opt.MapFrom(bvm => bvm.Name))
                .ForMember(b => b.Photo, opt => opt.MapFrom(bvm => bvm.Photo))
                .ForMember(b => b.ChapterBody, opt => opt.MapFrom(bvm => bvm.ChapterBody))
                .ForMember(b => b.FanFiction, opt => opt.MapFrom(bvm => bvm.FanFiction))
                .ForMember(b => b.FanFictionId, opt => opt.MapFrom(bvm => bvm.FanFictionId))
                .ReverseMap();
            CreateMap<ModerationViewModel, Models.Moderation>()
                .ForMember(b => b.Id, opt => opt.MapFrom(bvm => bvm.Id))
                .ForMember(b => b.Message, opt => opt.MapFrom(bvm => bvm.Message))
                .ForMember(b => b.Status, opt => opt.MapFrom(bvm => bvm.Status))
                .ForMember(b => b.StatusId, opt => opt.MapFrom(bvm => bvm.StatusId))
                .ForMember(b => b.User, opt => opt.MapFrom(bvm => bvm.User))
                .ForMember(b => b.UserId, opt => opt.MapFrom(bvm => bvm.UserId))
                .ForMember(b => b.ChangeTime, opt => opt.MapFrom(bvm => bvm.ChangeTime))
                .ReverseMap();
            CreateMap<StatusViewModel, Models.Status>()
                .ForMember(b => b.Id, opt => opt.MapFrom(bvm => bvm.Id))
                .ForMember(b => b.Name, opt => opt.MapFrom(bvm => bvm.Name))
                .ReverseMap();
            CreateMap<EditProfileViewModel, Models.User>()
                .ForMember(b => b.Id, opt => opt.MapFrom(bvm => bvm.Id))
                .ForMember(b => b.UserPhoto, opt => opt.MapFrom(bvm => bvm.Photo))
                .ForMember(b => b.RegistrationDate, opt => opt.Ignore())
                .ForMember(b => b.LastLoginDate, opt => opt.Ignore())
                .ForMember(b => b.Email, opt => opt.Ignore())
                .ForMember(b => b.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(b => b.EmailConfirmed, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<CommentViewModel, Models.Comment>()
                .ForMember(b => b.Id, opt => opt.MapFrom(bvm => bvm.Id))
                .ForMember(b => b.User, opt => opt.MapFrom(bvm => bvm.User))
                .ForMember(b => b.UserId, opt => opt.MapFrom(bvm => bvm.UserId))
                .ForMember(b => b.CommentBody, opt => opt.MapFrom(bvm => bvm.CommentBody))
                .ForMember(b => b.SendingTime, opt => opt.MapFrom(bvm => bvm.SendingTime))
                .ForMember(b => b.FanFiction, opt => opt.MapFrom(bvm => bvm.FanFiction))
                .ForMember(b => b.FanFictionId, opt => opt.MapFrom(bvm => bvm.FanFictionId))
                .ReverseMap();
            CreateMap<FanFictionViewModel, Models.FanFiction>()
                .ForMember(b => b.Id, opt => opt.MapFrom(bvm => bvm.Id))
                .ForMember(b => b.Name, opt => opt.MapFrom(bvm => bvm.Name))
                .ForMember(b => b.Fandom, opt => opt.MapFrom(bvm => bvm.Fandom))
                .ForMember(b => b.FandomId, opt => opt.MapFrom(bvm => bvm.FandomId))
                .ForMember(b => b.Moderation, opt => opt.MapFrom(bvm => bvm.Moderation))
                .ForMember(b => b.ModerationId, opt => opt.MapFrom(bvm => bvm.ModerationId))
                .ForMember(b => b.Photo, opt => opt.MapFrom(bvm => bvm.Photo))
                .ForMember(b => b.ShortDescription, opt => opt.MapFrom(bvm => bvm.ShortDescription))
                .ForMember(b => b.UserId, opt => opt.MapFrom(bvm => bvm.UserId))
                .ForMember(b => b.User, opt => opt.MapFrom(bvm => bvm.User))
                .ForMember(b => b.UserRating, opt => opt.MapFrom(bvm => bvm.UserRating))
                .ReverseMap();
        }
    }
}