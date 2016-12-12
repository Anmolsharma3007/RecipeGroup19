using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Group19.AspNetIdentity;
using Group19.Recipes1.DataAccess.Models;
using Group19.Recipes1.Models;

namespace Group19.Recipes1.App_Start
{
    public class MapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Category, CategoryViewModel>().ReverseMap();
                cfg.CreateMap<Measurement, MeasurementViewModel>().ReverseMap();
                cfg.CreateMap<Product, ProductViewModel>().ReverseMap();
                cfg.CreateMap<Ingredient, IngredientViewModel>()
                    .ForMember(dest => dest.MeasurementViewModel, opt => opt.MapFrom(src => src.Measurement))
                    .ReverseMap();
                cfg.CreateMap<Recipe, RecipeViewModel>()
                    .ForMember(dest => dest.IngredientViewModels, opt => opt.MapFrom(src => src.Ingredients))
                    .ForMember(dest => dest.CategoryViewModels, opt => opt.MapFrom(src => src.Categories))
                    .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.UserName))
                    .ReverseMap()
                    .ForMember(dest => dest.Author, opt => opt.Ignore());

                cfg.CreateMap<IdentityUser, UserViewModel>()
                    .ForMember(dest => dest.IsAdmin, opt => opt.ResolveUsing(src => src.Roles.Select(c => c.Name).Contains(Roles.Admin)))
                    .ForMember(dest => dest.IsEditor, opt => opt.ResolveUsing(src => src.Roles.Select(c => c.Name).Contains(Roles.Editor)))
                    .ReverseMap();
            });
        }
    }
}