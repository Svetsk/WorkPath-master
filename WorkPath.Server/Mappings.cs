using AutoMapper;
using WorkPath.Server.EditModels;
using WorkPath.Server.Entities;
using WorkPath.Server.ViewModels;

namespace WorkPath.Server;

public class Mappings: Profile
{
    public Mappings()
    {
        CreateMap<UserEditModel, UserViewModel>().ReverseMap();
        CreateMap<UserEditModel, UserEntity>().ReverseMap();
        CreateMap<UserViewModel, UserEntity>().ReverseMap();
        
        CreateMap<EducationEditModel, EducationViewModel>().ReverseMap();
        CreateMap<EducationEditModel, EducationEntity>().ReverseMap();
        CreateMap<EducationViewModel, EducationEntity>().ReverseMap();
        
        CreateMap<CompanyEditModel, CompanyViewModel>().ReverseMap();
        CreateMap<CompanyEditModel, CompanyEntity>().ReverseMap();
        CreateMap<CompanyViewModel, CompanyEntity>().ReverseMap();
        
        CreateMap<JobEditModel, JobViewModel>().ReverseMap();
        CreateMap<JobEditModel, JobEntity>().ReverseMap();
        CreateMap<JobViewModel, JobEntity>().ReverseMap();
    }
}