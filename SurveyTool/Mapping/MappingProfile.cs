using AutoMapper;
using SurveyTool.Models;
using SurveyTool.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyTool.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMapFromViewModelToEntities();
            CreateMapFromEntitiesToViewModel();
        }
        private void CreateMapFromViewModelToEntities()
        {
            //câu lệnh cho phép bỏ qua các trường mà viewmodel không có
            CreateMap<SurveyViewModel, Survey>();
        }
        private void CreateMapFromEntitiesToViewModel()
        {
            CreateMap<Survey, SurveyViewModel>();
        }

    }

}