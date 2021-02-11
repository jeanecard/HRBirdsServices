using AutoMapper;
using HRBirdEntity;
using HRBirdsEntities;
using HRBirdsModelDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRBirdService.Config
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<HRSubmitGenderDto, HRSubmitGender>().ReverseMap();
            CreateMap<HRSubmitAgeDto, HRSubmitAge>().ReverseMap();
            CreateMap<HRSubmitSourceDto, HRSubmitSource>().ReverseMap();
            CreateMap<HRSubmitPictureListItemDto, HRSubmitPictureListItem>().ReverseMap();
            CreateMap<HRSubmitPictureInput, HRSubmitPicture>().ReverseMap();
        }
    }
}
